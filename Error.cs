using System;
using System.IO;
//Ana Paola Morales Anaya
namespace Generador
{
    public class Error : Exception
    {
        public Error(string mensaje, StreamWriter log) : base(mensaje)
        {
            log.WriteLine(mensaje);
        }
    }
}