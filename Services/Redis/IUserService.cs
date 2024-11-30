using RASP_Redis.Models.Auth;

namespace RASP_Redis.Services.Redis
{
    public interface IUserService
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(string username, string password);
        string GenerateJwtToken(User user);
    }
}
