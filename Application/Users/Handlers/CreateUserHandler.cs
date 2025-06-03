using MediatR;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Users.Handlers;

public sealed class CreateUserHandler
	: IRequestHandler<CreateUserCommand, UserDto>
{
	private readonly IUserService _users;

	public CreateUserHandler(IUserService users) => _users = users;

	public async Task<UserDto> Handle(CreateUserCommand c, CancellationToken ct)
	{
		var req = new CreateUserRequest(c.Name, c.Email, c.PasswordHash);
		return await _users.CreateUserAsync(req);
	}
}
