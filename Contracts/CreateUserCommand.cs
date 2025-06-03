namespace WorkoutPlanner.Contracts;

using MediatR;

/// <summary>
/// Create a new user command.
/// </summary>
/// <param name="Name"></param>
/// <param name="Email"></param>
/// <param name="PasswordHash"></param>
public record CreateUserCommand(string Name, string Email, string PasswordHash)
	: IRequest<UserDto>;