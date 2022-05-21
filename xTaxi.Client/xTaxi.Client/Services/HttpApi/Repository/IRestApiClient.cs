using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xTaxi.Client.Models;

namespace xTaxi.Client.Services.HttpApi.Repository
{
    public interface IRestApiClient<TData> where TData : class
    {
        #region Login
        Task<bool> Register(string controllerUrl, RegisterModel registerModel);
        Task<bool> Login(string controllerUrl, LoginModel registerModel);
        Task<string> GetToken(string controllerUrl, LoginModel registerModel);
        #endregion
    }
}
