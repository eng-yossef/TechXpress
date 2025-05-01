using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Data.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }
        [MaxLength(1000)]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public Product Product { get; set; }
    }
}
