using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class DelayedState : IPackageState
    {
        private readonly string _reason;   // readonly

        // anledningen visas nu i statusen (tidigare sparades den men användes aldrig)
        public string StatusName => $"Försenat ({_reason})";

        public DelayedState(string reason = "Okänd anledning")
        {
            _reason = string.IsNullOrWhiteSpace(reason) ? "Okänd anledning" : reason;
        }

        public bool TryNextState(Package package, out string message)
        {
            // Paketet återgår till det state det hade innan förseningen
            IPackageState previousState = package.Status.GetPreviousState();
            package.SetState(previousState);

            message = "Förseningen är löst. Paketet är åter i rörelse.";
            return true;
        }

        public bool TryMarkedDelayed(Package package, string reason, out string message)
        {
            // Paketet är redan försenat och kan inte markeras igen
            message = "Paketet är redan markerat som försenat.";
            return false;
        }
    }
}