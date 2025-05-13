using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TechXpress.Data.Models;

namespace TechXpress.Web.Extensions
{
    public static class PaymentStatusExtensions
    {
        public static string GetPaymentStatusBadgeClass(this PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "bg-warning",
                PaymentStatus.Authorized => "bg-info",
                PaymentStatus.Completed => "bg-success",
                PaymentStatus.Failed => "bg-danger",
                PaymentStatus.Refunded => "bg-secondary",
                PaymentStatus.PartiallyRefunded => "bg-primary",
                PaymentStatus.Voided => "bg-dark",
                PaymentStatus.Disputed => "bg-danger",
                _ => "bg-secondary"
            };
        }

        public static string GetDisplayName(this PaymentStatus status)
        {
            var field = status.GetType().GetField(status.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? status.ToString();
        }
    }
}
