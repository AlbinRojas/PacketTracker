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
        public readonly List<string> _statusHistory = new();
        public IReadOnlyList<string> StatusHistory => _statusHistory;

        public StateController(IPackageState initialState)
        {
            CurrentState = initialState;
            _statusHistory.Add($"{DateTime.Now:HH:mm:ss} - {initialState.StatusName}");
        }

        public void SetState(IPackageState newState)
        {
            CurrentState = newState;
            _statusHistory.Add($"{DateTime.Now:HH:mm:ss} - {newState.StatusName}");
        }
    }
}
