using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IUserRepository
    {
        Task<(int MessageId, string MessageDescription, List<User>)> GetAllUsersAsync();
        Task<(int MessageId, string MessageDescription, User?)> GetUserByIdAsync(int id);
        Task<(int MessageId, string MessageDescription, User?)> CreateUserAsync(User user);
        Task<(int MessageId, string MessageDescription, User?)> UpdateUserAsync(User user);
    }
}