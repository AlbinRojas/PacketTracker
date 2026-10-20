using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class ReceivedInfoState : IPackageState
    {
        public string StatusName => "Received Info";
        
        public bool TryNextState(Package package, out string message) 
        {
            package.SetState(new InTransitState());
            message = "Paketet har gått vidare till 'Under Transport'.";
            return true;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message) 
        {
            message = "Paketet kan inte markeras som försenat i läget 'Received Info'.";
            return false;
        }
    }
}