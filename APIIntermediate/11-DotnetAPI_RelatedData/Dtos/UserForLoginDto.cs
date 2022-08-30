using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Dtos
{
    public class UserForLoginDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}