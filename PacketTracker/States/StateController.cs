using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PacketTracker
{
    public class StateController
    {
        public IPackageState CurrentState { get; private set; }

        //Används bara för att simulera att det tar någon tid
        private DateTime _simulatedTime;

        // Behåller textloggen för utskrifter
        private readonly List<string> _statusHistory = new();

        public IReadOnlyList<string> StatusHistory => _statusHistory;
        // Lista som sparar själva objekten
        private readonly List<IPackageState> _stateObjects = new();

        public bool IsDelivered => CurrentState is DeliveredState;

        public StateController(IPackageState initialState)
        {
            ArgumentNullException.ThrowIfNull(initialState);

            _simulatedTime = DateTime.Now;

            CurrentState = initialState;
            _statusHistory.Add($"{_simulatedTime:yyyy-MM-dd} {DateTime.Now:HH:mm:ss} - {initialState.StatusName}");
            _stateObjects.Add(initialState);
        }

        public void SetState(IPackageState newState)
        {
            ArgumentNullException.ThrowIfNull(newState);

            _simulatedTime = _simulatedTime.AddDays(1);

            CurrentState = newState;
            _statusHistory.Add($"{_simulatedTime:yyyy-MM-dd} {DateTime.Now:HH:mm:ss} - {newState.StatusName}");
            _stateObjects.Add(newState);
        }

        // flyttar paketet till nästa state i flödet
        public bool ProceedToNextState(Package package, out string message) 
        {
            ArgumentNullException.ThrowIfNull(package);

            // Låter nuvarande status försöka gå vidare, och sparar meddelandet
            bool success = CurrentState.TryNextState(package, out message);

            // Om det lyckas, notifiera observatörerna
            if (success)
            {
                package.NotifyObservers(message);
            }

            return success;
        }

        // Markera paketet som försenat med en anledning
        public bool MarkAsDelayed(Package package, string reason, out string message)
        {
            ArgumentNullException.ThrowIfNull(package);

            if (string.IsNullOrWhiteSpace(reason))
            {
                reason = "Okänd anledning";
            }

            bool success = CurrentState.TryMarkedDelayed(package, reason, out message);

            if (success)
            {
                package.NotifyObservers(message);
            }

            return success;
        }

        // Hämtar den föra state från paketets historik
        public IPackageState GetPreviousState() 
        {
            // Kollar om det finns minst två state i historiken
            if (_stateObjects.Count >= 2) 
            {
                // Returnerar det sista objektet i listan, alltså föra staten
                return _stateObjects[_stateObjects.Count - 2];
            }
            // Om inget tidigare state finns, returnerar vi nuvarande state
            return CurrentState;
        }
    }
}
