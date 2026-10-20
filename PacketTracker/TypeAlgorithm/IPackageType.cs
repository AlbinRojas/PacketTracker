using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketTracker.ShippingValues;

namespace PacketTracker
{
    public interface IPackageType
    {
        decimal CalculatePrice(Weight weight, Distance distance);
        DateTime CalculateEstimateDelivery(Distance distance);
        string GetTypeName();
    }
}
