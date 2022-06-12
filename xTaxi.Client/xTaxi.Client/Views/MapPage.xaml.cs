using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using xTaxi.Client.ViewModels;

namespace xTaxi.Client.Views
{
    public partial class MapPage
    {
        private MapPageViewModel _viewModel;
        public MapPage()
        {
            InitializeComponent();

            _viewModel = new MapPageViewModel();
            BindingContext = _viewModel;

            Task.Run(async () => await _viewModel.GetCardNumber());
        }

        public void OnMenuTapped(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        public void OnDoneClicked(object sender, EventArgs e)
        {
            headerSearch.FocusDestination();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var safeInsets = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();

            if (safeInsets.Top > 0)
            {
                headerSearch.BackButtonPadding = new Thickness(0, 20, 0, 0);
            }
        }

        private async void OnMapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
            Geocoder geoCoder = new Geocoder();

            Position position = new Position(e.Position.Latitude, e.Position.Longitude);
            IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
            string address = possibleAddresses.FirstOrDefault();
            Debug.WriteLine($"Address: {address}");

            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
            Position location = approximateLocations.FirstOrDefault();

            Debug.WriteLine(location.Latitude + ", " + location.Longitude);


            _viewModel.CenterMapCommand?.Execute(e.Position);

            _viewModel.PickupLocation = possibleAddresses.FirstOrDefault();
        }
    }
}