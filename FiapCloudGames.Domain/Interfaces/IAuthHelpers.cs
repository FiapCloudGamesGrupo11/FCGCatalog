namespace FiapCloudGames.Domain.Interfaces
{
    public interface IAuthHelpers
    {
        string GenerateJwtToken(string email, string role);
        string ComputeSha256Hash(string password);
    }
}
