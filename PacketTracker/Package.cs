using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketTracker.ShippingValues;
using PacketTracker.Interface;  
using PacketTracker.TypeAlgorithm;

namespace PacketTracker
{
    public class Package
    {
 
        public int PackageID { get; }
        public Weight Weight { get; }
        public Distance Distance { get; }
        public Address SenderAddress { get; }
        public Address ReceiverAddress { get; }
        public IPackageType PackageType { get; }
        public DateTime SendDate { get; } 
        public DateTime EstimatedDeliveryDate { get; }

        public decimal Price => PackageType.CalculatePrice(Weight, Distance);

        // Visar vilken station paketet är på just nu
        public Station? CurrentStation { get; private set; }

        public int RemainingStations => _route.Count;

        // HÄR LIGGER KÖN! (Paketets resplan)
        private readonly Queue<Station> _route;

        // Omvandlar listan med stationer till en kö för detta paket
        public StateController Status { get; }
        
        private readonly List<IPackageObserver> _observers = new List<IPackageObserver>();

        // Skapar ett nytt paket med all nödvändig information och kopplar det till StateController.
        public Package(int packageID, Weight weight, Distance distance, Address senderAddress,
                       Address receiverAddress, IPackageType packageType, List<Station> route, DateTime sendDate)
        {
           
            if (packageID <= 0)
            {
                throw new ArgumentException("Paket-ID måste vara större än 0.", nameof(packageID));
            }
            ArgumentNullException.ThrowIfNull(weight);
            ArgumentNullException.ThrowIfNull(distance);
            ArgumentNullException.ThrowIfNull(senderAddress);
            ArgumentNullException.ThrowIfNull(receiverAddress);
            ArgumentNullException.ThrowIfNull(packageType);
            ArgumentNullException.ThrowIfNull(route);

            PackageID = packageID;
            Weight = weight;
            Distance = distance;    
            SenderAddress = senderAddress;
            ReceiverAddress = receiverAddress;
            PackageType = packageType;
            SendDate = sendDate;

            // skickar med sendDate 
            EstimatedDeliveryDate = packageType.CalculateEstimateDelivery(distance, sendDate);

            _route = new Queue<Station>(route);

            // StateController skapas med ReceivedInfoState som startläge
            Status = new StateController(new ReceivedInfoState());
        }

        public void AddObserver(IPackageObserver observer)
        {
            ArgumentNullException.ThrowIfNull(observer);

            // undvik att samma observatör läggs till två gånger
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(IPackageObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(this, message);
            }
        }

        public int ObserverCount => _observers.Count;
        private readonly List<string> _notificationLog = new();
        public IReadOnlyList<string> NotificationLog => _notificationLog;

        public void LogNotification(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _notificationLog.Add(message);
            }
        }

        // Byter state på paketet (anropas av state-klasserna)
        public void SetState(IPackageState newState)
        {
            Status.SetState(newState);
        }

        // Flyttar paketet till nästa station i kön
        public bool MoveToNextStation()
        {
            if (_route.Count > 0)
            {
                CurrentStation = _route.Dequeue();
                Status.AddHistoryStation($"Paketet har anlänt till station: {CurrentStation.StationName}");
                return true;
            }

            return false; // kön är tom, paketet har passerat alla stationer
        }
    }
}