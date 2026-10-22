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

        // Behåller textloggen för utskrifter
        public readonly List<string> _statusHistory = new();
        // Lista som sparar själva objekten
        private readonly List<IPackageState> _stateObjects = new();

        public StateController(IPackageState initialState)
        {
            CurrentState = initialState;
            _statusHistory.Add($"{DateTime.Now:HH:mm:ss} - {initialState.StatusName}");
            _stateObjects.Add(initialState);
        }

        public void SetState(IPackageState newState)
        {
            CurrentState = newState;
            _statusHistory.Add($"{DateTime.Now:HH:mm:ss} - {newState.StatusName}");
            _stateObjects.Add(newState);
        }

        // flyttar paketet till nästa state i flödet
        public void ProceedToNextState(Package package) 
        {
            // Låter nuvarande status försöka gå vidare, och sparar meddelandet
            bool success = CurrentState.TryNextState(package, out string message);

            // Om bytet gick bra
            if (success) 
            {
                Console.WriteLine($"Uppdatering: {message}");
                NotifyObservers($"Paketet har änlänt till en ny station.");
            }
            // Om bytet inte gick
            else 
            {
                Console.WriteLine($"Information: {message}");
            }
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

        public void MarkAsDelayed(string reason) 
        {
            // Skapar ett nytt delayedstate och skickar med anledningen
            IPackageState delayedState = new DelayedState(reason);
            // Sätter det nya delayedstate som aktivt
            SetState(delayedState);
        }


    }
}
