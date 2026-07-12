namespace FiapCloudGames.Domain.Exceptions
{
    public class OrderAlreadyExistsException : Exception
    {
        public OrderAlreadyExistsException()
            : base("O usuário já adquiriu este jogo.")
        {
        }

        public OrderAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}