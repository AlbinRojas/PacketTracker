using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketTracker.Interface;

namespace PacketTracker
{
    public class EmailCustomerNotification : IObserver
    {
        private string _customerEmail;

        public EmailCustomerNotification(string customerEmail)
        {
            _customerEmail = customerEmail;
        }

        public void StatusChangedNotification(string packageID, string newStatus, string currentStation)
        {
            Console.WriteLine($"E-post till {_customerEmail}:\n Paket: {packageID} har ny status: {newStatus}. Den har nu nått {currentStation}.");
        }
    }
}
