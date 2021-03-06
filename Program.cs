using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
            if (command == "--hide" && text != null && soubor.Length > 0 && soubor.Contains(".png")) Console.WriteLine(Hide(text, soubor));
            else if (command == "--show" && text == null && soubor.Length > 0 && soubor.Contains(".png")) Console.WriteLine(Show(soubor));
            else if (command == "--help") Console.WriteLine(Help());
            else Console.WriteLine("Neplatný vstup!");
        }

        private static string Hide(string text, string soubor)
        {
            //tvorba názvu souboru
            string novySoubor = soubor.Split('.')[0] + "Encrypt" + ".png";

            //načtení obrázku jako bitmapy
            Bitmap bmp = null;
            try
            {
                 bmp = new Bitmap(soubor);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            //bložení dat do obrázku
            int x, y;
            x = y = 0;

            //vložení délky zprávy
            bmp.SetPixel(x, y, Color.FromArgb(
                    bmp.GetPixel(x, y).R,
                    bmp.GetPixel(x, y).G,
                    (byte)(text.Length)
                    ));

            //vložení textu
            for (x = 1; x < text.Length + 1; x++)
            {
                bmp.SetPixel(x, y, Color.FromArgb(
                    bmp.GetPixel(x, y).R,
                    bmp.GetPixel(x, y).G,
                    (byte)(text[x-1])
                    ));

                if (x == bmp.Width) { x = 0; y++; }
            }

            //vložení kontrolního součtu
            x++; if (x == bmp.Width) { x = 0; y++; }
            bmp.SetPixel(x, y, Color.FromArgb(
                    bmp.GetPixel(x, y).R,
                    bmp.GetPixel(x, y).G,
                    (byte)(char)(text.Length + 1)
                    ));

            //uložní nového souboru
            bmp.Save(novySoubor);

            //úspěšné vypsání a návrat
            return $"Úspěšně zašifrováno. Vytvořen nový soubor {novySoubor}";
        }

        private static string Show(string soubor)
        {
            //promněná pro zprávu
            string zprava = "";
            string navrat = null;

            //načtení a vytvoření bitmapy se zašifrovaným textem
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(soubor);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            //zjištění počtu znaků
            int delka = (int)bmp.GetPixel(0, 0).B;

            //načtení textu
            int x, y;
            for (y = 0; y < bmp.Height; y++)
            {
                for (x = 1; x < bmp.Width; x++)
                {
                    Color pixel = bmp.GetPixel(x, y);

                    zprava += (char)pixel.B;

                    if (zprava.Length == delka)
                    {
                        navrat = $@"Úspěšně dešifrováno. 
Zpráva: {zprava}";
                        return navrat;
                    }
                    #region pokus o kontrolní součet
                    //                    if (zprava.Length == delka+1)
                    //                    {                        
                    //                        return navrat = $@"Úspěšně dešifrováno. 
                    //Zpráva: {zprava.Remove(zprava.Length-1,1)}
                    //Kontrolní součet: {(byte)zprava.Last()-1}";

                    //                        //if ((int)bmp.GetPixel(x+1,y).B == delka +1)
                    //                        //{
                    //                        //    navrat += "\nZpráva je kompletní.";
                    //                        //    return navrat;
                    //                        //} 
                    #endregion
                
                }
            }

            return "Err";
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
                text = null;
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
