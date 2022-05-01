using Xamarin.Forms;
using xTaxi.Client.Services;
using xTaxi.Client.Views;

namespace xTaxi.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            GoogleMapsApiService.Initialize(Constants.Constants.GoogleMapsApiKey);
            MainPage = new CustomMasterDetailPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
