using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.UI
{
    // Första menyn där användaren väljer perspektiv (kund eller admin).
    public class StartMenu
    {
        private readonly List<IMenu> _menus;

        public StartMenu(IEnumerable<IMenu> menus)
        {
            ArgumentNullException.ThrowIfNull(menus);
            _menus = menus.ToList();
        }

        public void Run()
        {
            while (true)
            {
                ConsoleInput.ClearScreen();   
                Console.WriteLine("========== PacketTracker ==========");
                Console.WriteLine("Välj perspektiv:");
                for (int i = 0; i < _menus.Count; i++)
                {
                    Console.WriteLine($" {i + 1}. {_menus[i].Title}");
                }
                Console.WriteLine(" 0. Avsluta");
                Console.WriteLine("===================================");

                int choice = ConsoleInput.ReadInt("Val: ");
                Console.WriteLine();

                if (choice == 0)
                {
                    Console.WriteLine("Programmet avslutas.");
                    return;
                }

                if (choice < 1 || choice > _menus.Count)
                {
                    Console.WriteLine($"Ogiltigt val. Ange en siffra mellan 0 och {_menus.Count}.");
                    ConsoleInput.WaitForKey();
                    continue;
                }

                _menus[choice - 1].Run();
            }
        }
    }
}
