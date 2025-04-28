using E_Commerce_Draft.API.Models;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IAuthRepository
    {
        Task<string> GenerateJwtTokenAsync(string username, string role);
        Task<bool> ValidateUserAsync(UserLoginRequest request);

    }
}
