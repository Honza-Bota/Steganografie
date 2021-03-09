using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stega
{
    class Program
    {
        static void Main(string[] args)
        {
            //zadané hodnoty
            string command, text, soubor;
            bool correctInput;

            //rozdělení hodnot z pole
            correctInput = SplitInput(args, out command, out text, out soubor);

            //rozhodnutí o volání metody
            if (!correctInput) Console.WriteLine("Nelze spustit program bez vstupních kritérií.");
            if (command == "--hide" && text != "" && soubor.Length > 0 && soubor.Contains(".png")) Console.WriteLine(Hide(text, soubor));
            else if (command == "--show" && text == "" && soubor.Length > 0 && soubor.Contains(".png")) Console.WriteLine(Show(soubor));
            else if (command == "--help") Console.WriteLine(Help());
            else Console.WriteLine("Neplatný vstup!");
        }

        private static string Hide(string text, string soubor)
        {
            return "Úspěšně zašifrováno.";
        }

        private static string Show(string soubor)
        {
            return "Úspěšně dešifrováno.";
        }

        private static string Help()
        {
            return $@"Nápověda:
~ šifrování do obrázku:  --hide ""Text"" ""obrázek.png""
~ dešifrování z obrázku: --show ""obrázek.png""
~ nápověda: --help";
        }

        private static bool SplitInput(string[] data, out string command, out string text, out string soubor)
        {
            //ohlídání prázdného vstupu
            if (data.Length != 0)
            {
                //přiřazení zadaného příkazu
                command = data[0];

                //pokud je zvolena možnost hide, načte text
                text = "";
                if (data.Length > 2 && command == "--hide")
                {
                    for (int i = 1; i < data.Length - 1; i++)
                    {
                        text += data[i];
                    } 
                }

                //přiřazení názvu souboru
                soubor = data[data.Length - 1];

                //vrácení inforamce o korektním vstupu
                return true;
            }

            //špatný/prázdný command >> vrátí chybu
            command = text = soubor = "";
            return false;
        }
    }
}
