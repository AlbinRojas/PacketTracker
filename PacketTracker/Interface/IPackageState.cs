using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public interface IPackageState
    {
        public string StatusName { get; }
        bool TryNextState(StateController controller, out string message);
        bool TryMarkedDelayed(Package package, out string message);
    }
}
