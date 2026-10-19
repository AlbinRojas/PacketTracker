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
          decimal basePrice = 50.0m
          return basePrice + (decimal)(weight.Value * 5.0) + (decimal)(distance.Value * 1.0);

        }

        public DateTime CalculateEstimateDelivery(Distance distance)
        {
            // Standard delivery is slower, so we assume it takes 1 day for every 100 km
            int days = (int)Math.Ceiling(distance.Value / 300.0) + 2;
            return DateTime.Now.AddDays(days);
        }

        public string GetTypeName()
        {
            return "Standard";
        }
    }
}
