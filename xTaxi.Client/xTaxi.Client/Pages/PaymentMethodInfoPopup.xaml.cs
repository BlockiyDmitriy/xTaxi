using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using xTaxi.Client.ViewModels;

namespace xTaxi.Client.Pages
{
    public partial class PaymentMethodInfoPopup : PopupPage
    {
        private PaymentMethodInfoViewModel _viewModel;
        public PaymentMethodInfoPopup()
        {
            InitializeComponent();

            _viewModel = new PaymentMethodInfoViewModel();
            BindingContext = _viewModel;

            Task.Run(async ()=> await _viewModel.Init());
        }
    }
}