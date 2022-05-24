using System;
using System.Reflection;
using System.Threading.Tasks;
using xTaxi.Client.Models;
using xTaxi.Client.Services.HttpApi.UoW;

namespace xTaxi.Client.Services.LoginServices
{
    public class LoginService : BaseService, ILoginService
    {
        public LoginService(IUnitOfWork uOW) : base(uOW)
        {
        }

        public LoginService()
        {

        }
        public async Task<bool> CreateNewAccount(RegisterModel registerModel)
        {
            try
            {
                var isCreatedAccount = await UOW?.LoginRestApiClient.Register(Constants.Constants.Register, registerModel);

                return isCreatedAccount;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return false;
            }
        }

        public async Task<bool> Login(LoginModel loginModel)
        {
            try
            {
                var isLoginAccount = await UOW?.LoginRestApiClient.Login(Constants.Constants.Login, loginModel);

                return isLoginAccount;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return false;
            }
        }

        public async Task<string> GetToken(LoginModel loginModel)
        {
            try
            {
                var token = await UOW?.LoginRestApiClient.GetToken(Constants.Constants.GetToken, loginModel);

                _logService.TrackEvent("Get token: " + token);

                return token;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return string.Empty;
            }
        }
    }
}
