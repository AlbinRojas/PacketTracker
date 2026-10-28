using PacketTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.UI
{
    // Kundens perspektiv. Kunden kan bara läsa information och prenumerera på notiser –
    // inte ändra ett pakets status. Kunden ser heller inte intern information som återstående stationer.
    public class CustomerMenu : IMenu
    {
        private readonly PackageService _service;

        public string Title => "Kund";

        public CustomerMenu(PackageService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void Run()
        {
            while (true)
            {
                ConsoleInput.ClearScreen();   // NY: ren skärm inför varje visning av menyn
                Console.WriteLine("---------- Kund ----------");
                Console.WriteLine(" 1. Spåra mitt paket");
                Console.WriteLine(" 2. Få e-postnotiser för mitt paket");
                Console.WriteLine(" 3. Beräkna pris och leveranstid");
                Console.WriteLine(" 0. Tillbaka");
                Console.WriteLine("--------------------------");

                string choice = ConsoleInput.ReadText("Val: ");

                if (choice == "0")
                {
                    return;   // tillbaka till startmenyn
                }

                ConsoleInput.ClearScreen();   // visa bara resultatet av valet

                // inget fel ska kunna krascha programmet
                try
                {
                    switch (choice)
                    {
                        case "1": TrackPackage(); break;
                        case "2": SubscribeToNotifications(); break;
                        case "3": ShowPriceQuotes(); break;
                        default:
                            Console.WriteLine("Ogiltigt val. Ange en siffra mellan 0 och 3.");
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

        private void TrackPackage()
        {
            int id = ConsoleInput.ReadInt("Ange ditt paket-ID: ");

            if (_service.TryGetPackage(id, out Package? package))
            {
                PackagePrinter.PrintCustomerView(package);
            }
            else
            {
                Console.WriteLine($"Inget paket med ID {id} hittades. Kontrollera numret och försök igen.");
            }
        }

        private void SubscribeToNotifications()
        {
            int id = ConsoleInput.ReadInt("Ange ditt paket-ID: ");

            if (!_service.TryGetPackage(id, out _))
            {
                Console.WriteLine($"Inget paket med ID {id} hittades.");
                return;
            }

            string email = ConsoleInput.ReadText("Din e-postadress: ");

            try
            {
                _service.SubscribeToNotifications(id, new EmailCustomerNotification(email), out string message);
                Console.WriteLine(message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Notiser aktiverades inte: {ex.Message}");
            }
        }

        private void ShowPriceQuotes()
        {
            Console.WriteLine("--- Beräkna pris och leveranstid ---");
            decimal weight = ConsoleInput.ReadWeight();         // kontrollerar 1–1500 kg
            decimal distance = ConsoleInput.ReadDistance();     // kontrollerar 1–2500 km

            if (!_service.TryGetPriceQuotes(weight, distance, out List<PriceQuote> quotes, out string message))
            {
                Console.WriteLine(message);
                return;
            }

            Console.WriteLine();
            Console.WriteLine(message);
            Console.WriteLine($"{"Frakttyp",-10} {"Prioritet",-10} {"Pris",12}   {"Beräknad leverans"}");
            foreach (PriceQuote quote in quotes)
            {
                Console.WriteLine($"{quote.TypeName,-10} {quote.Priority,-10} {quote.Price,9:0.00} kr   {quote.EstimatedDelivery:yyyy-MM-dd}");
            }
        }
    }
}
