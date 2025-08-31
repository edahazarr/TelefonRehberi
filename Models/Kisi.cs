using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace TelefonRehberi.Models

{public class Kisi
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    public string Ad { get; set; }

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    public string Soyad { get; set; }

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string Telefon { get; set; }

    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Departman alanı zorunludur.")]
    public string Departman { get; set; }
    
    
    public string? PasswordHash { get; set; }

    
}

    }
