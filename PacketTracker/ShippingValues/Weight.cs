using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.ShippingValues
{
    // Värdeobjekt. Egenskapen har bara "get" och värdet valideras.
    public class Weight
    {
        // Tillåtna gränser. Regeln ligger här, så den gäller i hela systemet,
        // och menyn läser samma konstanter i stället för att hårdkoda talen.
        public const decimal MinKg = 1m;
        public const decimal MaxKg = 1500m;

        public decimal WeightInKg { get; }

        public Weight(decimal weightInKg)
        {
            // kontrollerar både min- och maxgräns
            if (weightInKg < MinKg || weightInKg > MaxKg)
            {
                throw new ArgumentException($"Vikten måste vara mellan {MinKg} och {MaxKg} kg.", nameof(weightInKg));
            }

            WeightInKg = weightInKg;
        }

        public override string ToString() => $"{WeightInKg} kg";
    }
}
