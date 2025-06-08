using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WorkoutPlanner.Infrastructure.Repositories;

namespace WorkoutPlanner.Infrastructure.Security;

public class JwtService : IJwtService
{
	private readonly JwtSettings _jwtSettings;
	private readonly IUserRepository _userRepository;

	public JwtService(JwtSettings jwtSettings, IUserRepository userRepository)
	{
		_jwtSettings = jwtSettings;
		_userRepository = userRepository;
	}

	public async Task<(string AccessToken, string? RefreshToken)> GenerateTokensAsync(
		int userId, IEnumerable<string> roles)
	{

		var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);
		var signingKey = new SymmetricSecurityKey(keyBytes);
		var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

		var accessTokenExpires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = accessTokenExpires,
			Issuer = _jwtSettings.Issuer,
			Audience = _jwtSettings.Audience,
			SigningCredentials = creds
		};

		var handler = new JwtSecurityTokenHandler();
		var securityToken = handler.CreateToken(tokenDescriptor);
		var jwt = handler.WriteToken(securityToken);

		// Generate random refresh token
		// and hash it before storing in the database

		byte[] randomBytes = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomBytes);
		string rawRefreshToken = Convert.ToBase64String(randomBytes);

		byte[] rawBytes = Encoding.UTF8.GetBytes(rawRefreshToken);
		byte[] hashedBytes;
		using (var sha256 = SHA256.Create())
		{
			hashedBytes = sha256.ComputeHash(rawBytes);
		}

		string tokenHash = BitConverter.ToString(hashedBytes)
			.Replace("-", "")
			.ToLowerInvariant();

		var refreshExpires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

		int refreshTokenId = await _userRepository.AddRefreshTokenAsync(userId, tokenHash, refreshExpires);

		return (AccessToken: jwt, RefreshToken: rawRefreshToken);
	}

}