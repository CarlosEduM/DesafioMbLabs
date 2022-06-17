using DesafioMbLabs.Data;
using DesafioMbLabs.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public class UserService : IUserService
    {
        private readonly SqlServerContext _dbContext;

        public UserService(SqlServerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteUserAsync(User user)
        {
            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserAsync(string email, string password)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _dbContext.Users
                .Include(u => u.Tickets)
                .Include(u => u.Payments)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
