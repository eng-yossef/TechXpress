using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Models;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.CategoryRepo
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(TechXpressDbContext context) : base(context)
        {
        }

        public string TestRepository()
        {
            return "Category Repository is operational";
        }

        public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
        {
            return await _context.Set<Category>()
                .Include(c => c.Products)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category> GetByIdWithProductsAsync(int id)
        {
            return await _context.Set<Category>()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> GetProductCountAsync(int categoryId)
        {
            return await _context.Set<Category>()
                .Where(c => c.Id == categoryId)
                .SelectMany(c => c.Products)
                .CountAsync();
        }

        public async Task<Dictionary<int, int>> GetProductCountForAllCategoriesAsync()
        {
            return await _context.Set<Category>()
                .Select(c => new
                {
                    CategoryId = c.Id,
                    ProductCount = c.Products.Count
                })
                .ToDictionaryAsync(x => x.CategoryId, x => x.ProductCount);
        }

        public async Task<IEnumerable<Category>> GetFilteredCategoriesAsync(
            Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null)
        {
            IQueryable<Category> query = _context.Set<Category>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.Set<Category>()
                .Where(c => c.Name.Contains(searchTerm) ||
                          (c.Description != null && c.Description.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}