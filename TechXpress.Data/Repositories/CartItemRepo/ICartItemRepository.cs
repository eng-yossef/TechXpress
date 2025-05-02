using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.CartItemRepo
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<CartItem> GetByProductAndCartAsync(int productId, int cartId);
        Task UpdateQuantityAsync(int itemId, int newQuantity);
        Task<IEnumerable<CartItem>> GetItemsForCartAsync(int cartId);
        Task<bool> ProductExistsInCartAsync(int productId, int cartId);
    }
}