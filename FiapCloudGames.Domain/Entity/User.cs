using FiapCloudGames.Domain.Enums;

namespace FiapCloudGames.Domain.Entity
{
    public class User : BaseEntity
    {
        public User() { }
        public User(string name, string lastName, string email, string password, Role role = Role.Common)
        {
            Id = Guid.NewGuid();
            Name = name;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
            Status = Status.Active;
        }

        public void ActiveUser()
        {
            this.Status = Status.Active;
        }

        public void BlockUser()
        {
            this.Status = Status.Blocked;
        }

        public void BanUser()
        {
            this.Status = Status.Banned;
        }

        public void ChangeRoleToAdmin()
        {
            this.Role = Role.Admin;
        }

        public void ChangeRoleToCommon()
        {
            this.Role = Role.Common;
        }


        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; private set; }
        public Status Status { get; private set; }

        public IList<UsersGames> UsersGames { get; set; }
    }    
}
