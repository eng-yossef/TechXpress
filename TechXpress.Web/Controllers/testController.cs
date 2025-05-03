using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechXpress.Services.CategoriesService;
using TechXpress.Services.ProductsService;

namespace TechXpress.Web.Controllers
{
    public class testController : Controller
    {
        private readonly IProductService _productService;
        // private readonly IOrderService _orderService;
        // private readonly IReviewService _reviewService;
        // private readonly IOrderDetailsService _orderDetailsService;
        // private readonly IShoppingCartService _shoppingCartService;
        // private readonly ICartItemService _cartItemService;
        private readonly ICategoryService _categoryService;

        public testController(
              // Add any required services here
              IProductService productService,
             ICategoryService categoryService
            // IOrderService orderService,
            // IReviewService reviewService,
            // IOrderDetailsService orderDetailsService,
            // IShoppingCartService shoppingCartService,
            // ICartItemService cartItemService,



            )
        {
            _productService = productService;
            // _orderService = orderService;
            // _reviewService = reviewService;
            // _orderDetailsService = orderDetailsService;
            // _shoppingCartService = shoppingCartService;
            // _cartItemService = cartItemService;
            _categoryService = categoryService;



            // Constructor logic here
        }
        public async Task<IActionResult>  Index()
        {
            // Example usage of the services
            var products = await _productService.GetAllAsync();
            // var orders = await _orderService.GetAllAsync();
            // var reviews = await _reviewService.GetAllAsync();
            // var orderDetails = await _orderDetailsService.GetAllAsync();
            // var shoppingCarts = await _shoppingCartService.GetAllAsync();
            // var cartItems = await _cartItemService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            var result = string.Join("\n", categories.Select(c =>
                $" {c.Id} {c.Name} {c.Description} "
            ));

            //return Content(result);
            return View();
        }
        [Authorize]
        public IActionResult ShowUserId()
        {
            // Example usage of the services
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Content(userId);
        }
    }
}
