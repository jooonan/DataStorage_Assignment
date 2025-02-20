using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Data.Repositories;
namespace Business.Services;

public class UserService(UserRepository userRepository) : IUserService
{
    private readonly UserRepository _userRepository = userRepository;

    public async Task<UserDto> AddUserAsync(string firstName, string lastName, string email)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByNameAsync(firstName, lastName);
            if (existingUser != null)
            {
                return new UserDto
                {
                    Id = existingUser.Id,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.Email
                };
            }

            var newUser = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            await _userRepository.AddUserAsync(newUser);

            return new UserDto
            {
                Id = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding user: {ex.Message}");
            throw;
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? null : new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving user by ID ({id}): {ex.Message}");
            return null;
        }
    }

    public async Task UpdateUserAsync(UserDto userDto)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userDto.Id);
            if (user == null) return;

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;

            await _userRepository.UpdateUserAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while updating user (ID: {userDto.Id}): {ex.Message}");
            throw;
        }
    }
}
