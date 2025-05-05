using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.GenericRepository;
using TechXpress.Data.Repositories.ProductRepo;
using TechXpress.Data.UnitOfWork;
using TechXpress.Services.GenericServices;

namespace TechXpress.Services.ProductsService
{
    public class ProductService : GenericService<ProductViewModel>, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductService(IUnitOfWork unitOfWork)
            : base(unitOfWork.Products)
        {
            _unitOfWork = unitOfWork;
            _productRepository = unitOfWork.Products as IProductRepository;
        }

        public TechXpressDbContext GetDbContext()
        {
            return _unitOfWork.Context;
        }

        public async Task<string> Test()
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();
            return string.Join(", ", products.Select(p => p.Name)) + " | Product Service runs successfully.";
        }

        public async Task<IEnumerable<ProductViewModel>> GetFeaturedProductsAsync(int count)
        {
            return await _productRepository.GetFeaturedProductsAsync(count);
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetProductsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<ProductViewModel>> SearchProductsAsync(string searchTerm)
        {
            return await _productRepository.SearchProductsAsync(searchTerm);
        }

        public async Task UpdateStockAsync(int productId, int quantityChange)
        {
            await _productRepository.UpdateStockAsync(productId, quantityChange);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> IsProductInStockAsync(int productId)
        {
            return await _productRepository.IsProductInStockAsync(productId);
        }

        public async Task<decimal> GetProductPriceAsync(int productId)
        {
            return await _productRepository.GetProductPriceAsync(productId);
        }

        public async Task ApplyDiscountAsync(int productId, decimal discountPercentage)
        {
            await _productRepository.ApplyDiscountAsync(productId, discountPercentage);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ProductViewModel>> GetFilteredProductsAsync(
            Expression<Func<ProductViewModel, bool>> filter = null,
            Func<IQueryable<ProductViewModel>, IOrderedQueryable<ProductViewModel>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null)
        {
            return await _productRepository.GetFilteredProductsAsync(
                filter,
                orderBy,
                includeProperties,
                skip,
                take);
        }

        public async Task<ProductViewModel> GetProductsWithCategoryAsync(int productId)
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();
            return products.FirstOrDefault(p => p.Id == productId);
        }





    }
}