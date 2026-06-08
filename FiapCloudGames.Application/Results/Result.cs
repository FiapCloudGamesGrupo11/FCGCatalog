namespace FiapCloudGames.Application.Results
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Value { get; private set; }
        public List<string> Errors { get; private set; } = new();

        private Result() { }

        public static Result<T> Success(T value)
        {
            return new() { IsSuccess = true, Value = value };
        }

        public static Result<T> Failure(params string[] errors)
        {
            return new() { IsSuccess = false, Errors = errors.ToList() };
        }
    }
}
