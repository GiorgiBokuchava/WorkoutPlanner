namespace WorkoutPlanner.Contracts;

public record CreateUserRequest(
	string Name,
	string Email,
	string Password
);

public record UpdateUserRequest(
	string Name,
	string Email,
	string Password
);

public record UserDto(
	int Id,
	string Name,
	string Email
);

public record LoginRequest(
	string Email,
	string Password
);