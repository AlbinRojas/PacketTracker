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
        private readonly string _customerEmail;

        public string CustomerEmail => _customerEmail;

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

            string time = package.Status.TimeStamp;
            // visar "Ej avgått" om paketet ännu inte är på en station
            Console.WriteLine($"  [E-post till {_customerEmail}]");
            Console.WriteLine($"   Paket:   {package.PackageID}");
            Console.WriteLine($"   Status:  {package.Status.CurrentState.StatusName}");
            Console.WriteLine($"   Station: {package.CurrentStation?.StationName ?? "Ej avgått"}");

            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine($"   Meddelande: {message}");
            }
            package.LogNotification($"{time} - E-post skickad till {_customerEmail}: {message}");
        }

        public override bool Equals(object? obj)
        {
            return obj is EmailCustomerNotification other
                     && string.Equals(_customerEmail, other._customerEmail, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(_customerEmail);
        }
    }
}