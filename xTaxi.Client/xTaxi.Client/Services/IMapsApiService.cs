using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xTaxi.Client.Models;

namespace xTaxi.Client.Services
{
    public interface IMapsApiService
    {
        Task<Direction> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude);
        Task<PlaceAutoCompleteResult> GetPlaces(string text);
        Task<Place> GetPlaceDetails(PlaceAutoCompletePrediction place);
    }
}
