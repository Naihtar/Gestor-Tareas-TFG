using System.Threading.Tasks;
using TFG.Models;

namespace TFG.Services.AuthentificationServices {
    public interface IAuthenticationService {
        Task<AppUser> GetUserByDataInput(string input);
        Task<bool> AuthenticateUserAsync(string input, string password);
        string HashPassword(string password);
    }
}
