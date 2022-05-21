using Xamarin.Forms;
using xTaxi.Client.ViewModels;

namespace xTaxi.Client.Pages
{
    public partial class StartupPage : ContentPage
    {
        StartupViewModel _viewModel;
        public StartupPage()
        {
            InitializeComponent();

            _viewModel = new StartupViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.CheckLogin();
        }
    }
}