using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Models;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.OrderRepo
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(TechXpressDbContext context) : base(context)
        {
        }

        public string TestRepository()
        {
            return "Order Repository is operational";
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Set<Order>()
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
        {
            return await _context.Set<Order>()
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Set<Order>()
                .Where(o => o.OrderStatus == status)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalSalesAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Set<Order>()
                .Where(o => o.PaymentStatus == PaymentStatus.Completed);

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value);

            return await query.SumAsync(o => o.TotalAmount);
        }

        public async Task<int> GetOrderCountAsync(OrderStatus? status = null)
        {
            var query = _context.Set<Order>().AsQueryable();

            if (status.HasValue)
                query = query.Where(o => o.OrderStatus == status.Value);

            return await query.CountAsync();
        }

        public async Task<Dictionary<OrderStatus, int>> GetOrderStatusDistributionAsync()
        {
            return await _context.Set<Order>()
                .GroupBy(o => o.OrderStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<IEnumerable<Order>> GetFilteredOrdersAsync(
            Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null)
        {
            IQueryable<Order> query = _context.Set<Order>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus, string adminNotes = null)
        {
            var order = await _context.Set<Order>().FindAsync(orderId);
            if (order != null)
            {
                order.OrderStatus = newStatus;
                order.LastUpdated = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(adminNotes))
                {
                    order.AdminNotes = string.IsNullOrEmpty(order.AdminNotes)
                        ? adminNotes
                        : $"{order.AdminNotes}\n{adminNotes}";
                }

                _context.Set<Order>().Update(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddOrderNoteAsync(int orderId, string note, bool isAdminNote = false)
        {
            var order = await _context.Set<Order>().FindAsync(orderId);
            if (order != null)
            {
                if (isAdminNote)
                {
                    order.AdminNotes = string.IsNullOrEmpty(order.AdminNotes)
                        ? note
                        : $"{order.AdminNotes}\n{note}";
                }
                else
                {
                    order.CustomerNotes = string.IsNullOrEmpty(order.CustomerNotes)
                        ? note
                        : $"{order.CustomerNotes}\n{note}";
                }

                order.LastUpdated = DateTime.UtcNow;
                _context.Set<Order>().Update(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MarkOrderAsPaidAsync(int orderId, string paymentTransactionId)
        {
            var order = await _context.Set<Order>()
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return false;

            order.PaymentStatus = PaymentStatus.Completed;
            order.LastUpdated = DateTime.UtcNow;

            _context.Set<Order>().Update(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckOrderPaymentStatusAsync(int orderId)
        {
            return await _context.Set<Order>()
                .AnyAsync(o => o.Id == orderId && o.PaymentStatus == PaymentStatus.Completed);
        }
    }
}