namespace TechXpress.Web.ViewModels
{
    public class CartViewModel
    {
        public string CartId { get; set; }

        public List<CartItemViewModel> Items { get; set; } = new();

        public int TotalItems => Items?.Sum(i => i.Quantity) ?? 0;

        public decimal TotalPrice => Items?.Sum(i => i.Quantity * i.Product.Price) ?? 0m;
    }

   
}
