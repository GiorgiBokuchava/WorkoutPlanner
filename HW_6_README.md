# HW-6 documentation

## MediatR
Refactored only the Users flow to prove the concept.
Created five record messages (2 queries + 3 commands) and matching handlers in `Application/Users/Handlers`.
`UsersController` now depends on `IMediator`, so HTTP code is free of business logic.

## AutoMapper
Added `WorkoutMappingProfile` with two maps:
`CreateUserRequest -> User` and `User -> UserDto`.
`UserService` now calls `_mapper.Map` instead of hand-copying properties.

## FluentValidation
Implemented `CreateUserCommandValidator`. Checks:
* Name is not empty
* Email is well-formed
* PasswordHash is at least 8 chars
* 
## Modern C#  
* Checked that `<Nullable>enable</Nullable>` was present in the project file.

## Async
Double-checked for `.Result`/`.Wait` (none found) and confirmed every controller/service/repository method is `async` and `await`.
