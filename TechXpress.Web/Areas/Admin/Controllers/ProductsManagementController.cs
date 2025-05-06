using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechXpress.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsManagementController : Controller
    {
        public IActionResult Index()
        {
            return Content("Admin Page");
        }
    }
}
