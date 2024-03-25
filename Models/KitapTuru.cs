using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebProjesi1.Models
{
    public class KitapTuru
    {


        [Key] //Primary Key
        public int Id { get; set; }


        [Required(ErrorMessage ="Kitap Türü Adı Boş Bırakılamaz!.")]//not null anlamına gelir 
        [MaxLength(25)] //en fazla 25 karakter girilebilir 
        [DisplayName("Kitap Türü Adı")]//label kısmı için bu gözükecek eklemedeki 
        public string? Ad { get; set; }



    }
}
