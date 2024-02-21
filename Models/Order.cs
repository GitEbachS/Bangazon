using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models;

public class Order
{
    public int Id { get; set; }
    [Required]
    public int CustomerId { get; set; }
    public string PaymentType { get; set; }
    public DateTime DateCreated { get; set; }
    public string shipping { get; set; }
    public bool IsClosed { get; set; }
}