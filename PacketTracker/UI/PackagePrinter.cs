using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.UI
{
    // Ansvarar bara för hur paket skrivs ut. Delas av kund- och adminmenyn,
    // så att utskriftskoden inte dupliceras.
    public static class PackagePrinter
    {
        // Kortfattad rad, används i adminens lista
        public static void PrintLine(Package package)
        {
            Console.WriteLine($"#{package.PackageID} | {package.PackageType.GetTypeName(),-8} | prio {package.PackageType.Priority} | " +
                              $"{package.Status.CurrentState.StatusName} | leverans {package.EstimatedDeliveryDate:yyyy-MM-dd} | {package.Price:0.00} kr");
        }

        // Kundens vy: det kunden behöver veta, utan intern information
        public static void PrintCustomerView(Package package)
        {
            Console.WriteLine($"--- Ditt paket {package.PackageID} ---");
            Console.WriteLine($"Från:              {package.SenderAddress.City}");
            Console.WriteLine($"Till:              {package.ReceiverAddress}");
            Console.WriteLine($"Frakttyp:          {package.PackageType.GetTypeName()}");
            Console.WriteLine($"Status:            {package.Status.CurrentState.StatusName}");
            Console.WriteLine($"Senast sett vid:   {package.CurrentStation?.StationName ?? "Ej avgått från terminalen"}");
            Console.WriteLine($"Beräknad leverans: {package.EstimatedDeliveryDate:yyyy-MM-dd}");
            PrintHistory(package);
        }

        // Adminens vy: all information om paketet
        public static void PrintAdminView(Package package)
        {
            Console.WriteLine($"--- Paket {package.PackageID} (adminvy) ---");
            Console.WriteLine($"Frakttyp:          {package.PackageType.GetTypeName()} (prioritet {package.PackageType.Priority})");
            Console.WriteLine($"Avsändare:         {package.SenderAddress}");
            Console.WriteLine($"Mottagare:         {package.ReceiverAddress}");
            Console.WriteLine($"Vikt / avstånd:    {package.Weight} / {package.Distance}");
            Console.WriteLine($"Pris:              {package.Price:0.00} kr");
            Console.WriteLine($"Skickat:           {package.SendDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Beräknad leverans: {package.EstimatedDeliveryDate:yyyy-MM-dd}");
            Console.WriteLine($"Status:            {package.Status.CurrentState.StatusName}");
            Console.WriteLine($"Nuvarande station: {package.CurrentStation?.StationName ?? "Ej avgått"}");
            Console.WriteLine($"Stationer kvar:    {package.RemainingStations}");
            PrintHistory(package);
            PrintNotifications(package);
        }

        private static void PrintHistory(Package package)
        {
            Console.WriteLine("Statushistorik:");
            foreach (string entry in package.Status.StatusHistory)
            {
                Console.WriteLine($"  {entry}");
            }
        }

        private static void PrintNotifications(Package package)
        {
            Console.WriteLine("Skickade notiser:");
            
            if (package.ObserverCount == 0)
            {
                Console.WriteLine(" Inga notiser är aktiverade för detta paketet.");
                return;
            }

            if (package.NotificationLog.Count == 0)
            {
                Console.WriteLine(" Notiser är aktiverade, men inget mail har skickats ännu.");
                return;
            }

            foreach (string entry in package.NotificationLog)
            {
                Console.WriteLine($"  {entry}");
            }
        }
    }
}
