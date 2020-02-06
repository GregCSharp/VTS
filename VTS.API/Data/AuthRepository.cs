using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VTS.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _context { get; }
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<User> Login(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
                return null;

            return user;
        }

        public async Task<User> Register(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string name)
        {
            if(await _context.Users.AnyAsync(u => u.Name.ToLower() == name.ToLower()))
                return true;
            return false;
        }
    }
}