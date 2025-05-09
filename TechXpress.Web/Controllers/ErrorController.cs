using Microsoft.AspNetCore.Mvc;

namespace TechXpress.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/NotFound")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }
    }

}
