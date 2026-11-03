using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.ShippingValues
{
    public class Station
    {
        public string StationName { get; }

        public Station(string stationName)
        {
            // en station måste ha ett namn
            if (string.IsNullOrWhiteSpace(stationName))
            {
                throw new ArgumentException("Stationsnamnet får inte vara tomt.", nameof(stationName));
            }

            StationName = stationName.Trim();
        }

        // Snygg utskrift
        public override string ToString() => StationName;
    }
}