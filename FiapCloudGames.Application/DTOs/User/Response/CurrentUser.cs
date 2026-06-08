using FiapCloudGames.Domain.Enums;

namespace FiapCloudGames.Application.DTOs.User.Response
{
    public class CurrentUser
    {
        public Guid Id { get; set; }
        public Role Role { get; set; }
    }
}
