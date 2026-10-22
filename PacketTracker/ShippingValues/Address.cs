using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.ShippingValues
{
    public class Address
    {
        public string AddressName { get; set; }
        public string City { get; set; }

        public Address(string addressName, string city) 
        {
            AddressName = addressName;
            City = city;
        }
    }
}
