using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.Interface
{
    public interface IPackageObserver
    {
        void Update(Package package, string message);
    }
}
    