namespace WorkoutPlanner.Contracts;

using MediatR;

/// <summary>Read-model request: “return a user by ID”.</summary>
public record GetUserByIdQuery(int Id) : IRequest<UserDto>;