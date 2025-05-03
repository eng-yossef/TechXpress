using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Repositories.CartItemRepo;
using TechXpress.Data.Repositories.CategoryRepo;
using TechXpress.Data.Repositories.OrderDetailRepo;
using TechXpress.Data.Repositories.OrderRepo;
using TechXpress.Data.Repositories.ProductRepo;
using TechXpress.Data.Repositories.ReviewRepo;
using TechXpress.Data.Repositories.ShoppingCartRepo;

namespace TechXpress.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IReviewRepository Reviews { get; }

        ICartItemRepository CartItems { get; }

        IShoppingCartRepository ShoppingCarts { get; } 

        Task<int> CompleteAsync(); // Async version of SaveChanges


    }
}
