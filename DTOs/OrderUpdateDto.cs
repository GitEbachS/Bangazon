using System.ComponentModel.DataAnnotations;
namespace Bangazon.DTOs
{
    public class OrderUpdateDto
    {
        public string PaymentType { get; set; }
        public string Shipping { get; set; }

        public bool IsClosed { get; set; }
    }
}
