using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Repositories.CategoryRepo;
using TechXpress.Data.UnitOfWork;
using TechXpress.Services.GenericServices;

namespace TechXpress.Services.CategoriesService
{
    public class CategoryService : GenericService<Category>, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork)
            : base(unitOfWork.Categories)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = unitOfWork.Categories as ICategoryRepository;
        }

        public async Task<string> Test()
        {
            var categories = await _categoryRepository.GetAllWithProductsAsync();
            return $"{categories.Count()} categories loaded | Category Service is operational";
        }

        public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
        {
            return await _categoryRepository.GetAllWithProductsAsync();
        }

        public async Task<Category> GetByIdWithProductsAsync(int id)
        {
            return await _categoryRepository.GetByIdWithProductsAsync(id);
        }

        public async Task<int> GetProductCountAsync(int categoryId)
        {
            return await _categoryRepository.GetProductCountAsync(categoryId);
        }

        public async Task<Dictionary<int, int>> GetProductCountForAllCategoriesAsync()
        {
            return await _categoryRepository.GetProductCountForAllCategoriesAsync();
        }

        public async Task<IEnumerable<Category>> GetFilteredCategoriesAsync(
            Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null)
        {
            return await _categoryRepository.GetFilteredCategoriesAsync(
                filter,
                orderBy,
                includeProperties,
                skip,
                take);
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm)
        {
            return await _categoryRepository.SearchCategoriesAsync(searchTerm);
        }
    }
}