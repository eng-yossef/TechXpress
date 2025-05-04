using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Services.CartItemsService;
using TechXpress.Services.ShoppingCartsService;
using TechXpress.Web.ViewModels;

namespace TechXpress.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IShoppingCartService _cartService;
        private readonly ICartItemService _cartItemService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string SessionCartIdKey = "CartId";

        public CartController(
            IShoppingCartService cartService,
            ICartItemService cartItemService,
            IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
            _httpContextAccessor = httpContextAccessor;
        }

        // ---------------------------- Core Actions ----------------------------

        public async Task<IActionResult> Index()
        {
            var cart = await GetOrCreateCartAsync(includeItems: true);
            if (cart == null || cart.Items == null || !cart.Items.Any())
                return View("Empty");

            var cartViewModel = new CartViewModel
            {
                CartId = cart.Id.ToString(),
                Items = cart.Items.Select(item => new CartItemViewModel
                {
                    ItemId = item.Id.ToString(),
                    Quantity = item.Quantity,
                    Product = new ProductViewModel
                    {
                        Id = item.ProductId,
                        Name = item.Product?.Name,
                        ImageUrl = item.Product?.ImageUrl,
                        Price = item.Product?.Price ?? 0m
                    }
                }).ToList()
            };

            return View(cartViewModel);
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var cart = await GetOrCreateCartAsync();
            await _cartService.AddItemToCartAsync(cart.Id, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var cart = await GetOrCreateCartAsync();
            await _cartService.UpdateItemQuantityAsync(cart.Id, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var cart = await GetOrCreateCartAsync();
            await _cartService.RemoveItemFromCartAsync(cart.Id, productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var cart = await GetOrCreateCartAsync();
            await _cartService.ClearCartAsync(cart.Id);
            return RedirectToAction("Index");
        }

        // ---------------------------- Merge Guest Cart on Login ----------------------------

        [Authorize]
        public async Task<IActionResult> MergeAfterLogin()
        {
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index");

            if (HttpContext.Session.TryGetValue(SessionCartIdKey, out byte[] sessionCartIdBytes))
            {
                var sessionCartId = System.Text.Encoding.UTF8.GetString(sessionCartIdBytes);
                await _cartService.MergeGuestCartWithUserCartAsync(sessionCartId, userId);
                HttpContext.Session.Remove(SessionCartIdKey);
            }

            return RedirectToAction("Index");
        }

        // ---------------------------- Helper Methods ----------------------------

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private async Task<ShoppingCart> GetOrCreateCartAsync(bool includeItems = false)
        {
            var userId = GetUserId();

            // Logged-in user
            if (!string.IsNullOrEmpty(userId))
            {
                return await _cartService.GetOrCreateUserCartAsync(userId, includeItems);
            }

            // Guest user (session-based)
            if (!HttpContext.Session.TryGetValue(SessionCartIdKey, out byte[] cartIdBytes))
            {
                var newCart = await _cartService.CreateGuestCartAsync();
                HttpContext.Session.SetString(SessionCartIdKey, newCart.Id.ToString());
                return await _cartService.GetCartByIdAsync(newCart.Id.ToString(), includeItems);
            }

            var cartId = System.Text.Encoding.UTF8.GetString(cartIdBytes);
            return await _cartService.GetCartByIdAsync(cartId, includeItems);
        }
    }
}