using Microsoft.AspNetCore.Mvc;

namespace TechXpress.Web.Controllers.Admin
{
    public class ProductsManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
