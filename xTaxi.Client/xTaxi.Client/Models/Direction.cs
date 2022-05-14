using System;
using System.Collections.Generic;
using System.Text;

namespace xTaxi.Client.Models
{
    public class Direction
    {
        public IList<GeocodedWaypoint> GeocodedWaypoints { get; set; }

        public IList<Route> Routes { get; set; }

        public string Status { get; set; }
    }
}
