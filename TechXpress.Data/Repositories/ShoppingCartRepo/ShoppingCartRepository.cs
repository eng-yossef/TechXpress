using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Models;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.ShoppingCartRepo
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(TechXpressDbContext context) : base(context)
        {
        }

        public async Task<ShoppingCart> GetByUserIdAsync(string userId)
        {
            return await _context.Set<ShoppingCart>()
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<ShoppingCart> GetWithItemsAsync(int cartId)
        {
            return await _context.Set<ShoppingCart>()
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<decimal> CalculateTotalAsync(int cartId)
        {
            return await _context.Set<ShoppingCart>()
                .Where(c => c.Id == cartId)
                .SelectMany(c => c.Items)
                .SumAsync(i => i.Product.Price * i.Quantity);
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cart = await _context.Set<ShoppingCart>()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart != null)
            {
                _context.Set<CartItem>().RemoveRange(cart.Items);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CartExistsForUserAsync(string userId)
        {
            return await _context.Set<ShoppingCart>()
                .AnyAsync(c => c.UserId == userId);
        }
    }
}