using CgvMate.Api.Data.Interfaces;
using CgvMate.Api.Entities;

namespace CgvMate.Api.Services;

public class UserService
{
    private readonly IUserRepo _userRepo;

    public UserService(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return _userRepo.GetAllUsersAsync();
    }

    public Task<User> GetUserByIdAsync(int id)
    {
        return _userRepo.GetUserByIdAsync(id);
    }

    public Task<User> GetUserByEmailAsync(string email)
    {
        return _userRepo.GetUserByEmailAsync(email);
    }

    public Task AddUserAsync(User user)
    {
        return _userRepo.AddUserAsync(user);
    }

    public Task UpdateUserAsync(User user)
    {
        return _userRepo.UpdateUserAsync(user);
    }

    public Task DeleteUserAsync(int id)
    {
        return _userRepo.DeleteUserAsync(id);
    }
}
