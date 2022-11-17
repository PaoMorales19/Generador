using System;
using System.IO;
//Ana Paola Morales Anaya
namespace Generador
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using(Lenguaje a = new Lenguaje ("c2.gram"))
                {
                    a.Gramatica();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
