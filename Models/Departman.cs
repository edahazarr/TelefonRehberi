using System.ComponentModel.DataAnnotations;

namespace TelefonRehberi.Models
{
    public class Departman
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Departman adı zorunludur")]
        public string Ad { get; set; }
    }
}
