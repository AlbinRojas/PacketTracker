using PacketTracker.ShippingValues;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracker.UI
{
    // Säker inmatning från konsolen. Delas av alla menyer.
    // Ingen metod kraschar på felaktig inmatning – användaren får försöka igen.
    public static class ConsoleInput
    {
        // Rensar konsolen så att varje meny visas på en ren skärm.
        public static void ClearScreen()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                // Console.Clear fungerar inte om utdata är omdirigerad (t.ex. till en fil) – då hoppar vi över.
            }
        }

        // Väntar tills användaren trycker på en tangent, så att resultatet hinner läsas innan skärmen rensas.
        public static void WaitForKey()
        {
            Console.WriteLine();
            Console.Write("Tryck på valfri tangent för att fortsätta...");
            try
            {
                Console.ReadKey(intercept: true);
            }
            catch (InvalidOperationException)
            {
                // ReadKey fungerar inte om indata är omdirigerad – läs en rad i stället.
                Console.ReadLine();
            }
        }

        public static string ReadText(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            // Om indata tar slut (t.ex. Ctrl+Z / Ctrl+D) avslutas programmet snyggt
            if (input is null)
            {
                Console.WriteLine();
                Console.WriteLine("Inmatningen avslutades.");
                Environment.Exit(0);
            }

            return input.Trim();
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                string input = ReadText(prompt);
                if (int.TryParse(input, out int value))
                {
                    return value;
                }
                Console.WriteLine("Ogiltigt heltal, försök igen.");
            }
        }

        // Ersätter ReadPositiveDecimal. Läser ett tal och frågar igen direkt
        // om det är ogiltigt eller utanför tillåtet intervall.
        public static decimal ReadDecimalInRange(string prompt, decimal min, decimal max, string unit)
        {
            while (true)
            {
                string input = ReadText(prompt);

                // Godkänner både "2,5" och "2.5"
                bool ok = decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal value)
                       || decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value);

                if (!ok)
                {
                    Console.WriteLine("Ogiltigt tal, försök igen.");
                }
                else if (value < min || value > max)
                {
                    Console.WriteLine($"Värdet måste vara mellan {min} och {max} {unit}. Försök igen.");
                }
                else
                {
                    return value;
                }
            }
        }

        // Läser vikt inom de gränser som Weight-klassen bestämmer
        public static decimal ReadWeight()
        {
            return ReadDecimalInRange($"Vikt ({Weight.MinKg}–{Weight.MaxKg} kg): ", Weight.MinKg, Weight.MaxKg, "kg");
        }

        // Läser avstånd inom de gränser som Distance-klassen bestämmer
        public static decimal ReadDistance()
        {
            return ReadDecimalInRange($"Avstånd ({Distance.MinKm}–{Distance.MaxKm} km): ", Distance.MinKm, Distance.MaxKm, "km");
        }

        //  Läser in en frakttyp och frågar igen direkt om den inte finns.
        //  Användaren kan skriva antingen siffran eller namnet (t.ex. "2" eller "Express").
        //  Listan hämtas från fabriken, så en ny frakttyp dyker upp här automatiskt.
        public static string ReadShippingType()
        {
            IReadOnlyList<string> types = PackageTypeFactory.GetRegisteredTypeNames();

            Console.WriteLine("Frakttyper:");
            for (int i = 0; i < types.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {types[i]}");
            }

            while (true)
            {
                string input = ReadText("Frakttyp (siffra eller namn): ");

                // Siffra, t.ex. "2"
                if (int.TryParse(input, out int number) && number >= 1 && number <= types.Count)
                {
                    return types[number - 1];
                }

                // Namn, t.ex. "express" (skiftlägesokänsligt via fabriken)
                if (PackageTypeFactory.TryCreatePackageType(input, out _))
                {
                    return input;
                }

                Console.WriteLine($"Okänd frakttyp: '{input}'. Välj en siffra mellan 1 och {types.Count} eller skriv namnet.");
            }
        }

        public static Address ReadAddress(string label)
        {
            while (true)
            {
                string street = ReadText($"{label} – gatuadress: ");
                string city = ReadText($"{label} – ort: ");
                try
                {
                    return new Address(street, city);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{ex.Message} Försök igen.");
                }
            }
        }

        public static List<Station> ReadRoute()
        {
            string input = ReadText("Stationer på vägen, separerade med komma (t.ex. Borlänge, Gävle): ");

            return input
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(name => new Station(name))
                .ToList();
        }
    }
}

