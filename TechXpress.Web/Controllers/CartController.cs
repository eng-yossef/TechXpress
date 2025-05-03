using Microsoft.AspNetCore.Mvc;

namespace TechXpress.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
