using Microsoft.Data.SqlClient;
using System.Data;

namespace WorkoutPlanner.Infrastructure.Persistence;

public abstract class RepositoryBase
{
	private readonly IConfiguration _configuration;

	protected RepositoryBase(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	protected IDbConnection GetConnection()
	{
		return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))
			?? throw new InvalidOperationException("DefaultConnection is not set");
	}
}