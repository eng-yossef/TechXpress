using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechXpress.Services;
using TechXpress.Services.ProductsService;
using TechXpress.Services.ShoppingCartsService;

namespace TechXpress.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IProductService _productService;
        protected readonly IShoppingCartService _cartService;

        public BaseController(
            IProductService productService,
            IShoppingCartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        protected async Task<int> GetCartItemCountAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            return await _cartService.GetCartItemCountAsync(cart.Id);
        }
    }
}