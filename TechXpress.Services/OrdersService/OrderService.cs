using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Repositories.OrderRepo;
using TechXpress.Data.UnitOfWork;
using TechXpress.Services.GenericServices;

namespace TechXpress.Services.OrdersService
{
    public class OrderService : GenericService<Order>, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IUnitOfWork unitOfWork)
            : base(unitOfWork.Orders)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = unitOfWork.Orders as IOrderRepository;
        }

        public async Task<string> Test()
        {
            var recentOrders = await _orderRepository.GetFilteredOrdersAsync(
                take: 3,
                orderBy: q => q.OrderByDescending(o => o.OrderDate));

            return $"Latest order: {recentOrders.FirstOrDefault()?.OrderNumber} | Order Service is operational";
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _orderRepository.GetOrderWithDetailsAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserAsync(userId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _orderRepository.GetOrdersByStatusAsync(status);
        }

        public async Task<Order> CreateOrderAsync(string userId, string shippingAddress, IEnumerable<OrderDetail> items)
        {
            var order = new Order
            {
                UserId = userId,
                ShippingAddress = shippingAddress,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                OrderDetails = items.ToList(),
                TotalAmount = items.Sum(i => i.Price * i.Quantity)
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus, string adminNotes = null)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus, adminNotes);
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddOrderNoteAsync(int orderId, string note, bool isAdminNote = false)
        {
            await _orderRepository.AddOrderNoteAsync(orderId, note, isAdminNote);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ProcessOrderPaymentAsync(int orderId, string paymentTransactionId)
        {
            var result = await _orderRepository.MarkOrderAsPaidAsync(orderId, paymentTransactionId);
            await _unitOfWork.CompleteAsync();
            return result;
        }

        public async Task<bool> VerifyOrderPaymentAsync(int orderId)
        {
            return await _orderRepository.CheckOrderPaymentStatusAsync(orderId);
        }

        public async Task<decimal> GetSalesTotalAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _orderRepository.GetTotalSalesAsync(startDate, endDate);
        }

        public async Task<int> GetOrderCountAsync(OrderStatus? status = null)
        {
            return await _orderRepository.GetOrderCountAsync(status);
        }

        public async Task<Dictionary<OrderStatus, int>> GetOrderStatusDistributionAsync()
        {
            return await _orderRepository.GetOrderStatusDistributionAsync();
        }

        public async Task<IEnumerable<Order>> GetFilteredOrdersAsync(
            Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null)
        {
            return await _orderRepository.GetFilteredOrdersAsync(
                filter,
                orderBy,
                includeProperties,
                skip,
                take);
        }
    }
}