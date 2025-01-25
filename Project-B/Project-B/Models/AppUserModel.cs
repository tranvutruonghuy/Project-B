using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Project_B.Models
{
    public class AppUserModel : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

       
    }
}
