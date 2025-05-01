using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public string ShippingAddress { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public ICollection<OrderDetail> OrderDetails { get; set; }

  
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2
    }

    public enum OrderStatus
    {
        Pending = 0,
        Shipped = 1,
        Delivered = 2,
        Cancelled = 3
    }
}
