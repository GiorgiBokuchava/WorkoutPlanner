﻿using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IUserService
{
	Task<IEnumerable<UserDto>> GetAllUsersAsync();
	Task<UserDto?> GetUserByIdAsync(int id);
	Task<UserDto> CreateUserAsync(CreateUserRequest request);
	Task<bool> UpdateUserAsync(int id, UpdateUserRequest request);
	Task<bool> DeleteUserAsync(int id);
}
