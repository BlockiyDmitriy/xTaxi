using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xTaxi.Client.Views;

namespace xTaxi.Client.ViewModels
{
    [System.Obsolete]
    public class LoginViewModel
    {
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
        }

        private async Task Login()
        {
            await Shell.Current.GoToAsync($"//{nameof(MapPage)}");
        }
    }
}
