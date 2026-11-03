using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketTracker.Interface;

namespace PacketTracker
{
    public class EmailCustomerNotification : IPackageObserver
    {
        private readonly string _customerEmail;   // readonly

        public EmailCustomerNotification(string customerEmail)
        {
            // enkel kontroll av e-postadressen
            if (string.IsNullOrWhiteSpace(customerEmail) || !customerEmail.Contains('@'))
            {
                throw new ArgumentException("Ogiltig e-postadress.", nameof(customerEmail));
            }

            _customerEmail = customerEmail.Trim();
        }

        public void Update(Package package, string message)
        {
            // visar "Ej avgått" om paketet ännu inte är på en station
            Console.WriteLine($"  [E-post till {_customerEmail}]");
            Console.WriteLine($"   Paket:   {package.PackageID}");
            Console.WriteLine($"   Status:  {package.Status.CurrentState.StatusName}");
            Console.WriteLine($"   Station: {package.CurrentStation?.StationName ?? "Ej avgått"}");

            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine($"   Meddelande: {message}");
            }
        }
    }
}