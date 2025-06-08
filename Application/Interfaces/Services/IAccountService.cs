using System.Security.Claims;
using WorkoutPlanner.Contracts;

namespace WorkoutPlanner.Application.Interfaces.Services;

public interface IAccountService
{
	Task<UserDto?> GetCurrentUserAsync(ClaimsPrincipal user);
	Task<bool> UpdateCurrentUserAsync(ClaimsPrincipal user, UpdateUserRequest request);
	Task<bool> DeleteCurrentUserAsync(ClaimsPrincipal user);
}