using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class Standard:IPackageType
    {
        public decimal CalculatePrice(Weight weight, Distance distance)
        {
            decimal basePrice = 50.0m;
            return basePrice + (decimal)(weight.WeightInKg * 5) + (decimal)(distance.DistanceInKm * 1);

        }
        //fix 4
        public DateTime CalculateEstimateDelivery(Distance distance)
        {
            int days = (int)Math.Ceiling(distance.DistanceInKm / 300) + 2;
            return DateTime.Now.AddDays(days);
        }

        public string GetTypeName()
        {
            return "Standard";
        }
    }
}
