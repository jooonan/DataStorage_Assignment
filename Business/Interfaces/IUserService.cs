using Business.Dtos;

namespace Business.Interfaces;

public interface IUserService
{
    Task<UserDto> AddUserAsync(string firstName, string lastName, string email);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task UpdateUserAsync(UserDto userDto);
}