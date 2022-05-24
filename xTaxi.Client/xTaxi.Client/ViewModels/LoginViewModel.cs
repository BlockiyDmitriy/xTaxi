using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xTaxi.Client.Models;
using xTaxi.Client.Pages;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Services.LoginServices;
using xTaxi.Client.Utils;
using xTaxi.Client.Views;

namespace xTaxi.Client.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ILogService _logService;
        private readonly ILoginService _loginService;

        public ICommand LoginCommand { get; }
        public ICommand CreateNewAccountCommand { get; }
        public LoginModel LoginModel { get; set; }

        public LoginViewModel()
        {
            _logService = DependencyResolver.Get<ILogService>();
            _loginService = DependencyResolver.Get<ILoginService>();

            LoginModel = new LoginModel();

            LoginCommand = new Command(async () => await Login());
            CreateNewAccountCommand = new Command(async () => await CreateNewAccount());

        }

        private async Task CreateNewAccount()
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(CreateNewAccountPage)}");
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        private async Task Login()
        {
            try
            {
                var isLogin = await _loginService.Login(LoginModel);

                if (isLogin)
                {
                    await Shell.Current.GoToAsync($"{nameof(MapPage)}");
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new ErrorPopup("Wrong login or password"));
                }
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                await PopupNavigation.Instance.PushAsync(new ErrorPopup("Error authorization"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
