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
        public string StatusName => "Information mottagen";
        
        public bool TryNextState(Package package, out string message) 
        {
            package.SetState(new InTransitState());

            message = "Paketet har lämnat terminalen.";
            return true;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message) 
        {
            message = "Paketet kan inte bli försenat i detta stadium";
            return false;
        }
    }
}
