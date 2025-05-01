using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Repositories.Category;
using TechXpress.Data.Repositories.Order;
using TechXpress.Data.Repositories.OrederDetails;
using TechXpress.Data.Repositories.Product;
using TechXpress.Data.Repositories.Review;

namespace TechXpress.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IReviewRepository Reviews { get; }

        int Complete(); // equivalent to SaveChanges()
    }
}
