using System;
using System.Collections.Generic;
//Ana Paola Morales Anaya
//Requerimiento 1. Construir un metodo para escribir en el archivo Lenguaje.cs identando el codigo
//                 "{" incrementa un tabulador, "}" decrementa un tabulador---listo
//Requerimiento 2. Declarar un atributo "primeraProduccion" de tipo string y actualizarlo con la 
//                 primera produccion de la gramatica---listo
//Requerimiento 3. La primera produccion es publica y el resto es privada---listo
//Requerimiento 4. El constructor lexico parametrico debe validar que la extensión del archivo a compilar
//sea .gen y sino levantar una exception---listo
//Requerimiento 5. Resolver la ambiguedad de st y snt
//Requerimiento 6. Agregar el parentesis izquierdoy el parentesis derecho escapado en la matriz de transiciones
//Requerimiento 7. Implementar el or y la cerradura epsilon
namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        int tab;
        string primeraProduccion;
        List<string> listaSNT;
        public Lenguaje(string nombre) : base(nombre)
        {
            listaSNT = new List<string>();
            tab = 0;
            primeraProduccion = "";
        }

        public Lenguaje()
        {
            listaSNT = new List<string>();
            tab = 0;
            primeraProduccion = "";
        }
        public void Dispose()
        {
            cerrar();
        }
        private void tabulador(string tabCodigo)
        {
            if (tabCodigo == "}")
            {
                tab--;
            }
            for (int i = 0; i < tab; i++)
            {
                lenguaje.Write("\t");
            }
            if (tabCodigo == "{")
            {
                tab++;
            }
            lenguaje.WriteLine(tabCodigo);
        }
        public void Gramatica()
        {
            Cabecera();
            primeraProduccion = getContenido();
            Programa(primeraProduccion);
            CabeceraLenguaje();
            ListaProducciones(true);
            tabulador("}");
            tabulador("}");
        }
        private bool esSNT(string contenido)
        {
            return listaSNT.Contains(contenido);
        }
        private void AgregarSNT(string contenido)
        {
            listaSNT.Add(contenido);
        }
        private void Programa(string ProduccionPrincipal)
        {
            programa.WriteLine("using System;");
            programa.WriteLine("using System.IO;");
            programa.WriteLine("using System.Collections.Generic;");
            programa.WriteLine();
            programa.WriteLine("namespace Generico");
            programa.WriteLine("{");
            programa.WriteLine("\tpublic class Programa");
            programa.WriteLine("\t{");
            programa.WriteLine("\t\tstatic void Main(string[] args)");
            programa.WriteLine("\t\t{");
            programa.WriteLine("\t\t\ttry");
            programa.WriteLine("\t\t\t{");
            programa.WriteLine("\t\t\t\tusing (Lenguaje a = new Lenguaje())");
            programa.WriteLine();
            programa.WriteLine("\t\t\t\t{");
            programa.WriteLine("\t\t\t\t\ta." + ProduccionPrincipal + "();");
            programa.WriteLine("\t\t\t\t}");
            programa.WriteLine("\t\t\t}");
            programa.WriteLine("\t\t\tcatch (Exception e)");
            programa.WriteLine("\t\t\t{");
            programa.WriteLine("\t\t\t\tConsole.WriteLine(e.Message);");
            programa.WriteLine("\t\t\t}");
            programa.WriteLine("\t\t}");
            programa.WriteLine("\t}");
            programa.WriteLine("}");

        }

        private void Cabecera()
        {
            match("Gramatica");
            match(":");
            match(Tipos.ST);
            match(Tipos.FinProduccion);

        }
        private void CabeceraLenguaje()
        {
            tabulador("using System;");
            tabulador("using System.Collections.Generic;");
            tabulador("namespace Generico");
            tabulador("{");
            tabulador("public class Lenguaje : Sintaxis, IDisposable");
            tabulador("{");
            tabulador("public Lenguaje(string nombre) : base(nombre)");
            tabulador("{");
            tabulador("}");
            tabulador("public Lenguaje()");
            tabulador("{");
            tabulador("}");
            tabulador("public void Dispose()");
            tabulador("{");
            tabulador("cerrar();");
            tabulador("}");
        }

        private void ListaProducciones(bool primero)
        {
            if (primero)
            {
                tabulador("public void " + getContenido() + "()");
            }
            else
            {
                tabulador("private void " + getContenido() + "()");
            }
            primero = false;
            tabulador("{");
            match(Tipos.ST);
            match(Tipos.Produce);
            Simbolos();
            match(Tipos.FinProduccion);
            tabulador("}");
            if (!FinArchivo()) //Para saltar a la siguiente linea
            {
                ListaProducciones(primero);
            }
        }

        private void Simbolos()
        {
            if (getContenido() == "\\(")
            {
                match("\\(");
                Simbolos();
                match("\\)");
            }
            else if (esTipo(getContenido()))
            {
                tabulador("match(Tipos." + getContenido() + ");");
                match(Tipos.ST);
            }
            else if (esSNT(getContenido()))
            {
                tabulador(getContenido() + "();");
                match(Tipos.ST);
            }
            else if (getClasificacion() == Tipos.ST)
            {
                tabulador("match(\"" + getContenido() + "\");");
                match(Tipos.ST);
            }
            if (getClasificacion() != Tipos.FinProduccion && getContenido() != "\\)")
            {
                Simbolos();
            }

        }
        private bool esTipo(string Clasificacion)
        {
            switch (Clasificacion)
            {
                case "Identificador":
                case "Numero":
                case "Caracter":
                case "Asignacion":
                case "Inicializacion":
                case "OperadorLogico":
                case "OperadorRelacional":
                case "OperadorTernario":
                case "OperadorTermino":
                case "OperadorFactor":
                case "IncrementoTermino":
                case "IncrementoFactor":
                case "FinSentencia":
                case "Cadena":
                case "TipoDato":
                case "Zona":
                case "Condicion":
                case "Ciclo":
                    return true;
            }
            return false;
        }

    }
}