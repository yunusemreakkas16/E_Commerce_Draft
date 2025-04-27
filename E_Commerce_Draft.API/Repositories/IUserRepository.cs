using E_Commerce_Draft.API.Models.Domain;
using static E_Commerce_Draft.API.Models.Domain.User;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IUserRepository
    {
        Task<UserListResponseModel> GetAllUsersAsync();
        Task<UserResponseModel> GetUserByIdAsync(int id);
        Task<UserResponseModel> CreateUserAsync(User user);
        Task<UserResponseModel> UpdateUserAsync(User user);
    }
}