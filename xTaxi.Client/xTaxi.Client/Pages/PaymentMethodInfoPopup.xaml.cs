using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using xTaxi.Client.Models;
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

        private void CashSelectedCheckedChanged(object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                var paymentMethod = new PaymentMethod() { PayMethod = EnumPaymentMethod.Cash };
                _viewModel.SetCheckedCardCardCommand.Execute(paymentMethod);
            }
            else
            {
                var paymentMethod = new PaymentMethod() { PayMethod = EnumPaymentMethod.Card };
                _viewModel.SetCheckedCardCardCommand.Execute(paymentMethod);
            }
        }
    }
}