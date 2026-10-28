using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker
{
    // varje frakttyp kapslar in sin egen pris-, tids- och prioritetslogik.
    public interface IPackageType
    {
        // Prioritet i transportkedjan. Lägre tal = högre prioritet (1 = skickas först).
        int Priority { get; }

        decimal CalculatePrice(Weight weight, Distance distance);

        // tar emot avsändningsdatum i stället för att använda DateTime.Now inuti.
        // Det gör metoden testbar (samma indata ger alltid samma resultat).
        DateTime CalculateEstimateDelivery(Distance distance, DateTime sendDate);

        string GetTypeName();
    }
}
