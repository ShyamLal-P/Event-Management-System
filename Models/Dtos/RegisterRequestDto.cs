using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string[] Roles { get; set; }
    }
}
