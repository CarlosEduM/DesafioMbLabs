using DesafioMbLabs.Models;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public interface IUserService
    {
        public Task NewUser(User user);

        public Task<User> GetUser(int id);

        public Task UpdateUser(User user);

        public Task DeleteUser(int id);
    }
}
