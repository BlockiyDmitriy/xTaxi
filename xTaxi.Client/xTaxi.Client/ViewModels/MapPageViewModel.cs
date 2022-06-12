using Acr.UserDialogs;
using Stateless;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using xTaxi.Client.Helpers;
using xTaxi.Client.Models;
using xTaxi.Client.Services;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;
using static Stateless.StateMachine<xTaxi.Client.Helpers.XUberState, xTaxi.Client.Helpers.XUberTrigger>;

namespace xTaxi.Client.ViewModels
{
    public class MapPageViewModel : INotifyPropertyChanged
    {
        private readonly ILogService _logService;
        private readonly IPaymentService _paymentService;

        public ObservableCollection<PlaceAutoCompletePrediction> Places { get; private set; }
        public ObservableCollection<PlaceAutoCompletePrediction> RecentPlaces { get; private set; }
        public PlaceAutoCompletePrediction RecentPlace1 { get; private set; }
        public PlaceAutoCompletePrediction RecentPlace2 { get; private set; }
        public ObservableCollection<PriceOption> PriceOptions { get; private set; }
        public PriceOption PriceOptionSelected { get; set; }

        public string PickupLocation { get; set; }

        Position OriginCoordinates { get; set; }
        Position DestinationCoordinates { get; set; }

        string _destinationLocation;
        public string DestinationLocation
        {
            get => _destinationLocation;
            set
            {
                _destinationLocation = value;
                if (!string.IsNullOrEmpty(_destinationLocation))
                {
                    GetPlacesCommand.Execute(_destinationLocation);
                }
            }
        }

        PlaceAutoCompletePrediction _placeSelected;
        public PlaceAutoCompletePrediction PlaceSelected
        {
            get => _placeSelected;
            set
            {
                _placeSelected = value;
                if (_placeSelected != null)
                {
                    GetPlaceDetailCommand.Execute(_placeSelected);
                }

            }
        }

        bool _isOriginFocused;
        public bool IsOriginFocused
        {
            get => _isOriginFocused;
            set
            {
                _isOriginFocused = value;
                if (_isOriginFocused)
                {
                    FireTriggerCommand.Execute(XUberTrigger.ChooseOrigin);
                }
            }
        }

        bool _isDestinationFocused;
        public bool IsDestinationFocused
        {
            get => _isDestinationFocused;
            set
            {
                _isDestinationFocused = value;
                if (_isDestinationFocused)
                {
                    FireTriggerCommand.Execute(XUberTrigger.ChooseDestination);
                }
            }
        }

        public XUberState State { get; private set; }

        public bool IsSearching { get; private set; }
        public string CardNumber { get; set; }

        public ICommand DrawRouteCommand { get; set; }
        public ICommand CenterMapCommand { get; set; }
        public ICommand CenterMapByNameCommand { get; set; }
        public ICommand CleanPolylineCommand { get; set; }
        public ICommand GetPlaceDetailCommand { get; }
        public ICommand FireTriggerCommand { get; }
        public ICommand CameraIdledCommand { get; private set; }

        private ICommand GetPlacesCommand { get; }
        public ICommand GetLocationNameCommand { get; }

        private TriggerWithParameters<PlaceAutoCompletePrediction> CalculateRouteTrigger { get; }

        private readonly IGoogleMapsApiService _googleMapsApi = new GoogleMapsApiService();
        private readonly IMapsApiService _mapsApi = new MapsApiService();
        private readonly StateMachine<XUberState, XUberTrigger> _stateMachine;

        public MapPageViewModel()
        {
            _logService = DependencyResolver.Get<ILogService>();
            _paymentService = DependencyResolver.Get<IPaymentService>();

            RecentPlaces = new ObservableCollection<PlaceAutoCompletePrediction>()
            {
                {
                    new PlaceAutoCompletePrediction()
                    { 
                        Address = "177 Ortega Ave, Mountain View, CA 94040, USA",
                        StructuredFormatting = new StructuredFormatting()
                        {
                            MainText = "Ortega Ave, Mountain",
                            SecondaryText = "Ortega Ave, Mountain View, USA"
                        }
                    }
                },
                {
                    new PlaceAutoCompletePrediction()
                    { 
                        Address = "Bill Graham Pkwy, Mountain View, CA 94043, USA",
                        StructuredFormatting = new StructuredFormatting()
                        {
                            MainText = "Bill Graham Pkwy",
                            SecondaryText = "Bill Graham Pkwy, Mountain View, USA"
                        }
                    }
                },
                {
                    new PlaceAutoCompletePrediction()
                    { 
                        Address = "1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA",
                        StructuredFormatting = new StructuredFormatting()
                        {
                            MainText = "Amphitheatre Pkwy",
                            SecondaryText = "Amphitheatre Pkwy, Mountain View, USA"
                        }
                    }
                }
            };

            RecentPlace1 = RecentPlaces[0];
            RecentPlace2 = RecentPlaces[1];

            PriceOptions = new ObservableCollection<PriceOption>()
            {
                {
                    new PriceOption()
                    { 
                        Tag="xUBERX", 
                        Category="Economy", 
                        CategoryDescription="Affortable, everyday rides", 
                        PriceDetails=new List<PriceDetail>()
                        {
                            {
                                new PriceDetail()
                                {
                                    Type="xUber X", 
                                    Price=332, 
                                    ArrivalETA="12:00pm", 
                                    Icon="https://carcody.com/wp-content/uploads/2019/11/Webp.net-resizeimage.jpg" 
                                }
                            },
                            {
                                new PriceDetail()
                                { 
                                    Type="xUber Black",
                                    Price=150, 
                                    ArrivalETA="12:40pm", 
                                    Icon="https://i0.wp.com/uponarriving.com/wp-content/uploads/2019/08/uberxl.jpg" 
                                }
                            }
                        }
                    } 
                },
                {
                    new PriceOption()
                    {
                        Tag="xUBERXL",
                        Category="Extra Seats",
                        CategoryDescription="Affortable rides for group up to 6",
                        PriceDetails=new List<PriceDetail>()
                        {
                            {
                                new PriceDetail()
                                { 
                                    Type="xUber XL", 
                                    Price=332, 
                                    ArrivalETA="12:00pm", 
                                    Icon="https://i0.wp.com/uponarriving.com/wp-content/uploads/2019/08/uberxl.jpg" 
                                }
                            }
                        }
                    }
                }
            };


            var _stateMachine = new StateMachine<XUberState, XUberTrigger>(XUberState.Initial);

            CalculateRouteTrigger = _stateMachine.SetTriggerParameters<PlaceAutoCompletePrediction>(XUberTrigger.CalculateRoute);

            _stateMachine.Configure(XUberState.Initial)
                .OnEntry(Initialize)
                .OnExit(() => { Places = new ObservableCollection<PlaceAutoCompletePrediction>(RecentPlaces); })
                .OnActivateAsync(GetActualUserLocation)
                .Permit(XUberTrigger.ChooseDestination, XUberState.SearchingDestination)
                .Permit(XUberTrigger.CalculateRoute, XUberState.CalculatingRoute);

            _stateMachine
                .Configure(XUberState.SearchingOrigin)
                .Permit(XUberTrigger.Cancel, XUberState.Initial)
                .Permit(XUberTrigger.ChooseDestination, XUberState.SearchingDestination);

            _stateMachine
                .Configure(XUberState.SearchingDestination)
                .Permit(XUberTrigger.Cancel, XUberState.Initial)
                .Permit(XUberTrigger.ChooseOrigin, XUberState.SearchingOrigin)
                .PermitIf(XUberTrigger.CalculateRoute, XUberState.CalculatingRoute, () => OriginCoordinates != null);

            _stateMachine
              .Configure(XUberState.CalculatingRoute)
              .OnEntryFromAsync(CalculateRouteTrigger, GetPlacesDetail)
              .Permit(XUberTrigger.ChooseRide, XUberState.ChoosingRide)
              .Permit(XUberTrigger.Cancel, XUberState.Initial);

            _stateMachine
               .Configure(XUberState.ChoosingRide)
               .OnEntryAsync(GetCardNumber)
               .Permit(XUberTrigger.Cancel, XUberState.Initial)
               .Permit(XUberTrigger.ChooseDestination, XUberState.SearchingDestination)
               .Permit(XUberTrigger.ConfirmPickUp, XUberState.ConfirmingPickUp);

            _stateMachine
              .Configure(XUberState.ConfirmingPickUp)
              .Permit(XUberTrigger.ChooseRide, XUberState.ChoosingRide)
              .Permit(XUberTrigger.ShowXUberPass, XUberState.ShowingXUberPass);

            _stateMachine
              .Configure(XUberState.ShowingXUberPass)
              .Permit(XUberTrigger.ConfirmPickUp, XUberState.ConfirmingPickUp)
              .Permit(XUberTrigger.WaitForDriver, XUberState.WaitingForDriver);

            _stateMachine
             .Configure(XUberState.WaitingForDriver)
             .Permit(XUberTrigger.CancelTrip, XUberState.Initial)
             .Permit(XUberTrigger.StartTrip, XUberState.TripInProgress);

            GetPlaceDetailCommand = new Command<PlaceAutoCompletePrediction>(async (param) =>
            {
                if (_stateMachine.CanFire(XUberTrigger.CalculateRoute))
                {
                    await _stateMachine.FireAsync(CalculateRouteTrigger, param);

                    State = _stateMachine.State;
                }
            });
            GetPlacesCommand = new Command<string>(async (param) => await GetPlacesByName(param));
            GetLocationNameCommand = new Command<Position>(async (param) => await GetLocationName(param));

            FireTriggerCommand = new Command<XUberTrigger>((trigger) =>
            {
                if (_stateMachine.CanFire(trigger))
                {
                    _stateMachine.Fire(trigger);
                    State = _stateMachine.State;
                }

                IsSearching = (State == XUberState.SearchingOrigin || State == XUberState.SearchingDestination);
            });

            PriceOptionSelected = PriceOptions.First();

            State = _stateMachine.State;

            _stateMachine.ActivateAsync();
        }

        private void Initialize()
        {
            CleanPolylineCommand.Execute(null);
            DestinationLocation = string.Empty;
        }

        private async Task GetActualUserLocation()
        {
            try
            {
                await Task.Yield();
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    OriginCoordinates = new Position(location.Latitude, location.Longitude);
                    CenterMapCommand.Execute(location);
                    GetLocationNameCommand.Execute(new Position(location.Latitude, location.Longitude));
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync("Error", "Unable to get actual location", "Ok");
            }
        }

        private async Task GetLocationName(Position position)
        {
            try
            {
                Geocoder geoCoder = new Geocoder();
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                PickupLocation = possibleAddresses.FirstOrDefault();
                CenterMapCommand.Execute(position);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async Task GetPlacesByName(string placeText)
        {
            CenterMapByNameCommand.Execute(placeText);
        }

        private async Task GetPlacesDetail(PlaceAutoCompletePrediction placeA)
        {
            try
            {
                DestinationCoordinates = new Position(placeA.Position.Latitude, placeA.Position.Longitude);
                if (await LoadRoute())
                {
                    RecentPlaces.Add(placeA);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Erorr get place details");
            }
            
        }

        private async Task<bool> LoadRoute()
        {
            var retVal = false;

            var googleDirection = await _mapsApi.GetDirections($"{OriginCoordinates.Latitude}", $"{OriginCoordinates.Longitude}", $"{DestinationCoordinates.Latitude}", $"{DestinationCoordinates.Longitude}");
            if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
            {
                var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                DrawRouteCommand.Execute(positions);
                FireTriggerCommand.Execute(XUberTrigger.ChooseRide);
                retVal = true;
            }
            else
            {
                FireTriggerCommand.Execute(XUberTrigger.ChooseRide);
                await UserDialogs.Instance.AlertAsync(":(", "No route found", "Ok");
            }

            return retVal;
        }

        public async Task GetCardNumber()
        {
            try
            {
                var paymentMethod = await _paymentService.GetPaymentMethod();

                string displayedCardNumber = string.Empty;

                if (paymentMethod.PayMethod == Models.EnumPaymentMethod.Cash)
                {
                    displayedCardNumber = string.Format("Cash");
                }
                else
                {
                    var creditCardData = await _paymentService.GetPaymentCard();

                    if (string.IsNullOrEmpty(creditCardData.CardNumber))
                    {
                        return;
                    }
                    var lastFourCardNumber = creditCardData.CardNumber.Substring(creditCardData.CardNumber.Length - 4);
                    displayedCardNumber = string.Format("***" + lastFourCardNumber);

                    _logService.Log(displayedCardNumber);
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CardNumber = displayedCardNumber;
                });
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
