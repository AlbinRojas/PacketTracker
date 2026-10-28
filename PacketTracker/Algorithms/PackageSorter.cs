using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.Algorithms
{
    public static class PackageSorter
    {
        // Sorterar på prioritet (1 först) och därefter på beräknat leveransdatum (tidigast först).
        public static List<Package> SortByPriority(IEnumerable<Package> packages)
        {
            ArgumentNullException.ThrowIfNull(packages);

            // Kopierar så att originalsamlingen inte ändras
            var list = new List<Package>(packages);

            for (int i = 1; i < list.Count; i++)
            {
                Package current = list[i];
                int j = i - 1;

                // Flytta större element ett steg åt höger tills rätt plats för "current" hittas
                while (j >= 0 && Compare(list[j], current) > 0)
                {
                    list[j + 1] = list[j];
                    j--;
                }

                list[j + 1] = current;
            }

            return list;
        }

        // Jämförelseregel: först prioritet, sedan leveransdatum
        private static int Compare(Package a, Package b)
        {
            int byPriority = a.PackageType.Priority.CompareTo(b.PackageType.Priority);
            if (byPriority != 0)
            {
                return byPriority;
            }

            return a.EstimatedDeliveryDate.CompareTo(b.EstimatedDeliveryDate);
        }















    }
}
