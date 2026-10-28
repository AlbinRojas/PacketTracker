using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.TypeAlgorithm
{
    public class Express : IPackageType
    {
        // högst prioritet
        public int Priority => 1;

        public decimal CalculatePrice(Weight weight, Distance distance)
        {
            return 100m + (weight.WeightInKg * 10m) + (distance.DistanceInKm * 2m);
        }

        //  tar emot sendDate i stället för DateTime.Now
        public DateTime CalculateEstimateDelivery(Distance distance, DateTime sendDate)
        {
            int days = (int)Math.Ceiling(distance.DistanceInKm / 800m) + 2;
            return sendDate.AddDays(days);
        }

        public string GetTypeName()
        {
            return "Express";
        }
    }
}
