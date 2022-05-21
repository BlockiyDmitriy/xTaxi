using System.Threading.Tasks;
using xTaxi.Client.Models;

namespace xTaxi.Client.Services.LoginServices
{
    public interface ILoginService
    {
        public Task<bool> CreateNewAccount(RegisterModel registerModel);
        public Task<bool> Login(LoginModel registerModel);
        public Task<string> GetToken(LoginModel loginModel);
    }
}
