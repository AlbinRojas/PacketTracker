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
        private string _customerEmail;

        public EmailCustomerNotification(string customerEmail)
        {
            _customerEmail = customerEmail;
        }
        
        public void Update(Package package, string message)
        {
            Console.WriteLine($"E-post till {_customerEmail}:\n Paket: {package.PackageID}\n Status: {package.Status.CurrentState.StatusName}\n Station: {package.CurrentStation?.StationName}");

            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine($"Meddelande: {message}");
            }

            Console.WriteLine();
        }
    }
}
