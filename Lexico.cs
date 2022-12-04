using System.IO;
//Ana Paola Morales Anaya
namespace Generador
{
    public class Lexico : Token
    {
        protected StreamReader archivo;
        protected StreamWriter log;
        protected StreamWriter lenguaje;
        protected StreamWriter programa;
        public string direccion;
        const int F = -1;
        const int E = -2;
        protected int linea, posicion = 0;
        int[,] TRAND = new int[,]
        {
        //  WS - >  L  EDF La (  )  \
            
            {0, 1, 8, 3, 4, 8, 8, 8, 5}, //Estado 0
            {F, F, 2, F, F, F, F, F, F}, //Estado 1
            {F, F, F, F, F, F, F, F, F}, //Estado 2
            {F, F, F, 3, F, F, F, F, F}, //Estado 3
            {F, F, F, F, F, F, F, F, F}, //Estado 4
            {F, F, F, F, F, F, 6, 7, F}, //Estado 5
            {F, F, F, F, F, F, F, F, F}, //Estado 6
            {F, F, F, F, F, F, F, F, F}, //Estado 7
            {F, F, F, F, F, F, F, F, F}, //Estado 8

        };
        public Lexico()
        {
            //C:\\Users\\HOME\\Documents\\PAOLA TAREAS\\5TO SEMESTRE\\AUTOMATAS II\\UNIDAD 2\\Semantica\\prueba.cpp
            linea = 1;
            string nombre ="";
            DirectoryInfo di = new DirectoryInfo("C:\\Users\\user\\Downloads\\PAOLA TAREAS RESPALDOS\\Generador");
            FileInfo[] archivos = di.GetFiles("*.gram");

            foreach (FileInfo archivo in archivos)
            {
                nombre = archivo.Name;
            }
            string path= direccion = nombre;
            string pathSinExtension= Path.ChangeExtension(path, null);
            log = new StreamWriter(pathSinExtension + ".log");
            log.AutoFlush = true;

            lenguaje = new StreamWriter("C:\\Users\\user\\Downloads\\PAOLA TAREAS RESPALDOS\\Generico\\Lenguaje.cs");
            lenguaje.AutoFlush = true;

            programa = new StreamWriter("C:\\Users\\user\\Downloads\\PAOLA TAREAS RESPALDOS\\Generico\\Program.cs");
            programa.AutoFlush = true;

            log.WriteLine("Archivo:"+ pathSinExtension+".gram");
            log.WriteLine(DateTime.Now);

            bool existencia = File.Exists(path);
            if (existencia == true)
            {
                archivo = new StreamReader(path);
            }
            else
            {
                throw new Error("Error: El archivo c.gram no existe", log);
            }
        }
        public Lexico(string nombre)
        {
            linea = 1;
            string pathLog = Path.ChangeExtension(nombre, ".log");
            log = new StreamWriter(pathLog);
            log.AutoFlush = true;

            lenguaje = new StreamWriter("C:\\Users\\user\\Downloads\\PAOLA TAREAS RESPALDOS\\Generico\\Lenguaje.cs");
            lenguaje.AutoFlush = true;

            programa = new StreamWriter("C:\\Users\\user\\Downloads\\PAOLA TAREAS RESPALDOS\\Generico\\Program.cs");
            programa.AutoFlush = true;

            string ext = Path.GetExtension(nombre);
            if(ext != ".gram")
            {
                throw new Error("\nError: La extensión del archivo debe ser .gram", log);
            }
            log.WriteLine("Archivo: " + nombre);
            log.WriteLine(DateTime.Now);

            if (File.Exists(nombre))
            {
                archivo = new StreamReader(nombre);
            }
            else
            {
                throw new Error("Error: El archivo " + Path.GetFileName(nombre) + " no existe ", log);
            }
        }
        public void cerrar()
        {
            archivo.Close();
            log.Close();
            lenguaje.Close();
            programa.Close();
        }

        private void clasifica(int estado)
        {
            switch (estado)
            {
                case 1:
                    setClasificacion(Tipos.ST);
                    break;
                case 2:
                    setClasificacion(Tipos.Produce);
                    break;
                case 3:
                    setClasificacion(Tipos.ST);
                    break;
                case 4:
                    setClasificacion(Tipos.FinProduccion);
                    break;
                case 5:
                    setClasificacion(Tipos.ST);
                    break;
                case 6:
                    setClasificacion(Tipos.PIzq);
                    break;
                case 7:
                    setClasificacion(Tipos.PDer);
                    break;
                case 8:
                    setClasificacion(Tipos.ST);
                    break;
            }

        }
        private int columna(char c) //Ubicar el caracter en la columna de la matriz
        {
            if (c == 10)
            {
                return 4;
            }
            else if (char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if (c == '-')
            {
                return 1;
            }
            else if (c == '>')
            {
                return 2;
            }
            else if (char.IsLetter(c))
            {
                return 3;
            }
            else if(c =='\\')
            {
                return 8;
            }  
            else if(c == '(')
            {
                return 6;
            }
            else if(c == ')')
            {
                return 7;
            }
            return 5;
        }
        //WS,EF,EL,L, D, .,	E, +, -, =,	:, ;, &, |,	!, >, <, *,	%, /, ", ?,La, ', #
        public void NextToken()
        {
            string buffer = "";
            char c;
            int estado = 0;

            while (estado >= 0)
            {
                c = (char)archivo.Peek(); //Funcion de transicion
                estado = TRAND[estado, columna(c)];
                clasifica(estado);
                if (estado >= 0)
                {
                    archivo.Read();
                    posicion++;
                    if (c == '\n')
                    {
                        linea++;
                    }
                    if (estado > 0)
                    {
                        buffer += c;
                    }
                    else
                    {
                        buffer = "";
                    }
                }
            }
            setContenido(buffer);

            if (estado == E)
            {

                throw new Error("Error lexico: No definido en linea: " + linea, log);
            }
            if (!FinArchivo())
            {
                log.WriteLine(getContenido() + " " + getClasificacion());
            }
        }

        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}