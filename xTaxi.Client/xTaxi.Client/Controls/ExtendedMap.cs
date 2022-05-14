using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;

namespace xTaxi.Client.Controls
{
    public class ExtendedMap : Map, IDisposable
    {
        public static readonly BindableProperty CalculateCommandProperty = BindableProperty.Create(
            nameof(CalculateCommand),
            typeof(ICommand),
            typeof(ExtendedMap),
            null,
            BindingMode.TwoWay);

        public ICommand CalculateCommand
        {
            get { return (ICommand)GetValue(CalculateCommandProperty); }
            set { SetValue(CalculateCommandProperty, value); }
        }

        public static readonly BindableProperty GetActualLocationCommandProperty = BindableProperty.Create(
            nameof(GetActualLocationCommand),
            typeof(ICommand),
            typeof(ExtendedMap),
            null,
            BindingMode.TwoWay);

        public ICommand GetActualLocationCommand
        {
            get { return (ICommand)GetValue(GetActualLocationCommandProperty); }
            set { SetValue(GetActualLocationCommandProperty, value); }
        }

        public static readonly BindableProperty ActivateMapClickedProperty = BindableProperty.Create(
            nameof(ActivateMapClicked),
            typeof(bool),
            typeof(ExtendedMap),
            false,
            BindingMode.TwoWay);

        public static readonly BindableProperty CenterMapCommandProperty = BindableProperty.Create(
            nameof(CenterMapCommand),
            typeof(ICommand),
            typeof(ExtendedMap),
            null,
            BindingMode.TwoWay);

        public ICommand CenterMapCommand
        {
            get { return (ICommand)GetValue(CenterMapCommandProperty); }
            set { SetValue(CenterMapCommandProperty, value); }
        }
        public static readonly BindableProperty CenterMapByNameCommandProperty = BindableProperty.Create(
            nameof(CenterMapByNameCommand),
            typeof(ICommand),
            typeof(ExtendedMap),
            null,
            BindingMode.TwoWay);

        public ICommand CenterMapByNameCommand
        {
            get { return (ICommand)GetValue(CenterMapByNameCommandProperty); }
            set { SetValue(CenterMapByNameCommandProperty, value); }
        }

        public static readonly BindableProperty DrawRouteCommandProperty = BindableProperty.Create(
            nameof(DrawRouteCommand),
            typeof(ICommand),
            typeof(ExtendedMap),
            null,
            BindingMode.TwoWay);

        public ICommand DrawRouteCommand
        {
            get { return (ICommand)GetValue(DrawRouteCommandProperty); }
            set { SetValue(DrawRouteCommandProperty, value); }
        }

        public static readonly BindableProperty CleanPolylineCommandProperty = BindableProperty.Create(
            nameof(CleanPolylineCommand),
            typeof(ICommand),
            typeof(ExtendedMap),
            null,
            BindingMode.TwoWay);

        public ICommand CleanPolylineCommand
        {
            get { return (ICommand)GetValue(CleanPolylineCommandProperty); }
            set { SetValue(CleanPolylineCommandProperty, value); }
        }

        public static readonly BindableProperty CameraIdledCommandProperty = BindableProperty.Create(
            nameof(CameraIdledCommand),
            typeof(ICommand),
            typeof(ExtendedMap),
            null,
            BindingMode.TwoWay);

        public ICommand CameraIdledCommand
        {
            get { return (ICommand)GetValue(CameraIdledCommandProperty); }
            set { SetValue(CameraIdledCommandProperty, value); }
        }

        public bool ActivateMapClicked
        {
            get { return (bool)GetValue(ActivateMapClickedProperty); }
            set { SetValue(ActivateMapClickedProperty, value); }
        }

        public event EventHandler OnCalculate = delegate { };
        public event EventHandler OnLocationError = delegate { };

        public ExtendedMap()
        {
            AddMapStyle();
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                CalculateCommand = new Command<List<Position>>(Calculate);
                GetActualLocationCommand = new Command(async () => await GetActualLocation());
                DrawRouteCommand = new Command<List<Position>>(DrawRoute);
                CenterMapCommand = new Command<Position>(OnCenterMap);
                CenterMapByNameCommand = new Command<string>(async (locationName)=> await OnCenterMapByName(locationName));
                CleanPolylineCommand = new Command(CleanPolyline);
            }
        }

        private void AddMapStyle()
        {
            MapType = MapType.Street;
        }

        private void Calculate(List<Position> list)
        {
            try
            {            
                OnCalculate?.Invoke(this, default(EventArgs));
                var polyline = new Polyline();
                foreach (var p in list)
                {
                    polyline.Geopath.Add(p);

                }
                CleanPolyline();
                MoveToRegion(MapSpan.FromCenterAndRadius(new Position(polyline.Geopath[0].Latitude, polyline.Geopath[0].Longitude), Distance.FromMiles(0.50f)));

                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(polyline.Geopath.First().Latitude, polyline.Geopath.First().Longitude),
                    Label = "First",
                    Address = "First",
                };
                Pins.Add(pin);
                var pin1 = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(polyline.Geopath.Last().Latitude, polyline.Geopath.Last().Longitude),
                    Label = "Last",
                    Address = "Last",
                };
                Pins.Add(pin1);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error calculate", ex.Message);
            }
        }

        private async Task GetActualLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Position(location.Latitude, location.Longitude), Distance.FromMiles(0.3)));

                }
            }
            catch (Exception ex)
            {
                OnLocationError?.Invoke(this, default(EventArgs));
            }
        }


        private void CleanPolyline() 
        {

        }

        private void DrawRoute(List<Position> list)
        {
            try
            {        
                CleanPolyline();
                var polyline = new Polyline();
                polyline.StrokeColor = Color.Black;
                polyline.StrokeWidth = 3;

                foreach (var p in list)
                {
                    polyline.Geopath.Add(p);

                }
                MoveToRegion(MapSpan.FromCenterAndRadius(new Position(polyline.Geopath[0].Latitude, polyline.Geopath[0].Longitude),  Distance.FromMiles(0.50f)));

                var pin = new Pin
                {
                    Type = PinType.SearchResult,
                    Position = new Position(polyline.Geopath.First().Latitude, polyline.Geopath.First().Longitude),
                    Label = "Pin",
                    Address = "Pin"
                };
                Pins.Add(pin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error draw rout", ex.Message);
            }
        }


        private void OnCenterMap(Position location)
        {
            try
            {
                MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(location.Latitude, location.Longitude), Distance.FromMiles(2)));

                LoadNearCars(location);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error center map by name", ex.Message);
            }
        }
        private async Task OnCenterMapByName(string locationName)
        {
            try
            {
                var geoCoder = new Geocoder();

                IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(locationName);
                Position location = approximateLocations.FirstOrDefault();

                Debug.WriteLine(location.Latitude + ", " + location.Longitude);

                MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(location.Latitude, location.Longitude), Distance.FromMiles(2)));

                LoadNearCars(location);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error center map by name", ex.Message);
            }
        }

        private void LoadNearCars(Position location)
        {
            try
            {
                CleanPolyline();
                Pins.Clear();
                for (int i = 0; i < 7; i++)
                {
                    var random = new Random();

                    Pins.Add(new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(location.Latitude + (random.NextDouble() * 0.008), location.Longitude + (random.NextDouble() * 0.008)),
                        Label = "Car",
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error load near cars", ex.Message);
            }
        }

        public void Dispose()
        {
        }
    }
}
