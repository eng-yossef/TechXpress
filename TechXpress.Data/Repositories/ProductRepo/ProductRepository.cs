using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.ProductRepo
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(TechXpressDbContext context) : base(context)
        {
        }

        public string test()
        {
            return "Product Repository Runs Successfully";
        }

        public Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            throw new NotImplementedException();
        }
    }
}
