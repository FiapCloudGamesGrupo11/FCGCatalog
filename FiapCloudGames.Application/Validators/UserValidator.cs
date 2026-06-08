using FiapCloudGames.Application.DTOs.User.Request;
using FluentValidation;

namespace FiapCloudGames.Application.Validators
{
    public class UserValidator : AbstractValidator<UserRequestView>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(2, 20).WithMessage("O nome deve conter entre 2 e 20 caracteres.");

            RuleFor(s => s.LastName)
                .NotEmpty().WithMessage("O Sobrenome é Obrigatório")
                .Length(2, 50).WithMessage("O sobrenome deve conter entre 2 e 50 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha Obrigatória")
            .MinimumLength(8).WithMessage("A senha deve conter pelo menos 8 caracteres")
            .Must(MinOneNumbers).WithMessage("A senha deve conter pelo menos 1 numeral")
            .Must(MinTwoEspecialChar).WithMessage("A senha deve conter pelo menos 2 caracteres especiais")
            .MaximumLength(50).WithMessage("A senha deve conter no máximo 50 caracteres");
        }

        private bool MinTwoEspecialChar(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                return false;

            int count = senha.Count(c => !char.IsLetterOrDigit(c));
            return count >= 2;
        }

        private bool MinOneNumbers(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                return false;

            int count = senha.Count(c => char.IsNumber(c));
            return count >= 1;
        }

    }
}
