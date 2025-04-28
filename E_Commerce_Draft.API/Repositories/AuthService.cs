using E_Commerce_Draft.API.Models;

namespace E_Commerce_Draft.API.Repositories
{
    public class AuthService: IAuthService
    {
        private readonly IAuthRepository authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        public async Task<string> LoginAsync(UserLoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Invalid login request");
            }

            var isValidUser = await authRepository.ValidateUserAsync(request);
            if (!isValidUser)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }
            var role = request.Username == "admin" ? "Admin" : "User";
            return await authRepository.GenerateJwtTokenAsync(request.Username, role);
        }
    }
}
