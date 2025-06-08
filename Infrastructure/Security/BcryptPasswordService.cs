﻿namespace WorkoutPlanner.Infrastructure.Security;

public class BcryptPasswordService : IPasswordService
{
	private const int WorkFactor = 10;

	public string HashPassword(string password)
	{
		return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
	}

	public bool VerifyPassword(string password, string hashedPassword)
	{
		return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
	}
}