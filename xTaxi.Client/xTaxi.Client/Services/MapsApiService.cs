using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using xTaxi.Client.Models;

namespace xTaxi.Client.Services
{
    public class MapsApiService : IMapsApiService
    {
        public Task<Direction> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude)
        {
            try
            {
                return Task.FromResult(new Direction());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e, MethodBase.GetCurrentMethod()?.Name);
                return Task.FromResult(new Direction());
            }
        }

        public Task<Place> GetPlaceDetails(PlaceAutoCompletePrediction place)
        {
            try
            {
                return Task.FromResult(new Place());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e, MethodBase.GetCurrentMethod()?.Name);
                return Task.FromResult(new Place());
            }
        }

        public Task<PlaceAutoCompleteResult> GetPlaces(string text)
        {
            try
            {
                return Task.FromResult(new PlaceAutoCompleteResult());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e, MethodBase.GetCurrentMethod()?.Name);
                return Task.FromResult(new PlaceAutoCompleteResult());
            }
        }
    }
}
