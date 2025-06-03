using AutoMapper;
using WorkoutPlanner.Contracts;
using WorkoutPlanner.Domain.Entities;

namespace WorkoutPlanner.Application.Mappings;

public class WorkoutMappingProfile : Profile
{
	public WorkoutMappingProfile()
	{
		CreateMap<CreateUserRequest, User>();
		CreateMap<UpdateUserRequest, User>();

		CreateMap<User, UserDto>();
	}
}