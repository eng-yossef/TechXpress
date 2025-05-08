using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Services.GenericServices; // Import your generic service interface
using TechXpress.Services.ShoppingCartsService; // Import your shopping cart service
using Microsoft.AspNetCore.Authorization;

namespace TechXpress.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerManagementController : Controller
    {
        private readonly IGenericService<ApplicationUser> _userService;
        private readonly IShoppingCartService _shoppingCartService;

        // Constructor injection of the services
        public CustomerManagementController(IGenericService<ApplicationUser> userService, IShoppingCartService shoppingCartService)
        {
            _userService = userService;
            _shoppingCartService = shoppingCartService;
        }

        // GET: Admin/CustomerManagement/Index
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync(); // Assuming you have a generic service to get all users
            return View(users); // View will display the list of customers
        }

        // GET: Admin/CustomerManagement/Details/{userId}
        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // Display detailed information about the user
        }

        // GET: Admin/CustomerManagement/Cart/{userId}
        public async Task<IActionResult> Cart(string userId)
        {
            var cart = await _shoppingCartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart); // Display the user's shopping cart
        }

        // POST: Admin/CustomerManagement/Delete/{userId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(user); // Delete user from the database
            return RedirectToAction(nameof(Index)); // Redirect to the customer list
        }

        // GET: Admin/CustomerManagement/Manage/{userId}
        public async Task<IActionResult> Manage(string userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // Show user management options, such as updating details
        }

        // POST: Admin/CustomerManagement/Update/{userId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string userId, ApplicationUser updatedUser)
        {
            if (userId != updatedUser.Id)
            {
                return BadRequest();
            }

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Update user details
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.AddressLine1 = updatedUser.AddressLine1;
            user.AddressLine2 = updatedUser.AddressLine2;
            user.City = updatedUser.City;
            user.State = updatedUser.State;
            user.PostalCode = updatedUser.PostalCode;
            user.Country = updatedUser.Country;

            await _userService.UpdateAsync(user); // Update the user in the database
            return RedirectToAction(nameof(Index)); // Redirect to the customer list
        }
    }
}
