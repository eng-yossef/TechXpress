using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Services.GenericServices;

namespace TechXpress.Services.ProductsService
{
    public interface IProductService : IGenericService<Product>
    {
        Task<string> Test();

        // Product-specific operations
        Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count);

        Task<Product> GetProductsWithCategoryAsync(int productId);

        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task UpdateStockAsync(int productId, int quantityChange);
        Task<bool> IsProductInStockAsync(int productId);
        Task<decimal> GetProductPriceAsync(int productId);
        Task ApplyDiscountAsync(int productId, decimal discountPercentage);

        // Advanced filtering
        Task<IEnumerable<Product>> GetFilteredProductsAsync(
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null);
    }
}