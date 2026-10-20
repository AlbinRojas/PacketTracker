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
                message = $"Paketet har anlänt till station: {package.CurrentStation}";
                package.SetState(new OutForDeliveryState());
                return true;
            }
            message = "Kunde inte flytta paketet till nästa station.";
            return false;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            package.SetState(new DelayedState(reason));
            message = $"Försenad under transport: {reason}";
            return true;
        }
    }
}