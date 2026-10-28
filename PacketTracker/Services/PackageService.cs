using PacketTracker.Algorithms;
using PacketTracker.Interface;
using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.Services
{
    public class PackageService
    {
        // "Databas" i minnet. Dictionary ger O(1) uppslag på paket-ID, som är den vanligaste operationen.
        private readonly Dictionary<int, Package> _packages = new();

        private int _nextId = 1001;

        public int Count => _packages.Count;

        // Skapar ett paket. Returnerar false med ett felmeddelande i stället för att krascha.
        public bool TryCreatePackage(string shippingType, decimal weightKg, decimal distanceKm,
                                     Address sender, Address receiver, List<Station> route,
                                     [NotNullWhen(true)] out Package? package, out string message)
        {
            package = null;

            if (!PackageTypeFactory.TryCreatePackageType(shippingType, out IPackageType? packageType))
            {
                string validTypes = string.Join(", ", PackageTypeFactory.GetRegisteredTypeNames());
                message = $"Okänd frakttyp: '{shippingType}'. Giltiga frakttyper: {validTypes}.";
                return false;
            }

            try
            {
                var weight = new Weight(weightKg);
                var distance = new Distance(distanceKm);
                package = new Package(_nextId, weight, distance, sender, receiver, packageType, route, DateTime.Now);
            }
            catch (ArgumentException ex)
            {
                message = $"Paketet kunde inte skapas: {ex.Message}";
                package = null;
                return false;
            }

            _packages.Add(package.PackageID, package);
            _nextId++;

            message = $"Paket {package.PackageID} skapat ({packageType.GetTypeName()}, {package.Price:0.00} kr).";
            return true;
        }

       
        public bool TryGetPackage(int packageId, [NotNullWhen(true)] out Package? package)
        {
            return _packages.TryGetValue(packageId, out package);
        }

        public bool AdvancePackage(int packageId, out string message)
        {
            if (!TryGetPackage(packageId, out Package? package))
            {
                message = $"Inget paket med ID {packageId} hittades.";
                return false;
            }

            return package.Status.ProceedToNextState(package, out message);
        }

        public bool MarkPackageDelayed(int packageId, string reason, out string message)
        {
            if (!TryGetPackage(packageId, out Package? package))
            {
                message = $"Inget paket med ID {packageId} hittades.";
                return false;
            }

            return package.Status.MarkAsDelayed(package, reason, out message);
        }

        public bool SubscribeToNotifications(int packageId, IPackageObserver observer, out string message)
        {
            if (!TryGetPackage(packageId, out Package? package))
            {
                message = $"Inget paket med ID {packageId} hittades.";
                return false;
            }

            package.AddObserver(observer);
            message = $"Notiser aktiverade för paket {packageId}.";
            return true;
        }

        // Prisförfrågan för kunden. Räknar ut pris, prioritet och leveransdatum för varje frakttyp.
        // Alla frakttyper anropas på exakt samma sätt via IPackageType.
        public bool TryGetPriceQuotes(decimal weightKg, decimal distanceKm, out List<PriceQuote> quotes, out string message)
        {
            quotes = new List<PriceQuote>();

            Weight weight;
            Distance distance;
            try
            {
                weight = new Weight(weightKg);
                distance = new Distance(distanceKm);
            }
            catch (ArgumentException ex)
            {
                message = ex.Message;
                return false;
            }

            foreach (string typeName in PackageTypeFactory.GetRegisteredTypeNames())
            {
                IPackageType type = PackageTypeFactory.CreatePackageType(typeName);
                quotes.Add(new PriceQuote(
                    type.GetTypeName(),
                    type.Priority,
                    type.CalculatePrice(weight, distance),
                    type.CalculateEstimateDelivery(distance, DateTime.Today)));
            }

            message = $"Prisförslag för {weight} och {distance}.";
            return true;
        }

        public IReadOnlyList<Package> GetAllPackages()
        {
            return _packages.Values.ToList();
        }

        // Prioritet i transportkedjan: paket sorteras med egen sorteringsalgoritm
        public List<Package> GetPackagesInPriorityOrder()
        {
            return PackageSorter.SortByPriority(_packages.Values);
        }

        // Flyttar fram alla ej levererade paket ett steg, i prioritetsordning (Express först).
        public List<string> AdvanceAllInPriorityOrder()
        {
            var results = new List<string>();

            foreach (Package package in GetPackagesInPriorityOrder())
            {
                if (package.Status.IsDelivered)
                {
                    continue;
                }

                // Försenade paket står still tills förseningen hävs manuellt
                if (package.Status.CurrentState is DelayedState)
                {
                    results.Add($"Paket {package.PackageID} ({package.PackageType.GetTypeName()}): står still – {package.Status.CurrentState.StatusName}");
                    continue;
                }

                package.Status.ProceedToNextState(package, out string message);
                results.Add($"Paket {package.PackageID} ({package.PackageType.GetTypeName()}): {message}");
            }

            return results;
        }
    }
}
