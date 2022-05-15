using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using xTaxi.Client.Models;
using xTaxi.Client.Services;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;

namespace xTaxi.Client.ViewModels
{
    public class AddNewCreditCardViewModel
    {
        private ILogService _logService;
        private IPaymentService _paymentService;

        public CardDataModel СardDataModel { get; set; }
        public ICommand AddCardCommand { get; }
        public ICommand CancelCommand { get; }
        public DateTime ExpiredCardTime { get; set; } = new DateTime();
        public string DisplayedCardNumber { get; set; }

        public AddNewCreditCardViewModel()
        {
            _logService = DependencyResolver.Get<ILogService>();
            _paymentService = DependencyResolver.Get<IPaymentService>();

            СardDataModel = new CardDataModel();

            AddCardCommand = new Command(async () => await AddNewCreditCard());
            CancelCommand = new Command(async () => await Cancel());
        }

        

        public async Task Init()
        {
            try
            {
                
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        private async Task AddNewCreditCard()
        {
            try
            {
                СardDataModel.ExpireCardDate = ExpiredCardTime.ToString();
                var res = await _paymentService.SetNewPaymentCard(СardDataModel);

                if (res == null)
                {
                    await UserDialogs.Instance.AlertAsync("Check the card details", "Incorrect data",  "Ok");
                }
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }
        private async Task Cancel()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }
    }
}
