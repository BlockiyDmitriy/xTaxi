using Xamarin.Forms;
using xTaxi.Client.ViewModels;

namespace xTaxi.Client.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();
            BindingContext = _viewModel;
        }
    }
}