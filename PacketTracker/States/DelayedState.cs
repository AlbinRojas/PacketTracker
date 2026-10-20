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
            //Gör så att paketet återgår till sitt tidigare state innan det blev försenat.
            IPackageState previusState = package.Status.GetPreviousState();
            package.SetState(previusState);

            message = $"Förseningen är löst. Paketet är åter i rörelse.";
            return true;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            // Paketet är redan i försenat state, så vi kan inte markera det som försenat igen.
            message = "Paketet är redan markerat som försenat.";
            return false;
        }
    }
}