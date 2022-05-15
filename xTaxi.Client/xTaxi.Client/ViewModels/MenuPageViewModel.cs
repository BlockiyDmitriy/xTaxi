using Rg.Plugins.Popup.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xTaxi.Client.Pages;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;

namespace xTaxi.Client.ViewModels
{
    public class MenuPageViewModel
    {
        private readonly ILogService _logService;

        public ICommand OpenPaymentInfoCommand { get; }

        public MenuPageViewModel()
        {
            _logService = DependencyResolver.Get<ILogService>();

            OpenPaymentInfoCommand = new Command(async () => await OpenPaymentInfo());
        }

        private async Task OpenPaymentInfo()
        {
            try
            {
                _logService.Log("Open payment info popup");

                await PopupNavigation.Instance.PushAsync(new PaymentMethodInfoPopup());
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }
    }
}
