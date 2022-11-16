using System;
using System.Collections.Generic;
//Ana Paola Morales Anaya

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        
        public void Dispose()
        {
            cerrar();
        }
        
    }
}