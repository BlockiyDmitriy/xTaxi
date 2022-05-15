using Xamarin.Forms;
using xTaxi.Client.ViewModels;

namespace xTaxi.Client.Views
{
    public partial class MenuPage : ContentPage
    {
        private MenuPageViewModel _viewModel;

        public MenuPage()
        {
            InitializeComponent();

            _viewModel = new MenuPageViewModel();
            BindingContext = _viewModel;
        }
    }
}