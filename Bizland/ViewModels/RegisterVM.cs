using System.ComponentModel.DataAnnotations;

namespace BIZLAND.ViewModels
{
    public class RegisterVM
    {
        [Required]

        public string Name { get; set; }

        public string Surname { get; set; }
        [Required]

        public string Usename { get; set; }
        [Required]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPaswwod { get; set; }
    }
}
