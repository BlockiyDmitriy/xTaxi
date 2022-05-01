using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace xTaxi.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage
    {
        public MapPage() => InitializeComponent();

        public void OnMenuTapped(object sender, EventArgs e) => CustomMasterDetailPage.Current.IsPresented = true;

        public void OnDoneClicked(object sender, EventArgs e) => headerSearch.FocusDestination();

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var safeInsets = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();

            if (safeInsets.Top > 0)
            {
                menuIcon.Margin = backButton.Margin = new Thickness(20, 40, 20, 0);
                headerSearch.BackButtonPadding = new Thickness(0, 20, 0, 0);
            }
        }
    }
}