using Bangazon.Models;
using System.ComponentModel.DataAnnotations;

namespace Bangazon.DTOs
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsSeller { get; set; }

    }
}
