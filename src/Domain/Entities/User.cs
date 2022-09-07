using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        // Custom field
        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        // Custom field
        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }
    }
}