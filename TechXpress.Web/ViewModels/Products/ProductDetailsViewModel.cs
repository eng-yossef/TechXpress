using System.Collections.Generic;
using TechXpress.Data.Models;

namespace TechXpress.Web.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> SimilarProducts { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int CartItemCount { get; set; }
        public ReviewViewModel NewReview { get; set; } = new ReviewViewModel();
    }
}