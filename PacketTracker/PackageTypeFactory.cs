using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    public static class PackageTypeFactory
    {
        private static readonly Dictionary<string, IPackageType> _registeredTypes = new Dictionary<string, IPackageType>(StringComparer.OrdinalIgnoreCase);

        public static void RegisterFactory(string shippingName, IPackageType packageType)
        {
            _registeredTypes[shippingName] = packageType;
        }

        public static IPackageType CreatePackageType(string shippingName)
        {
            if (_registeredTypes.TryGetValue(shippingName, out var packageType))
            {
                return packageType;
            }
            throw new ArgumentException($"No package type registered for shipping name: {shippingName}");
        }
    }
}
