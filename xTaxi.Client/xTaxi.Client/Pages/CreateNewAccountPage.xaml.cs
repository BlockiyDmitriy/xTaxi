using Xamarin.Forms;
using xTaxi.Client.ViewModels;

namespace xTaxi.Client.Pages
{
    public partial class CreateNewAccountPage : ContentPage
    {
        private readonly CreateNewAccountViewModel _viewModel;

        public CreateNewAccountPage()
        {
            InitializeComponent();

            _viewModel = new CreateNewAccountViewModel();
            BindingContext = _viewModel;
        }
    }
}