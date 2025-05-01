using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Product
    {
        public int ProductId { get; set; }
       // [Required(ErrorMessage ="Name Is Required"),MaxLength(50)]
        public string ProductName { get; set; }
        [MaxLength(500)]
        public string ProductDescription { get; set; } = string.Empty;
        public Decimal Price { get; set; }
        public int Stock { get; set; } = 0;
        public string? ImageURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //ForeignKey
        public int CategoryId { get; set; }
        public Category? category { get; set; }

    }
}
