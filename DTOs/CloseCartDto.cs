using Bangazon.Models;
using System.ComponentModel.DataAnnotations;

namespace Bangazon.DTOs
{
    public class CloseCartDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PaymentType { get; set; }
        public DateTime DateCreated { get; set; }
        public string Shipping { get; set; }

        public bool IsClosed { get; set; }
    }
}
