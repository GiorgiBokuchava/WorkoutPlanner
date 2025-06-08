using FluentValidation;
using MediatR;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Users.Handlers;

public sealed class CreateUserHandler
	: IRequestHandler<CreateUserCommand, UserDto>
{
	private readonly IUserService _users;
	private readonly IValidator<CreateUserCommand> _validator;

	public CreateUserHandler(IUserService users, IValidator<CreateUserCommand> validator)
	{
		_users = users;
		_validator = validator;
	}

	public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
	{
		var validationResult = await _validator.ValidateAsync(command, cancellationToken);
		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}

		var request = new CreateUserRequest(command.Name, command.Email, command.Password);
		return await _users.CreateUserAsync(request);
	}
}
