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
            return 100m + (weight.Value * 10m) + (distance.Value * 2m);
        }

        public DateTime CalculateEstimateDelivery(Distance distance)
        {
            // Express delivery is faster, so we assume it takes 1 day for every 500 km
            int days = distance.Value > 500 ? 2 : 1;
            return DateTime.Now.AddDays(days);
        }

        public string GetTypeName()
        {
            return "Express";
        }
    }
}
