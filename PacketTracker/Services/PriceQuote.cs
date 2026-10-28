using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.Services
{
    public record PriceQuote(string TypeName, int Priority, decimal Price, DateTime EstimatedDelivery);
}
