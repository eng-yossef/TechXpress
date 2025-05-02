using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.ProductRepo
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        // Basic test method
        public string test();

        // Core product operations
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
        Task<Product> GetProductWithCategoryAsync(int id);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task UpdateStockAsync(int productId, int quantity);
        Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds);

        // Advanced filtering
        Task<IEnumerable<Product>> GetFilteredProductsAsync(
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null);

        // Inventory management
        Task<int> GetProductStockAsync(int productId);
        Task<bool> IsProductInStockAsync(int productId);

        // Price operations
        Task<decimal> GetProductPriceAsync(int productId);
        Task ApplyDiscountAsync(int productId, decimal discountPercentage);
    }
}