using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.UnitOfWork;

namespace TechXpress.Services.Product
{
    public class ProductService : IProductService
    {
        public readonly IUnitOfWork _unitOfWork;
        public ProductService(
            IUnitOfWork unitOfWork
            ) {
            _unitOfWork = unitOfWork;
        }
        public string test()
        {

            return    _unitOfWork.Products.test()+" Product Service Runs Successfully";
        }

    }
}
