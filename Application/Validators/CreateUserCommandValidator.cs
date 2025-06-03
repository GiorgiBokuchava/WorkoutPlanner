using FluentValidation;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required.");

		RuleFor(x => x.Email)
			.NotEmpty().EmailAddress().WithMessage("A valid email is required.");

		RuleFor(x => x.PasswordHash)
			.MinimumLength(8).WithMessage("Password must be at least 8 characters.");
	}
}
