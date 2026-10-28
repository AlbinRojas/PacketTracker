using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace PacketTracker
{
    //  nya frakttyper registreras utan att fabriken ändras.
    public static class PackageTypeFactory
    {
        private static readonly Dictionary<string, IPackageType> _registeredTypes =
            new Dictionary<string, IPackageType>(StringComparer.OrdinalIgnoreCase);

        public static void RegisterFactory(string shippingName, IPackageType packageType)
        {
           
            if (string.IsNullOrWhiteSpace(shippingName))
            {
                throw new ArgumentException("Frakttypen måste ha ett namn.", nameof(shippingName));
            }
            ArgumentNullException.ThrowIfNull(packageType);

            _registeredTypes[shippingName] = packageType;
        }

        public static IPackageType CreatePackageType(string shippingName)
        {
            if (_registeredTypes.TryGetValue(shippingName, out var packageType))
            {
                return packageType;
            }
            throw new ArgumentException($"Ingen frakttyp registrerad med namnet: {shippingName}");
        }

        // Säker variant som inte kastar undantag, används av PackageService.
        public static bool TryCreatePackageType(string shippingName, [NotNullWhen(true)] out IPackageType? packageType)
        {
            packageType = null;
            if (string.IsNullOrWhiteSpace(shippingName))
            {
                return false;
            }
            return _registeredTypes.TryGetValue(shippingName.Trim(), out packageType);
        }

        // Används av menyn för att visa vilka frakttyper som finns.
        public static IReadOnlyList<string> GetRegisteredTypeNames()
        {
            return _registeredTypes.Keys.ToList();
        }
    }
}
