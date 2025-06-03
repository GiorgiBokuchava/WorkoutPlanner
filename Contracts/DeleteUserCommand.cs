namespace WorkoutPlanner.Contracts;

using MediatR;

/// <summary>
/// Delete a user command.
/// </summary>
/// <param name="Id"></param>
public record DeleteUserCommand(int Id) : IRequest<Unit>;