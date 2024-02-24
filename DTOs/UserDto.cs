using Bangazon.Models;
using System.ComponentModel.DataAnnotations;

namespace Bangazon.DTOs
{
    public class UserDto
    {
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsSeller { get; set; }
        public string Uid { get; set; }
      
    }
}
