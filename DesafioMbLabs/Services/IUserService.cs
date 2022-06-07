using DesafioMbLabs.Models;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public interface IUserService
    {
        public Task CreateUser(User user);

        public Task<User> GetUser(string email, string password);

        public Task<User> GetUser(string email);

        public Task UpdateUser(User user);

        public Task DeleteUser(User user);
    }
}
