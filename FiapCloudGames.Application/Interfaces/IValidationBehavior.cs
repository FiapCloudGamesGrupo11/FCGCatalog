namespace FiapCloudGames.Application.Interfaces
{
    public interface IValidationBehavior<T>
    {
        Task ValidateAsync(T request);
    }
}
