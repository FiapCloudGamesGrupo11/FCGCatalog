using FiapCloudGames.Application.Interfaces;
using FluentValidation;

namespace FiapCloudGames.Application.Behaviors
{
    public class ValidationBehavior<T> : IValidationBehavior<T>
    {
        private readonly IEnumerable<IValidator<T>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<T>> validators)
        {
            _validators = validators;
        }

        public async Task ValidateAsync(T request)
        {
            if (!_validators.Any())
                return;

            var context = new ValidationContext<T>(request);

            var results = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context))
            );

            var failures = results
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
        }
    }
}
