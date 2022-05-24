using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;
using xTaxi.Client.Views;

namespace xTaxi.Client.ViewModels
{
    public class CreateNewAccountViewModel : INotifyPropertyChanged
    {
        private readonly ILogService _logService;

        public ICommand CreateAccountCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateNewAccountViewModel()
        {
            _logService = DependencyResolver.Get<ILogService>();

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

                await Shell.Current.GoToAsync($"//{nameof(MapPage)}");
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
