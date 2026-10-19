using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public class Package
    {
        public int PackageID { get; private set; }
        public decimal PackageWeight { get; private set; }
        public decimal PackageDistance { get; private set; }
        public string SenderAddress { get; private set; }
        public string ReciverAddress { get; private set; }
        public string CurrentLocation { get; private set; }
        public StateController Status { get; private set; }

        // Skapar ett nytt paket med all nödvändig information och kopplar det till StateController.
        public Package(int packageID, decimal packageWeight, decimal packageDistance, string senderAddress, string reciverAddress, StateController status)
        {
            PackageID = packageID;
            PackageWeight = packageWeight;
            PackageDistance = packageDistance;
            SenderAddress = senderAddress;
            ReciverAddress = reciverAddress;
            CurrentLocation = CurrentLocation;

            Status = status;
        }
    }
}
