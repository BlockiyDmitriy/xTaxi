using System;
using System.Collections.Generic;
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

        public static readonly BindableProperty UpdateCommandProperty = BindableProperty.Create(
            nameof(UpdateCommand),
            typeof(ICommand),
            typeof(ExtendedMap),
            null,
            BindingMode.TwoWay);

        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
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
                UpdateCommand = new Command<Position>(Update);
                GetActualLocationCommand = new Command(async () => await GetActualLocation());
                DrawRouteCommand = new Command<List<Position>>(DrawRoute);
                UpdateCommand = new Command<Position>(Update);
                CenterMapCommand = new Command<Location>(OnCenterMap);
                CleanPolylineCommand = new Command(CleanPolyline);
            }
        }

        private void AddMapStyle()
        {
            MapType = MapType.Street;
        }


        private async void Update(Position position)
        {

        }

        private void Calculate(List<Position> list)
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


        private void OnCenterMap(Location location)
        {
            MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(location.Latitude, location.Longitude), Distance.FromMiles(2)));

            LoadNearCars(location);
        }

        private void LoadNearCars(Location location)
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

        public void Dispose()
        {
        }
    }
}
