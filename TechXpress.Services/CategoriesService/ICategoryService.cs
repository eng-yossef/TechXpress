using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Services.GenericServices;

namespace TechXpress.Services.CategoriesService
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task<string> Test();

        // Category-specific operations
        Task<IEnumerable<Category>> GetAllWithProductsAsync();
        Task<Category> GetByIdWithProductsAsync(int id);
        Task<int> GetProductCountAsync(int categoryId);
        Task<Dictionary<int, int>> GetProductCountForAllCategoriesAsync();

        // Advanced filtering
        Task<IEnumerable<Category>> GetFilteredCategoriesAsync(
            Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null);

        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm);
    }
}