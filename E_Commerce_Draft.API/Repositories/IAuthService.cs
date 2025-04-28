using E_Commerce_Draft.API.Models;

namespace E_Commerce_Draft.API.Repositories
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UserLoginRequest request);
    }
}
