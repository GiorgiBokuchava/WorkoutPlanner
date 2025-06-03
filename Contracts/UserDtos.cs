namespace WorkoutPlanner.Contracts;

public record CreateUserRequest(
	string Name,
	string Email,
	string PasswordHash
);

public record UpdateUserRequest(
	string Name,
	string Email,
	string PasswordHash
);

public record UserDto(
	int Id,
	string Name,
	string Email
);