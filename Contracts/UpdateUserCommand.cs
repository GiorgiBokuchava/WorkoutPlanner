namespace WorkoutPlanner.Contracts;

using MediatR;

/// <summary>
/// Update an existing user command.
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Email"></param>
/// <param name="Password"></param>
public record UpdateUserCommand(int Id, string Name, string Email, string Password)
	: IRequest<Unit>;