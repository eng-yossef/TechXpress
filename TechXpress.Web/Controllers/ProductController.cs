using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechXpress.Services.ProductsService;

namespace TechXpress.Web.Controllers
{
    //[Authorize]
    public class ProductController : Controller
    {
        public readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetByIdAsync(1);
            return Content(result.Name);
        }

    }
}
