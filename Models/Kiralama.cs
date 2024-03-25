using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjesi1.Models
{
    public class Kiralama
    {
      

        [Key]
        public int Id { get; set; }

        [Required]
        public int OgrenciId { get; set; }  

        [ValidateNever]
        public int KitapId { get; set; }
        [ForeignKey("KitapId")]

        [ValidateNever]
        public Kitap Kitap { get; set; }

        [Required(ErrorMessage = "Kitap adedi alanı zorunludur.")]
        [Display(Name = "Kitap Adedi")]
        public int kitapadedi { get; set; }



    }
}
