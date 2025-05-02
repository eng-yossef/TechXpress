using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.CategoryRepo
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // Basic test method
        string TestRepository();

        // Core category operations
        Task<IEnumerable<Category>> GetAllWithProductsAsync();
        Task<Category> GetByIdWithProductsAsync(int id);

        // Product count operations
        Task<int> GetProductCountAsync(int categoryId);
        Task<Dictionary<int, int>> GetProductCountForAllCategoriesAsync();

        // Advanced filtering
        Task<IEnumerable<Category>> GetFilteredCategoriesAsync(
            Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null);

        // Search operations
        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm);
    }
}