using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class DelayedState : IPackageState
    {
        public string StatusName => "Försenat";
        private string _reason;

        public DelayedState(string reason = "Okänd anledning")
        {
            _reason = reason;
        }

        public bool TryNextState(Package package, out string message)
        {
            // Efter försening kan paketet gå tillbaka till transport eller ut för leverans
            package.SetState(new InTransitState());
            message = $"Förseningen är löst. Paketet är åter i rörelse.";
            return true;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            message = "Paketet är redan markerat som försenat.";
            return false;
        }
    }
}