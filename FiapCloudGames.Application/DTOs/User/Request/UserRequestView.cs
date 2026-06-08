using FiapCloudGames.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.DTOs.User.Request
{
    public class UserRequestView
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
