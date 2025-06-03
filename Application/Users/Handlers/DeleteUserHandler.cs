using MediatR;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Users.Handlers;

public sealed class DeleteUserHandler
	: IRequestHandler<DeleteUserCommand, Unit>
{
	private readonly IUserService _users;

	public DeleteUserHandler(IUserService users) => _users = users;

	public async Task<Unit> Handle(DeleteUserCommand c, CancellationToken ct)
	{
		await _users.DeleteUserAsync(c.Id);
		return Unit.Value;
	}
}
