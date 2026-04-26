using FluentValidation;
using Prestacontrol.Application.DTOs;

namespace Prestacontrol.Application.Validators
{
    public class ClientValidator : AbstractValidator<ClientDto>
    {
        public ClientValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.DocumentId).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        }
    }

    public class CreateLoanValidator : AbstractValidator<CreateLoanRequest>
    {
        public CreateLoanValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.InterestRate).InclusiveBetween(0, 100);
            RuleFor(x => x.InstallmentsCount).GreaterThan(0);
            RuleFor(x => x.StartDate).NotEmpty();
        }
    }

    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
