using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechXpress.Services.ProductsService;

namespace TechXpress.Web.Controllers
{
    //[Authorize]
    public class ProductsController : Controller
    {
        public readonly IProductService _productService;
        public ProductsController(IProductService productService)
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
