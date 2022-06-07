using DesafioMbLabs.Models;

namespace DesafioMbLabs.Services
{
    public interface ITokenService
    {
        public string GenerateTocken(User user);
    }
}
