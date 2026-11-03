using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.ShippingValues
{
    // Används nu av Package (avsändare och mottagare) i stället för vanliga strängar.
    public class Address
    {
        public string AddressName { get; }
        public string City { get; }         

        public Address(string addressName, string city)
        {
            // tomma adresser är inte tillåtna
            if (string.IsNullOrWhiteSpace(addressName))
            {
                throw new ArgumentException("Gatuadressen får inte vara tom.", nameof(addressName));
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("Orten får inte vara tom.", nameof(city));
            }

            AddressName = addressName.Trim();
            City = city.Trim();
        }

        // Snygg utskrift
        public override string ToString() => $"{AddressName}, {City}";
    }
}
