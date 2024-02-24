using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models;

public class Order
{
    public int Id { get; set; }
    [Required]
    public int CustomerId { get; set; }
    public string PaymentType { get; set; }
    public DateTime DateCreated { get; set; }
    public string Shipping { get; set; }
    public ICollection<Product> Products { get; set; }
    public decimal? TotalCost
    { 
        get
        {
            if (Products != null)
            {
             return Products.Sum(product => product.Price) ;
                
            }
                return null;
        }
            
}
    
    public bool IsClosed { get; set; }
}