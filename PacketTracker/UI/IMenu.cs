using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.UI
{
    public interface IMenu
    {
        string Title { get; }
        void Run();
    }
}
