using Microsoft.EntityFrameworkCore;
using StockSense.Data;
using StockSense.Interface;
using StockSense.Models;
using StockSense.Models.ViewModels.Auth;

namespace StockSense.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if(await _context.Users.AnyAsync(u=> u.Email == registerViewModel.Email))
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                FullName = registerViewModel.FullName,
                Email = registerViewModel.Email,
                Password = registerViewModel.Password,
                Mobile = registerViewModel.ContactNumber,
                RoleId = registerViewModel.RoleId ?? 1,
                Status = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users
               .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }
            
            if (password != user.Password)
            {
                throw new Exception("Invalid email or password");
            }
            return user;
        }
    }
}
