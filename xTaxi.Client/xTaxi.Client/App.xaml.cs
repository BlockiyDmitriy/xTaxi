using Xamarin.Forms;
using xTaxi.Client.Services;
using xTaxi.Client.Services.LoaclDB;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;
using xTaxi.Client.Views;

namespace xTaxi.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            GoogleMapsApiService.Initialize(Constants.Constants.GoogleMapsApiKey);
            RegisterDependencies();
            MainPage = new CustomMasterDetailPage();
        }

        private void RegisterDependencies()
        {
            DependencyResolver.Register<DBService>();
            DependencyResolver.Register<LogService>();
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
