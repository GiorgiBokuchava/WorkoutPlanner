﻿namespace WorkoutPlanner.Domain.Entities;

public class RefreshToken
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public string TokenHash { get; set; } = string.Empty;
	public DateTime ExpiresAt { get; set; }
	public DateTime? RevokedAt { get; set; }
	public byte? RevocationReason { get; set; }
}