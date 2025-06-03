using MediatR;
using WorkoutPlanner.Application.Interfaces.Services;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Users.Handlers;

public sealed class GetAllUsersHandler
	: IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
	private readonly IUserService _users;

	public GetAllUsersHandler(IUserService users)
	{
		_users = users;
	}

	public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery _, CancellationToken ct)
	{
		return await _users.GetAllUsersAsync();
	}
}
