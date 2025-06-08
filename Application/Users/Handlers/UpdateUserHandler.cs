using MediatR;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Users.Handlers;

public sealed class UpdateUserHandler
	: IRequestHandler<UpdateUserCommand, Unit>
{
	private readonly IUserService _users;

	public UpdateUserHandler(IUserService users) => _users = users;

	public async Task<Unit> Handle(UpdateUserCommand c, CancellationToken ct)
	{
		var req = new UpdateUserRequest(c.Name, c.Email, c.Password);
		var ok = await _users.UpdateUserAsync(c.Id, req);
		return Unit.Value;
	}
}
