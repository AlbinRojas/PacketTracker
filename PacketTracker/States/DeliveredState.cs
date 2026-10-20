using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class DeliveredState : IPackageState
    {
        public String StatusName => "Levererat";
        public bool TryNextState(Package package, out string message)
        {
            message = "Packetet har blivit levererat.";
            return false;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            message = "Packetet är levererat och kan inte bli försenat";
            return false;
        }
    }
}
