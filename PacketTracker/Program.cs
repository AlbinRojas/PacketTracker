using PacketTracker.Services;
using PacketTracker.ShippingValues;
using PacketTracker.TypeAlgorithm;
using PacketTracker.UI;

namespace PacketTracker
{
    // Startpunkt. Registrerar frakttyper, skapar tjänsten, lägger in testdata och startar startmenyn.
    public class Program
    {
        public static void Main(string[] args)
        {
            // Gör att å, ä, ö visas rätt i konsolen
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1) Registrera frakttyperna i fabriken.
            //    En ny frakttyp = en ny klass som implementerar IPackageType + en rad här.
            PackageTypeFactory.RegisterFactory("Standard", new Standard());
            PackageTypeFactory.RegisterFactory("Express", new Express());
            PackageTypeFactory.RegisterFactory("Fragile", new Fragile());

            // 2) Skapa tjänsten som håller paketen i minnet
            var service = new PackageService();

            // 3) Lägg in några paket så att det finns något att spåra direkt
            SeedData(service);

            // 4) Starta startmenyn där användaren väljer perspektiv.
            //    En ny roll = en ny klass som implementerar IMenu + en rad i listan nedan.
            var startMenu = new StartMenu(new List<IMenu>
            {
                new CustomerMenu(service),
                new AdminMenu(service)
            });
            startMenu.Run();
        }

        private static void SeedData(PackageService service)
        {
            var falun = new Address("Åsgatan 10", "Falun");

            service.TryCreatePackage("Standard", 4m, 480m, falun, new Address("Storgatan 3", "Umeå"),
                new List<Station> { new Station("Terminal Borlänge"), new Station("Terminal Sundsvall") },
                out _, out _);

            service.TryCreatePackage("Express", 1.5m, 230m, falun, new Address("Kungsgatan 8", "Stockholm"),
                new List<Station> { new Station("Terminal Borlänge"), new Station("Terminal Västerås") },
                out _, out _);

            service.TryCreatePackage("Fragile", 7m, 460m, falun, new Address("Avenyn 12", "Göteborg"),
                new List<Station> { new Station("Terminal Örebro"), new Station("Terminal Jönköping") },
                out _, out _);
        }
    }
}

