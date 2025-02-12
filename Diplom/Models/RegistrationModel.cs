using System.ComponentModel.DataAnnotations;

namespace Diplom.Models
{
    public class RegistrationModel
    {
        [Required]
        public string? Login { get; set; }

        [Required]
        public string? Password { get; set;}

    }
}
