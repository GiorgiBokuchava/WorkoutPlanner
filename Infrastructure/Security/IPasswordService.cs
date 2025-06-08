namespace WorkoutPlanner.Infrastructure.Security;

public interface IPasswordService
{
	/// <summary>
	/// Hashes a plaintext password (using bcrypt). The returned string
	/// contains salt and cost parameters.
	/// </summary>
	string HashPassword(string password);

	/// <summary>
	/// Verifies that the given plaintext matches the provided bcrypt hash.
	/// </summary>
	bool VerifyPassword(string password, string hashedPassword);
}