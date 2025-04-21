using E_Commerce_Draft.API.Models.Domain;

namespace E_Commerce_Draft.API.Repositories
{
    public class UserRepository : IUserRepository

    {
        public Task<User> AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User?> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
