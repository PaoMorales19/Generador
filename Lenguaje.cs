using System;
using System.Collections.Generic;
//Ana Paola Morales Anaya

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        public Lenguaje(string nombre) : base(nombre)
        {

        }

        public Lenguaje()
        {

        }
        public void Dispose()
        {
            cerrar();
        }
        public void Gramatica()
        {
            Cabecera();
            ListaProducciones();
        }
        private void Cabecera()
        {
            match("Gramatica");
            match(":");
            match(Tipos.SNT);
            match(Tipos.FinProduccion);

        }

        private void ListaProducciones()
        {
            match(Tipos.SNT);
            match(Tipos.Produce);
            match(Tipos.FinProduccion);
            if (!FinArchivo()) //Para saltar a la siguiente linea
            {
                ListaProducciones();
            }
        }

    }
}