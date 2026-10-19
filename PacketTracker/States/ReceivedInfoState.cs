using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class ReceivedInfoState : IPackageState
    {
        public string StatusName => "Received Info";
        
        public bool TryNextState(StateController controller, out string message) 
        {
            controller.SetState(new InTransitState());

            message = "Package is now in the 'In Transit' state.";
            return true;
        }

        public bool TryMarkedDelayed(Package package, out string message) 
        {
            message = "Package cannot be marked as delayed in the 'Received Info' state.";
            return false;
        }
    }
}
