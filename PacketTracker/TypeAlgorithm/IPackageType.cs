using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public interface IPackageType
    {
        //fix 3
        decimal CalculatePrice(Weight weight, Distance distance);
        DateTime CalculateEstimateDelivery(Distance distance);
        string GetTypeName();

    }
}
