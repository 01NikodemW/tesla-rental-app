using FluentValidation;
using Workshop.Domain.Repositories;

namespace Workshop.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUsersRepository usersRepository)
    {
        RuleFor(dto => dto.Email)
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(x => x.Email)
            .Custom((value, context) =>
            {
                var emailInUse = usersRepository.CheckIfUserWithProvidedEmailInDb(value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "Email is taken");
                }
            });


        RuleFor(dto => dto.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$")
            .WithMessage("Password must contain at least 1 capital letter, 1 digit and 1 special character");


        RuleFor(dto => dto.PasswordConfirmation)
            .Equal(dto => dto.Password)
            .WithMessage("Passwords do not match");


        RuleFor(dto => dto.FirstName)
            .Length(3, 100);

        RuleFor(dto => dto.LastName)
            .Length(3, 100);
    }
}