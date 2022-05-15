using Rg.Plugins.Popup.Pages;
using xTaxi.Client.ViewModels;

namespace xTaxi.Client.Pages
{
    public partial class AddNewCreditCardPopup : PopupPage
    {
        private AddNewCreditCardViewModel _viewModel;
        public AddNewCreditCardPopup()
        {
            InitializeComponent();

            _viewModel = new AddNewCreditCardViewModel();
            BindingContext = _viewModel;
        }
    }
}