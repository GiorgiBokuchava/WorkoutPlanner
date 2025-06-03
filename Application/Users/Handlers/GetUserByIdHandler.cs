using MediatR;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Users.Handlers;

public sealed class GetUserByIdHandler
	: IRequestHandler<GetUserByIdQuery, UserDto?>
{
	private readonly IUserService _users;

	public GetUserByIdHandler(IUserService users) => _users = users;

	public async Task<UserDto?> Handle(GetUserByIdQuery q, CancellationToken ct)
		=> await _users.GetUserByIdAsync(q.Id);
}
