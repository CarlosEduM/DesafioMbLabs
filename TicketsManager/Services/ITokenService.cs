using TicketsManager.Models;

namespace TicketsManager.Services
{
    public interface ITokenService
    {
        public string GenerateTocken(User user);
    }
}
