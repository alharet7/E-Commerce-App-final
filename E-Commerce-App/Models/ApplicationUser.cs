using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_App.Models
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public IList<string> Roles { get; set; }
    }
}
