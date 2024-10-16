using System;
using System.Diagnostics.Metrics;

class Program
{
    static void Main()
    {
        // Opciók listája
        string[] menuOpcio = { "Rajzolás","Szerkesztés","Mentés","Kilépés" };
        int aktualisKivalasztas = 0;
        ConsoleKey billentyu;

        RajzoldMenu(menuOpcio, aktualisKivalasztas, null);
        bool enterLenyomva = false;

        do
        {
            if (!enterLenyomva)
            {
                RajzoldMenu(menuOpcio, aktualisKivalasztas, null);
            }
            else
            {
                RajzoldMenu(menuOpcio, aktualisKivalasztas, menuOpcio[aktualisKivalasztas]);
                enterLenyomva = false;
            }

            billentyu = Console.ReadKey(true).Key;

            switch (billentyu)
            {
                case ConsoleKey.UpArrow:
                    if (aktualisKivalasztas > 0)
                        aktualisKivalasztas--;
                    break;

                case ConsoleKey.DownArrow:
                    if (aktualisKivalasztas < menuOpcio.Length - 1)
                        aktualisKivalasztas++;
                    break;

                case ConsoleKey.Enter:
                    if (menuOpcio[aktualisKivalasztas] == "Kilépés")
                    {
                        return;
                    }
                    else if (menuOpcio[aktualisKivalasztas] == "Rajzolás")
                    {
                        Rajzolo_Resz();
                    }

                    else if (menuOpcio[aktualisKivalasztas] == "Szerkesztés")
                    {
                        Szerkesztes();
                    }

                    else if (menuOpcio[aktualisKivalasztas] == "Mentés")
                    {
                        Mentes_Resz();
                    }
                    RajzoldMenu(menuOpcio, aktualisKivalasztas, menuOpcio[aktualisKivalasztas]);
                    enterLenyomva = true;
                    break;
            }
        }
        while (billentyu != ConsoleKey.Escape);
    }

    static void RajzoldMenu(string[] opciok, int kivalasztottIndex, string kivalasztottOpcio)
    {
        Console.Clear();

        int konzolSzelesseg = Console.WindowWidth;
        int konzolMagassag = Console.WindowHeight;
        int szelesseg = 20;
        int belsoSzelesseg = szelesseg - 2;
        int menuMagassag = opciok.Length * 3;

        int felsoPozicio = (konzolMagassag / 2) - (menuMagassag / 2);
        int balPozicio = (konzolSzelesseg / 2) - (szelesseg / 2);

        for (int i = 0; i < opciok.Length; i++)
        {
            Console.SetCursorPosition(balPozicio, felsoPozicio + i * 3);
            Console.WriteLine("|" + new string('-', belsoSzelesseg) + "|");

        
            string szoveg = opciok[i].PadLeft((belsoSzelesseg / 2) + (opciok[i].Length / 2)).PadRight(belsoSzelesseg);
            Console.SetCursorPosition(balPozicio, felsoPozicio + 1 + i * 3);

            if (i == kivalasztottIndex)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.WriteLine("|" + szoveg + "|");
            Console.ResetColor();

            Console.SetCursorPosition(balPozicio, felsoPozicio + 2 + i * 3);
            Console.WriteLine("|" + new string('-', belsoSzelesseg) + "|");
        }
    }

    static void Rajzolo_Resz()
    {
        Console.Clear();
        int kurzorX = 0;
        int kurzorY = 0;
        ConsoleColor aktSzin = ConsoleColor.White;

        Dictionary<(int, int), string> szinezettPoziciok = new Dictionary<(int, int), string>(); //Console színe és a 2 koordináta mentése csak, ha átszínezne egy meglevo koord-ot


        Console.Clear();
        ConsoleKeyInfo gomb_lenyom;

        while (true)
        {
            kiIras();
            gomb_lenyom = Console.ReadKey(true);

            if (gomb_lenyom.Key == ConsoleKey.Escape)
            {
                Mentes(szinezettPoziciok);
                break;
            }
            KezeljeAGombNyom(gomb_lenyom);

        }

        void KezeljeAGombNyom(ConsoleKeyInfo gomb)
        {

            if (gomb_lenyom.Key == ConsoleKey.LeftArrow)
            {
                kurzorX = Math.Max(0, kurzorX - 1);
            }
            else if (gomb_lenyom.Key == ConsoleKey.LeftArrow)
            {
                kurzorX = Math.Max(0, kurzorX - 1);
            }
            else if (gomb_lenyom.Key == ConsoleKey.RightArrow)
            {
                kurzorX = Math.Min(Console.WindowWidth - 1, kurzorX + 1);
            }
            else if (gomb_lenyom.Key == ConsoleKey.UpArrow)
            {
                kurzorY = Math.Max(0, kurzorY - 1);
            }
            else if (gomb_lenyom.Key == ConsoleKey.DownArrow)
            {
                kurzorY = Math.Min(Console.WindowHeight - 1, kurzorY + 1);
            }
            else if (gomb_lenyom.Key == ConsoleKey.Spacebar)
            {
                rajzolas_a_kurzornal();
            }
            else
            {
                if (char.IsDigit(gomb_lenyom.KeyChar))
                {
                    SzinValtas(gomb_lenyom.KeyChar);
                }
            }
        }
        void rajzolas_a_kurzornal()
        {
            Console.SetCursorPosition(kurzorX, kurzorY);
            Console.BackgroundColor = aktSzin;
            Console.Write(" ");
            Console.ResetColor();


            szinezettPoziciok[(kurzorX, kurzorY)] = aktSzin.ToString();
        }
        void SzinValtas(char number)
        {
            int szinIndexe = int.Parse(number.ToString()) % 16;
            aktSzin = (ConsoleColor)szinIndexe;
        }
        void kiIras()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Pos: ({kurzorX}, {kurzorY}) | Color: {aktSzin}   ");
            Console.ResetColor();
        }
        void Mentes(Dictionary<(int,int),string> poziciok)
        {
            using (StreamWriter mentes = new StreamWriter("rajz.txt", true))
            {
                foreach (var poz in poziciok)
                {
                    mentes.WriteLine($"{poz.Value};{poz.Key.Item1};{poz.Key.Item2}");
                }
            } 
        }
    }
    static void Mentes_Resz()
    {
        Console.Clear();
        Console.Write("Add meg a mentés helyét: ");
        string menteshelye = Console.ReadLine();

        Console.Write("\nAdd meg a file nevét amin menteni akarod:");
        string mentesneve = Console.ReadLine();
        if (!string.IsNullOrEmpty(menteshelye))
        {
            string asd = menteshelye + $"\\{mentesneve}.txt";
            File.Copy("rajz.txt", asd, true);
            Console.WriteLine($"Mentés sikeres lett ide: ${asd}");
            File.Delete("rajz.txt");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    static void Szerkesztes()
    {
        Console.Clear();
        Console.Write("Add meg a szerkeszteni kívánt rajz fájl helyét: ");
        string szerkhelye = Console.ReadLine();

        if (!File.Exists(szerkhelye))
        {
            Console.WriteLine("A fájl nem található.");
            return;
        }

        Dictionary<(int, int), string> szinezettPoziciok = new Dictionary<(int, int), string>();

        using (StreamReader reader = new StreamReader(szerkhelye))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(';');
                if (parts.Length == 3)
                {
                    string szin = parts[0];
                    int x = int.Parse(parts[1]);
                    int y = int.Parse(parts[2]);
                    szinezettPoziciok[(x, y)] = szin;
                }
            }
        }
        Console.Clear() ;
        foreach (var poz in szinezettPoziciok)
        {
            ConsoleColor szin = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), poz.Value);
            Console.SetCursorPosition(poz.Key.Item1, poz.Key.Item2);
            Console.BackgroundColor = szin;
            Console.Write(" ");
        }
        Console.ResetColor();

        int kurzorX = 0;
        int kurzorY = 0;
        ConsoleColor aktSzin = ConsoleColor.White;
        ConsoleKeyInfo gomb_lenyom;

        while (true)
        {
            kiIras();
            gomb_lenyom = Console.ReadKey(true);

            if (gomb_lenyom.Key == ConsoleKey.Escape)
            {
                Mentes(szinezettPoziciok);
                break;
            }

            KezeljeAGombNyom(gomb_lenyom);
        }

        void KezeljeAGombNyom(ConsoleKeyInfo gomb)
        {
            if (gomb.Key == ConsoleKey.LeftArrow)
            {
                kurzorX = Math.Max(0, kurzorX - 1);
            }
            else if (gomb.Key == ConsoleKey.RightArrow)
            {
                kurzorX = Math.Min(Console.WindowWidth - 1, kurzorX + 1);
            }
            else if (gomb.Key == ConsoleKey.UpArrow)
            {
                kurzorY = Math.Max(0, kurzorY - 1);
            }
            else if (gomb.Key == ConsoleKey.DownArrow)
            {
                kurzorY = Math.Min(Console.WindowHeight - 1, kurzorY + 1);
            }
            else if (gomb.Key == ConsoleKey.Spacebar)
            {
                rajzolas_a_kurzornal();
            }
            else
            {
                if (char.IsDigit(gomb.KeyChar))
                {
                    SzinValtas(gomb.KeyChar);
                }
            }
        }

        void rajzolas_a_kurzornal()
        {
            Console.SetCursorPosition(kurzorX, kurzorY);
            Console.BackgroundColor = aktSzin;
            Console.Write(" ");
            Console.ResetColor();

            szinezettPoziciok[(kurzorX, kurzorY)] = aktSzin.ToString();
        }

        void SzinValtas(char number)
        {
            int szinIndexe = int.Parse(number.ToString()) % 16;
            aktSzin = (ConsoleColor)szinIndexe;
        }

        void kiIras()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Pos: ({kurzorX}, {kurzorY}) | Color: {aktSzin}   ");
            Console.ResetColor();
        }

        void Mentes(Dictionary<(int, int), string> poziciok)
        {
            using (StreamWriter mentes = new StreamWriter("rajz.txt", true))
            {
                foreach (var poz in poziciok)
                {
                    mentes.WriteLine($"{poz.Value};{poz.Key.Item1};{poz.Key.Item2}");
                }
            }
        }
    }
}
