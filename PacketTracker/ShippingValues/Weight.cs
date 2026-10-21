using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.ShippingValues
{
    public class Weight
    {
        public decimal WeightInKg { get; set; }

        public Weight(decimal weightInKg) 
        {
            WeightInKg = weightInKg;
        }
    }
}
