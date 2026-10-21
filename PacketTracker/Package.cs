using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketTracker.ShippingValues;

namespace PacketTracker
{
    public class Package
    {
 
        public int PackageID { get; private set; }
        public decimal PackageWeight { get; private set; }
        public decimal PackageDistance { get; private set; }
        public string SenderAddress { get; private set; }
        public string ReciverAddress { get; private set; }
        public IPackageType PackageType { get; private set; }
        public DateTime EstimatedDeliveryDate { get; private set; }

        // Visar vilken station paketet är på just nu
        public Station CurrentStation { get; private set; }

        // HÄR LIGGER KÖN! (Paketets resplan)
        private Queue<Station> _route;

        // Omvandlar listan med stationer till en kö för detta paket
        public StateController Status { get; private set; }

        // Skapar ett nytt paket med all nödvändig information och kopplar det till StateController.
        public Package(int packageID, decimal packageWeight, decimal packageDistance, string senderAddress, string reciverAddress, IPackageType packageType, StateController status, List<Station> route)
        {
            PackageID = packageID;
            PackageWeight = packageWeight;
            PackageDistance = packageDistance;
            SenderAddress = senderAddress;
            ReciverAddress = reciverAddress;

            PackageType = packageType;
            EstimatedDeliveryDate = packageType.CalculateEstimateDelivery(new Distance((decimal)packageDistance));

            _route = new Queue<Station>(route);

            // Här skapas StateController automatiskt med ReceivedInfoState som startläge
            Status = new StateController(new ReceivedInfoState());
        }
        // Metod för att byta state på paketet
        public void SetState(IPackageState newState)
        {
            Status.SetState(newState);
        }

        // Metod för att flytta paketet till nästa station i kön
        public bool MoveToNextStation()
        {
            if (_route.Count > 0)
            {
                CurrentStation = _route.Dequeue();
                return true;
            }

            return false; // kön är tom, alltså är paketet färdiglevererat
        }
    }
}
