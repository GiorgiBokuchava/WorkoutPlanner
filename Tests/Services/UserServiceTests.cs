using Moq;
using WorkoutPlanner.Application.Services;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;
using WorkoutPlanner.Infrastructure.Repositories;
using WorkoutPlanner.Infrastructure.Security;
using Xunit;

namespace WorkoutPlanner.Tests.Services
{
	public class UserServiceTests
	{
		[Fact]
		public async Task GetAllUsersAsync_ReturnsMappedDtos()
		{
			var repoMock = new Mock<IUserRepository>();
			repoMock
			  .Setup(r => r.GetAllUsersAsync())
			  .ReturnsAsync(new[]
			  {
				  new User { Id = 1, Name = "Alice", Email = "a@x.com" },
				  new User { Id = 2, Name = "Bob",   Email = "b@x.com" }
			  });
			var pwdMock = new Mock<IPasswordService>();
			var svc = new UserService(repoMock.Object, pwdMock.Object);

			// Act
			var dtos = (await svc.GetAllUsersAsync()).ToList();

			// Assert
			Assert.Equal(2, dtos.Count);
			Assert.Equal("Alice", dtos[0].Name);
			Assert.Equal("b@x.com", dtos[1].Email);
		}

		[Fact]
		public async Task CreateUserAsync_HashesPasswordAndReturnsDto()
		{
			// Arrange
			var pwdMock = new Mock<IPasswordService>();
			pwdMock
			  .Setup(p => p.HashPassword("secret"))
			  .Returns("HASHED_SECRET");

			var repoMock = new Mock<IUserRepository>();
			// simulate DB returning id=42
			repoMock
			  .Setup(r => r.AddUserAsync(It.Is<User>(u => u.PasswordHash == "HASHED_SECRET")))
			  .ReturnsAsync(42);

			var svc = new UserService(repoMock.Object, pwdMock.Object);
			var request = new CreateUserRequest("Carol", "c@x.com", "secret");

			// Act
			var dto = await svc.CreateUserAsync(request);

			// Assert
			Assert.Equal(42, dto.Id);
			Assert.Equal("Carol", dto.Name);
			Assert.Equal("c@x.com", dto.Email);
			pwdMock.Verify(p => p.HashPassword("secret"), Times.Once);
		}
	}
}
