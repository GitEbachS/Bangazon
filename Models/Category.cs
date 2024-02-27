using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models;

public class Category
{
    public int Id { get; set; }
    [Required]
    public ICollection<Product> Products { get; set; }

    public string Name { get; set; }
}