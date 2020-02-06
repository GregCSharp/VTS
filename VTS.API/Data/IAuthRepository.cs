using System.Threading.Tasks;

namespace VTS.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user);
        Task<User> Login(string name);
        Task<bool> UserExists(string name);
    }
}