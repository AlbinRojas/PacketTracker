using System;
using PacketTracker.Interface;
using PacketTracker.ShippingValues;
using PacketTracker.SpeedAlgorithm;

namespace PacketTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Säkerställ att åäö visas korrekt i konsolen
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=========================================");
            Console.WriteLine("   PAKETSPÅRNINGSSYSTEM (GoF Patterns)   ");
            Console.WriteLine("=========================================\n");

            // 1. Factory & Strategy Pattern: Registrera pakettyper
            PackageTypeFactory.RegisterFactory("Standard", new Standard());
            PackageTypeFactory.RegisterFactory("Express", new Express());
            PackageTypeFactory.RegisterFactory("Fragile", new Fragile());

            // Skapa värdeobjekt (Value Objects)
            Weight weight = new Weight { WeightInKg = 3.5m };
            Distance distance = new Distance { DistanceInKm = 600m };

            // Välj en pakettyp via Factory (Strategy-mönstret)
            IPackageType packageType = PackageTypeFactory.CreatePackageType("Express");
            Console.WriteLine($"[Strategy & Factory] Vald pakettyp: {packageType.GetTypeName()}");
            Console.WriteLine($"Beräknat pris: {packageType.CalculatePrice(weight, distance):C}");
            Console.WriteLine($"Beräknad leverans: {packageType.CalculateEstimateDelivery(distance):yyyy-MM-dd}\n");

            // 2. State & Observer Pattern: Skapa paket och prenumerant
            Package package = new Package(
                packageID: 5042,
                packageWeight: weight.WeightInKg,
                packageDistance: distance.DistanceInKm,
                senderAddress: "Stockholm",
                reciverAddress: "Göteborg",
                status: new StateController(new ReceivedInfoState())
            );

            // Skapa en Observer (E-postnotis till kund)
            IObserver customerObserver = new EmailCustomerNotification("anna.andersson@exempel.se");

            Console.WriteLine("--- SIMULERAR PAKETETS RESA ---");

            // Första statusuppdatering (Received Info -> In Transit)
            if (package.Status.CurrentState is ReceivedInfoState receivedState)
            {
                receivedState.TryNextState(package, out string msg1);
                customerObserver.Update(package, msg1);
            }

            // Gå vidare till nästa steg (In Transit -> Out For Delivery)
            if (package.Status.CurrentState is InTransitState inTransitState)
            {
                inTransitState.TryNextState(package, out string msg2);
                customerObserver.Update(package, msg2);
            }

            // Slutgiltigt steg (Out For Delivery -> Delivered)
            if (package.Status.CurrentState is OutForDeliveryState outForDeliveryState)
            {
                outForDeliveryState.TryNextState(package, out string msg3);
                customerObserver.Update(package, msg3);
            }

            // 3. Visa historik från StateController
            Console.WriteLine("\n--- PAKETETS HISTORIK ---");
            foreach (var historyRecord in package.Status.StatusHistory)
            {
                Console.WriteLine(historyRecord);
            }

            Console.WriteLine("\nKörningen är klar. Tryck på valfri tangent för att avsluta...");
            Console.ReadKey();
        }
    }
}