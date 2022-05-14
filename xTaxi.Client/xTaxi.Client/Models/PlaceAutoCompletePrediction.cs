using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace xTaxi.Client.Models
{
    public class PlaceAutoCompletePrediction
    {
        public Position Position { get; set; }
        public string Address { get; set; }
        public StructuredFormatting StructuredFormatting { get; set; }
    }

    public class Place
    {
        
    }

    public class PlaceAutoCompleteResult
    {

    }
    public class StructuredFormatting
    {
        public string MainText { get; set; }
        public string SecondaryText { get; set; }
    }
}
