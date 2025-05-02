using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Repositories.CategoryRepo;
using TechXpress.Data.Repositories.OrderRepo;
using TechXpress.Data.Repositories.OrederDetailsRepo;
using TechXpress.Data.Repositories.ProductRepo;
using TechXpress.Data.Repositories.ReviewRepo;

namespace TechXpress.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IReviewRepository Reviews { get; }

        Task<int> CompleteAsync(); // Async version of SaveChanges


    }
}
