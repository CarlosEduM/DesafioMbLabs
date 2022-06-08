using DesafioMbLabs.Models;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Service to create a new user on database
        /// </summary>
        /// <param name="user">User to be created</param>
        public Task CreateUser(User user);

        /// <summary>
        /// Get a user data when he is making login
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>User data</returns>
        public Task<User> GetUser(string email, string password);

        /// <summary>
        /// Get a user data when he was logged
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User data</returns>
        public Task<User> GetUser(string email);

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="user"></param>
        public Task UpdateUser(User user);

        /// <summary>
        /// Delete user data
        /// </summary>
        /// <param name="user">User to be deleted</param>
        public Task DeleteUser(User user);
    }
}
