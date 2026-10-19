using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.SpeedAlgorithm
{
    public class Express:IPackageType
    {
        public decimal CalculatePrice(Weight weight, Distance distance)
        {
            return 100m + (weight.WeightInKg * 10m) + (distance.DistanceInKm * 2m);
        }
        //fix1
        public DateTime CalculateEstimateDelivery(Distance distance)
        {
            int days = (int)Math.Ceiling(distance.DistanceInKm / 800) + 2;
            return DateTime.Now.AddDays(days);
        }

        public string GetTypeName()
        {
            return "Express";
        }
    }
}
