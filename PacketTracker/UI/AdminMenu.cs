using PacketTracker.Services;
using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.UI
{
    // Admin registrerar paket, ändrar status och ser all intern information.
    public class AdminMenu : IMenu
    {
        private readonly PackageService _service;

        public string Title => "Admin (logistikbolaget)";

        public AdminMenu(PackageService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void Run()
        {
            while (true)
            {
                ConsoleInput.ClearScreen();   //  ren skärm inför varje visning av menyn
                Console.WriteLine("---------- Admin ----------");
                Console.WriteLine(" 1. Registrera nytt paket");
                Console.WriteLine(" 2. Visa paketdetaljer");
                Console.WriteLine(" 3. Flytta paket till nästa steg");
                Console.WriteLine(" 4. Markera paket som försenat");
                Console.WriteLine(" 5. Lista alla paket i prioritetsordning");
                Console.WriteLine(" 6. Flytta fram alla paket i prioritetsordning");
                Console.WriteLine(" 0. Tillbaka");
                Console.WriteLine("---------------------------");

                string choice = ConsoleInput.ReadText("Val: ");

                if (choice == "0")
                {
                    return;   // tillbaka till startmenyn
                }

                ConsoleInput.ClearScreen();   //  visa bara resultatet av valet

                // inget fel ska kunna krascha programmet
                try
                {
                    switch (choice)
                    {
                        case "1": CreatePackage(); break;
                        case "2": ShowPackageDetails(); break;
                        case "3": AdvancePackage(); break;
                        case "4": MarkDelayed(); break;
                        case "5": ListByPriority(); break;
                        case "6": AdvanceAll(); break;
                        default:
                            Console.WriteLine("Ogiltigt val. Ange en siffra mellan 0 och 6.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
                }

                ConsoleInput.WaitForKey();   // vänta så att resultatet hinner läsas
            }
        }

        private void CreatePackage()
        {
            Console.WriteLine("--- Registrera paket ---");
            string type = ConsoleInput.ReadShippingType();   // frågar igen direkt vid ogiltig frakttyp
            decimal weight = ConsoleInput.ReadWeight();         // kontrollerar 1–1500 kg
            decimal distance = ConsoleInput.ReadDistance();     // kontrollerar 1–2500 km
            Address sender = ConsoleInput.ReadAddress("Avsändare");
            Address receiver = ConsoleInput.ReadAddress("Mottagare");
            List<Station> route = ConsoleInput.ReadRoute();

            if (!_service.TryCreatePackage(type, weight, distance, sender, receiver, route, out Package? package, out string message))
            {
                Console.WriteLine(message);
                return;
            }

            Console.WriteLine(message);

            string email = ConsoleInput.ReadText("Kundens e-post för notiser (lämna tomt för ingen): ");
            if (!string.IsNullOrWhiteSpace(email))
            {
                try
                {
                    _service.SubscribeToNotifications(package.PackageID, new EmailCustomerNotification(email), out string subMessage);
                    Console.WriteLine(subMessage);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Notiser aktiverades inte: {ex.Message}");
                }
            }
        }

        private void ShowPackageDetails()
        {
            int id = ConsoleInput.ReadInt("Paket-ID: ");

            if (_service.TryGetPackage(id, out Package? package))
            {
                PackagePrinter.PrintAdminView(package);
            }
            else
            {
                Console.WriteLine($"Inget paket med ID {id} hittades.");
            }
        }

        private void AdvancePackage()
        {
            int id = ConsoleInput.ReadInt("Paket-ID: ");
            bool success = _service.AdvancePackage(id, out string message);
            Console.WriteLine(success ? $"Uppdatering: {message}" : $"Information: {message}");
        }

        private void MarkDelayed()
        {
            int id = ConsoleInput.ReadInt("Paket-ID: ");
            string reason = ConsoleInput.ReadText("Anledning till försening: ");
            bool success = _service.MarkPackageDelayed(id, reason, out string message);
            Console.WriteLine(success ? $"Uppdatering: {message}" : $"Information: {message}");
        }

        private void ListByPriority()
        {
            Console.WriteLine("--- Alla paket i prioritetsordning ---");
            List<Package> sorted = _service.GetPackagesInPriorityOrder();

            if (sorted.Count == 0)
            {
                Console.WriteLine("Inga paket finns registrerade.");
                return;
            }

            foreach (Package package in sorted)
            {
                PackagePrinter.PrintLine(package);
            }
        }

        private void AdvanceAll()
        {
            Console.WriteLine("--- Flyttar fram alla paket (högst prioritet först) ---");
            List<string> results = _service.AdvanceAllInPriorityOrder();

            if (results.Count == 0)
            {
                Console.WriteLine("Det finns inga paket som kan flyttas fram.");
                return;
            }

            foreach (string line in results)
            {
                Console.WriteLine(line);
            }
        }
    }
}
