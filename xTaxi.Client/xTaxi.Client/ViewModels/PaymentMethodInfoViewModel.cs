using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using xTaxi.Client.Pages;
using xTaxi.Client.Services;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;

namespace xTaxi.Client.ViewModels
{
    public class PaymentMethodInfoViewModel : INotifyPropertyChanged
    {
        private ILogService _logService;
        private IPaymentService _paymentService;

        public ICommand AddNewCreditCardCommand { get; }
        public string DisplayedCardNumber { get; set; }
        public bool IsHasValidCard { get; set; }

        public PaymentMethodInfoViewModel()
        {
            _logService = DependencyResolver.Get<ILogService>();
            _paymentService = DependencyResolver.Get<IPaymentService>();


            AddNewCreditCardCommand = new Command(async () => await AddNewCreditCard());
        }

        public async Task Init()
        {
            try
            {
                var displayedCardNumber = await GetDisplaydCardNumber();
                if (!string.IsNullOrEmpty(displayedCardNumber))
                {
                    IsHasValidCard = false;
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        DisplayedCardNumber = displayedCardNumber;
                    });
                }
                else
                {
                    IsHasValidCard = true;
                }
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        private async Task<string> GetDisplaydCardNumber()
        {
            try
            {
                var cardData = await _paymentService.GetPaymentCard();
                if (string.IsNullOrEmpty(cardData.CardNumber))
                {
                    var res = "No card";
                    return res;
                }
                var lastFourCardNumber = cardData.CardNumber.Substring(cardData.CardNumber.Length - 4);
                var displayedCardNumber = string.Format("***" + lastFourCardNumber);

                _logService.Log(displayedCardNumber);
                return displayedCardNumber;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return string.Empty;
            }
        }

        private async Task AddNewCreditCard()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new AddNewCreditCardPopup());
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
