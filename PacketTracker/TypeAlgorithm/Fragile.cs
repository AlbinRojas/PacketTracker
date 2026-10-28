using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class Fragile : IPackageType
    {
        // mellanprioritet (hanteras före standard p.g.a. särskild hantering)
        public int Priority => 2;

        public decimal CalculatePrice(Weight weight, Distance distance)
        {
            decimal basePrice = 150.0m;
            decimal handlingFee = 75.0m;

            // tog bort onödiga (decimal)-casts
            return basePrice + handlingFee + (weight.WeightInKg * 12.0m) + (distance.DistanceInKm * 2.5m);
        }

        // tar emot sendDate i stället för DateTime.Now
        public DateTime CalculateEstimateDelivery(Distance distance, DateTime sendDate)
        {
            int days = (int)Math.Ceiling(distance.DistanceInKm / 250m) + 3;
            return sendDate.AddDays(days);
        }

        public string GetTypeName()
        {
            return "Fragile";
        }
    }
}