using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.ShippingValues
{
    // Värdeobjekt. Egenskapen har bara "get" (oföränderlig) och värdet valideras.
    public class Distance
    {
        // Tillåtna gränser. Regeln ligger här, så den gäller i hela systemet,
        // och menyn läser samma konstanter i stället för att hårdkoda talen.
        public const decimal MinKm = 1m;
        public const decimal MaxKm = 2500m;

        public decimal DistanceInKm { get; }

        public Distance(decimal distanceInKm)
        {
            // kontrollerar både min- och maxgräns
            if (distanceInKm < MinKm || distanceInKm > MaxKm)
            {
                throw new ArgumentException($"Avståndet måste vara mellan {MinKm} och {MaxKm} km.", nameof(distanceInKm));
            }

            DistanceInKm = distanceInKm;
        }

        public override string ToString() => $"{DistanceInKm} km";
    }
}
