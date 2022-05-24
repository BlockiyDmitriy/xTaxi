using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xTaxi.Client.Models;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Services.LoginServices;
using xTaxi.Client.Utils;
using xTaxi.Client.Views;

namespace xTaxi.Client.ViewModels
{
    public class CreateNewAccountViewModel : INotifyPropertyChanged
    {
        private readonly ILogService _logService;
        private readonly ILoginService _loginService;

        public ICommand CreateAccountCommand { get; }
        public ICommand CancelCommand { get; }
        public RegisterModel RegisterModel { get; set; }

        public CreateNewAccountViewModel()
        {
            _logService = DependencyResolver.Get<ILogService>();
            _loginService = DependencyResolver.Get<ILoginService>();

            RegisterModel = new RegisterModel();

            CreateAccountCommand = new Command(async () => await CreateAccount());
            CancelCommand = new Command(async () => await Cancel());
        }

        private Task Cancel()
        {
            try
            {
                Shell.Current.SendBackButtonPressed();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return Task.CompletedTask;
            }
        }

        private async Task CreateAccount()
        {
            try
            {
                if (IsAnyPropEmpty())
                {
                    await PopupNavigation.Instance.PushAsync(new ErrorPopup("All fields must be filled"));
                    return;
                }

                if (!IsValidEmail(RegisterModel.Email) || !IsValidPassword(RegisterModel.Password))
                {
                    await PopupNavigation.Instance.PushAsync(new ErrorPopup("Invalid mail or password"));
                    return;
                }

                if (RegisterModel.Password != RegisterModel.ConfirmPassword)
                {
                    await PopupNavigation.Instance.PushAsync(new ErrorPopup("Error confirm password"));
                    return;
                }


                var isCreatedAccount = await _loginService.CreateNewAccount(RegisterModel);

                if (!isCreatedAccount)
                {
                    await PopupNavigation.Instance.PushAsync(new ErrorPopup("Error creating account. \n\r Perhaps an account has already been created"));
                    return;
                }

                await Shell.Current.GoToAsync($"//{nameof(MapPage)}");
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }
        private bool IsAnyPropEmpty()
        {
            bool isAnyPropEmpty = RegisterModel.GetType().GetProperties()
                     .Where(p => p.GetValue(RegisterModel) is string) // selecting only string props
                     .Any(p => string.IsNullOrWhiteSpace((p.GetValue(RegisterModel) as string)));
            return isAnyPropEmpty;
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 6)
                return false;
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
