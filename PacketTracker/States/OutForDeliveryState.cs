using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.States
{
    public class OutForDeliveryState : IPackageState
    {
        public string StatusName => "Ute för leverans";

        public bool TryNextState(Package package, out string message)
        {
            package.SetState(new DeliveredState());
            message = "Paketet har blivit levererat.";
            return true;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            package.SetState(new DelayedState(reason));
            message = $"Försenat under leverans: {reason}";
            return true;
        }
    }
}
