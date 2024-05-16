using System.Threading.Tasks;
using TFGDesktopApp.Models;

namespace TFG.Services.AuthentificationServices {
    public interface IAuthenticationService {
        Task<AppUser> GetUserByDataInput(string input);
        Task<bool> AuthenticateUserAsync(string input, string password);
        string HashPassword(string password);
    }
}
