using Microsoft.AspNetCore.Mvc;

namespace TechXpress.Web.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
