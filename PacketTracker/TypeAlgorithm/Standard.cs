using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class Standard : IPackageType
    {
        // lägst prioritet
        public int Priority => 3;

        public decimal CalculatePrice(Weight weight, Distance distance)
        {
            decimal basePrice = 50.0m;

            return basePrice + (weight.WeightInKg * 5m) + (distance.DistanceInKm * 1m);
        }

        // tar emot sendDate i stället för DateTime.Now
        public DateTime CalculateEstimateDelivery(Distance distance, DateTime sendDate)
        {
            int days = (int)Math.Ceiling(distance.DistanceInKm / 300m) + 2;
            return sendDate.AddDays(days);
        }

        public string GetTypeName()
        {
            return "Standard";
        }
    }
}