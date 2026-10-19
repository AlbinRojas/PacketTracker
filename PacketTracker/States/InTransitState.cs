using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class InTransitState: IPackageState
    {
        bool TryNextState(StateController controller, out string message)
        {

            return true;
        }
        bool TryMarkedDelayed(Package package, out string message)
        {

            return true;
        }
    }
}
