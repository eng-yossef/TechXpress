using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Models;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.CartItemRepo
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(TechXpressDbContext context) : base(context)
        {
        }

        public async Task<CartItem> GetByProductAndCartAsync(int productId, int cartId)
        {
            return await _context.Set<CartItem>()
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.ShoppingCartId == cartId);
        }

        public async Task UpdateQuantityAsync(int itemId, int newQuantity)
        {
            var item = await _context.Set<CartItem>().FindAsync(itemId);
            if (item != null)
            {
                item.Quantity = newQuantity;
                _context.Set<CartItem>().Update(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CartItem>> GetItemsForCartAsync(int cartId)
        {
            return await _context.Set<CartItem>()
                .Include(i => i.Product)
                .Where(i => i.ShoppingCartId == cartId)
                .ToListAsync();
        }

        public async Task<bool> ProductExistsInCartAsync(int productId, int cartId)
        {
            return await _context.Set<CartItem>()
                .AnyAsync(i => i.ProductId == productId && i.ShoppingCartId == cartId);
        }
    }
}