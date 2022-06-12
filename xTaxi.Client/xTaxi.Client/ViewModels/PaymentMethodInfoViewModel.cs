using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using xTaxi.Client.Models;
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
        public ICommand SetCheckedCardCardCommand { get; }
        public string DisplayedCardNumber { get; set; }
        public bool IsHasValidCard { get; set; }
        public bool IsCheckedCard { get; set; }
        public bool IsCheckedCash { get; set; }

        public PaymentMethodInfoViewModel()
        {
            _logService = DependencyResolver.Get<ILogService>();
            _paymentService = DependencyResolver.Get<IPaymentService>();


            AddNewCreditCardCommand = new Command(async () => await AddNewCreditCard());
            SetCheckedCardCardCommand = new Command<PaymentMethod>(async (paymentMethod) => await SetCheckedCardCard(paymentMethod));
        }


        public async Task Init()
        {
            try
            {
                var displayedCardNumber = await GetDisplaydCardNumber();
                if (!string.IsNullOrEmpty(displayedCardNumber))
                {
                    IsHasValidCard = true;
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        DisplayedCardNumber = displayedCardNumber;
                    });
                }
                else
                {
                    IsHasValidCard = false;
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        DisplayedCardNumber = "No card";
                    });
                }
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await CheckedCardCard();
                });
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        private async Task SetCheckedCardCard(PaymentMethod paymentMethod)
        {
            try
            {
                var method = await _paymentService.SetPaymentMethod(paymentMethod);
            }
            catch (Exception ex)
            {
                _logService.TrackException(ex, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        private async Task CheckedCardCard()
        {
            try
            {
                var paymentMethod = await _paymentService.GetPaymentMethod();

                IsCheckedCard = paymentMethod.PayMethod switch
                {
                    EnumPaymentMethod.Card => true,
                    EnumPaymentMethod.Cash => false,
                    _ => false,
                };

                IsCheckedCash = !IsCheckedCard;
            }
            catch (Exception ex)
            {
                _logService.TrackException(ex, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        private async Task<string> GetDisplaydCardNumber()
        {
            try
            {
                var cardData = await _paymentService.GetPaymentCard();
                if (string.IsNullOrEmpty(cardData.CardNumber))
                {
                    return string.Empty;
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
                var paymentMethod = await _paymentService.SetPaymentMethod(new PaymentMethod() { PayMethod = EnumPaymentMethod.Card });
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
