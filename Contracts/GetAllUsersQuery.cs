using MediatR;

namespace WorkoutPlanner.Contracts;

/// <summary>Read-model request: “return every user”.</summary>
public record GetAllUsersQuery() : IRequest<IEnumerable<UserDto>>;