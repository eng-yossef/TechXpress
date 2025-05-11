using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Services.OrdersDetailsService;
using TechXpress.Services.OrdersService;
using TechXpress.Services.ProductsService;
using TechXpress.Services.ReviewsService;
using TechXpress.Web.Services.Interfaces;

namespace TechXpress.Web.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAIAssistantService _aiAssistantService;
        private readonly IAICommerceService _aiCommerceService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IReviewService _reviewService;


        public TestController(
            IProductService productService,
            IAIAssistantService aiAssistantService,
            IOrderService orderService,
            IOrderDetailsService orderDetailsService,
            IReviewService reviewService,
            IAICommerceService aiCommerceService)
        {
            _productService = productService;
            _aiAssistantService = aiAssistantService;
            _aiCommerceService = aiCommerceService;
            _orderDetailsService = orderDetailsService;
            _orderService = orderService;
            _reviewService = reviewService;
        }

        #region AI Assistant Actions
        public async Task<IActionResult> GetProductRecommendations()
        {
            // Get 5 featured products as recently viewed
            var viewedProducts = (await _productService.GetFeaturedProductsAsync(5)).ToList();
            if (!viewedProducts.Any())
                return Content("No products available for recommendations");

            var recommendations = await _aiAssistantService.GetProductRecommendations(viewedProducts);
            return Content(recommendations);
        }

        public async Task<IActionResult> RespondToCustomerQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
                return Content("Please provide a query");

            var response = await _aiAssistantService.GetCustomerQueryResponse(query);
            return Content(response);
        }
        #endregion

        #region AI Commerce Actions
        public async Task<IActionResult> GenerateProductDescription(int Id)
        {
            var product = await _productService.GetByIdAsync(Id);
            if (product == null)
                return Content($"Product {Id} not found");

            var description = await _aiCommerceService.GenerateProductDescriptions(product);
            // Remove all instances of **
            var cleanedDescription = description.Replace("**", "");
            return Content(cleanedDescription);
        }

        public async Task<IActionResult> AnalyzeProductSentiment(int productId)
        {
            // Simulate getting reviews by searching for product-related content
            var searchResults = await _productService.SearchProductsAsync($"Product {productId}");
            if (!searchResults.Any())
                return Content($"No information found for product {productId}");

            // Create mock reviews since we don't have direct access to reviews
            var mockReviews = new List<Review>
            {
                new Review { Comment = "Great product, works perfectly!", Rating = 5 },
                new Review { Comment = "Average quality for the price", Rating = 3 },
                new Review { Comment = "Not what I expected", Rating = 2 }
            };

            var analysis = await _aiCommerceService.AnalyzeCustomerSentiment(mockReviews);
            return Content(analysis);
        }

        [Authorize]
        public async Task<IActionResult> GetPersonalizedPromotions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Content("User not found");

            // Get user's purchased products as order history
            var purchasedProducts = await _productService.SearchProductsAsync($"user:{userId}");
            var mockOrderHistory = purchasedProducts.Select(p => new Order
            {
                Id = 1, // Mock order ID
                OrderDetails = new List<OrderDetail> { new OrderDetail { Product = p } }
            }).ToList();

            var mockCustomer = new ApplicationUser
            {
                Id = userId,
                UserName = User.Identity.Name,
                Email = User.FindFirstValue(ClaimTypes.Email)
            };

            var promotions = await _aiCommerceService.GeneratePersonalizedPromotions(mockCustomer, mockOrderHistory);
            return Content(promotions);
        }
        #endregion

        #region Utility Actions
        public IActionResult Test()
        {
            return Content("Test endpoint working");
        }

        public async Task<IActionResult> TestProductService()
        {
            var result = await _productService.Test();
            return Content(result);
        }

        [Authorize]
        public IActionResult ShowUserInfo()
        {
            var userInfo = new
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Username = User.Identity.Name,
                Email = User.FindFirstValue(ClaimTypes.Email)
            };

            return Json(userInfo);
        }
        #endregion

        #region Reports

        public async Task<IActionResult> GenerateSalesReport(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderService.GetAllAsync();

            var report = await _aiCommerceService.GenerateSalesPerformanceReport(orders.ToList(), startDate, endDate);
            return Json(report);
        }

        public async Task<IActionResult> GenerateInventoryReport()
        {
            // Create mock product data
            var products = await _productService.GetAllProductsWithCategoriesAsync();

            var report = await _aiCommerceService.GenerateInventoryAnalysisReport(products.ToList());
            return Json(report);
        }

        public async Task<IActionResult> GenerateCustomerReport()
        {
            var orders = await _orderService.GetAllAsync();
            if (orders == null || !orders.Any())
                return Content("No orders found to generate report");

            // Extract unique customers
            var customers = orders
                .Where(o => o.User != null)
                .Select(o => o.User)
                .DistinctBy(c => c.Id)
                .ToList();

            // Extract product IDs from all order details
            var productIds = orders
                .SelectMany(o => o.OrderDetails ?? new List<OrderDetail>())
                .Select(od => od.Product?.Id)
                .Where(id => id != null)
                .Distinct()
                .ToList();

            // Fetch full product details (with reviews)
            var productsWithReviews = new List<ProductViewModel>();
            foreach (var productId in productIds)
            {
                var product = await _productService.GetByIdAsync(productId.Value);
                if (product != null)
                    productsWithReviews.Add(product);
            }

            // Collect all reviews from those products
            var reviews = productsWithReviews
                .SelectMany(p => p.Reviews)
                .ToList();

            var report = await _aiCommerceService.GenerateCustomerBehaviorReport(customers, orders.ToList(), reviews);
            return Json(report);
        }
        #endregion

    }
}