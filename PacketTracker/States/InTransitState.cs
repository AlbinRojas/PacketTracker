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
        public string StatusName => "Under transport";

        public bool TryNextState(Package package, out string message)
        {
            if (package.MoveToNextStation())
            {
                
                message = $"Paketet har anlänt till station: {package.CurrentStation?.StationName}";
                return true;
            }

            package.SetState(new OutForDeliveryState());
            message = "Paketet är ute för leverans.";   
            return true;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            package.SetState(new DelayedState(reason));
            message = $"Försenat under transport: {reason}";
            return true;
        }
    }
}
