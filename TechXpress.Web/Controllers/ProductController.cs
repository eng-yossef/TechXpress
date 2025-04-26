using Microsoft.AspNetCore.Mvc;
using TechXpress.Services.Product;

namespace TechXpress.Web.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
