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
        public decimal CalculatePrice(Weight weight, Distance distance)
        {
            decimal basePrice = 150.0m;
            decimal handlingFee = 75.0m;

            return basePrice + handlingFee + (weight.WeightInKg * 12.0m) + (distance.DistanceInKm * 2.5m);
        }

        public DateTime CalculateEstimateDelivery(Distance distance)
        {
            int days = (int)Math.Ceiling(distance.DistanceInKm / 250) + 3;
            return DateTime.Now.AddDays(days);
        }

        public string GetTypeName()
        {
            return "Fragile";
        }
    }
}
