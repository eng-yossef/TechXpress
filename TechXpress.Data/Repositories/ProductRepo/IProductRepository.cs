using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Data.Repositories.GenericRepository;

namespace TechXpress.Data.Repositories.ProductRepo
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        public string test();
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();



    }
}
