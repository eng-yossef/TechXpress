using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Services.GenericServices;

namespace TechXpress.Services.ShoppingCartsService
{
    public interface IShoppingCartService : IGenericService<ShoppingCart>
    {
        Task<string> Test();

        // Cart operations
        Task<ShoppingCart> GetCartByUserIdAsync(string userId);
        Task<ShoppingCart> GetCartWithItemsAsync(int cartId);
        Task<decimal> CalculateCartTotalAsync(int cartId);
        Task ClearCartAsync(int cartId);
        Task<bool> CartExistsForUserAsync(string userId);

        Task<int> GetCartItemCountAsync(int cartId);


        // Item management
        Task AddItemToCartAsync(int cartId, int productId, int quantity = 1);
        Task RemoveItemFromCartAsync(int cartId, int productId);
        Task UpdateItemQuantityAsync(int cartId, int productId, int newQuantity);

        // Checkout
        Task<Order> ConvertCartToOrderAsync(int cartId, string shippingAddress);

        // Advanced operations
        Task MergeCartsAsync(string userId, int temporaryCartId);
    }
}