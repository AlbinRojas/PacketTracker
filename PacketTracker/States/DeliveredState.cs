using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class DeliveredState : IPackageState
    {
        public string StatusName => "Levererat";

        public bool TryNextState(Package package, out string message)
        {
            message = "Paketet är redan levererat och kan inte gå till ett nytt tillstånd.";
            return false;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            message = "Ett levererat paket kan inte markeras som försenat.";
            return false;
        }
    }
}