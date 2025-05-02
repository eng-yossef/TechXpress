using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.ShoppingCartRepo
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        Task<ShoppingCart> GetByUserIdAsync(string userId);
        Task<ShoppingCart> GetWithItemsAsync(int cartId);
        Task<decimal> CalculateTotalAsync(int cartId);
        Task ClearCartAsync(int cartId);
        Task<bool> CartExistsForUserAsync(string userId);
    }
}