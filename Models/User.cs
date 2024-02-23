using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public ICollection<Product> Products { get; set; }
  
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    public bool IsSeller { get; set; }
    public string Uid { get; set; }
}