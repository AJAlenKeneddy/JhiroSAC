using JhiroServer.Models;
using System.Threading.Tasks;

namespace JhiroServer.Services
{
    public interface IJwtService
    {
        string GenerateToken(Usuario usuario);
        string GetUserIdFromToken(string token);

        Task<string> GetTokenAsync(); 
    }
}
