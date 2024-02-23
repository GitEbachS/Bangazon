using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public int SellerId { get; set; }
    public User Seller { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public ICollection<Order> Orders { get; set; }
    public int CategoryId { get; set; }
}