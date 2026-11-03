using PacketTracker.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class InTransitState : IPackageState
    {
        public string StatusName => "Under Transport";
        public bool TryNextState(Package package, out string message)
        {
            if (package.MoveToNextStation())
            {
                message = $"Packetet har anlänt till station: {package.CurrentStation}";
                return true;
            }
            package.SetState(new OutForDeliveryState());
            message = "Packetet är ute för leverans.";
            return true;
        }
        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            package.SetState(new DelayedState(reason));
            message = $"Försenad under transport: {reason}";
            return true;
        }
    }
}
