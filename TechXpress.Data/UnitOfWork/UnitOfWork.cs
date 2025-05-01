using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.Category;
using TechXpress.Data.Repositories.Order;
using TechXpress.Data.Repositories.OrederDetails;
using TechXpress.Data.Repositories.Product;
using TechXpress.Data.Repositories.Review;

namespace TechXpress.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TechXpressDbContext _context;

        public IProductRepository Products { get;  set; }
        public ICategoryRepository Categories { get;  set; }
        public IOrderRepository Orders { get;  set; }
        public IOrderDetailsRepository OrderDetails { get;  set; }
        public IReviewRepository Reviews { get;  set; }

        public UnitOfWork(
            TechXpressDbContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IOrderRepository orderRepository,
            IOrderDetailsRepository orderDetailsRepository,
            IReviewRepository reviewRepository)
        {
            _context = context;
            Products = productRepository;
            Categories = categoryRepository;
            Orders = orderRepository;
            OrderDetails = orderDetailsRepository;
            Reviews = reviewRepository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
