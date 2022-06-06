using DesafioMbLabs.Data;
using DesafioMbLabs.Models;
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

        public Task DeleteUser(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetUser(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task NewUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public Task UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
