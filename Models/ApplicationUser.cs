using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebProjesi1.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int Ogrencino { get; set; }

        public string? Adres { get; set; }

        public string? Fakulte { get; set; }

        public string? Bolum { get; set; }

    }
}
