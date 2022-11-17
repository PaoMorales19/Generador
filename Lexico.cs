using System.IO;
//Ana Paola Morales Anaya
namespace Generador
{
    public class Lexico : Token
    {
        protected StreamReader archivo;
        protected StreamWriter log;
        const int F = -1;
        const int E = -2;
        protected int linea, posicion = 0;
        int[,] TRAND = new int[,]
        {
            {0, 1, 5, 3, 4, 5},
            {F, F, 2, F, F, F},
            {F, F, F, F, F, F},
            {F, F, F, 3, F, F},
            {F, F, F, F, F, F},
            {F, F, F, F, F, F},
            
        };
        public Lexico()
        {
            //C:\\Users\\HOME\\Documents\\PAOLA TAREAS\\5TO SEMESTRE\\AUTOMATAS II\\UNIDAD 2\\Semantica\\prueba.cpp
            linea = 1;
            string path = "c.gram";
            bool existencia = File.Exists(path);
            log = new StreamWriter("c.Log");

            log.WriteLine("Archiivo: c.gram");
            log.AutoFlush = true;
            if (existencia == true)
            {
                archivo = new StreamReader(path);
            }
            else
            {
                throw new Error("Error: El archivo prueba no existe", log);
            }
        }
        public Lexico(string nombre)
        {
            linea = 1;
            //log = new streamWriter(nombre.log)
            //Usar el objeto path

            string pathLog = Path.ChangeExtension(nombre, ".log");
            log = new StreamWriter(pathLog);
            log.AutoFlush = true;

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
        }

        private void clasifica(int estado)
        {
            switch(estado)
            {
                case 1:
                    setClasificacion(Tipos.ST);
                    break;
                case 2:
                    setClasificacion(Tipos.Produce);
                    break;
                case 3:
                    setClasificacion(Tipos.SNT);
                    break;
                case 4:
                    setClasificacion(Tipos.FinProduccion);
                    break;
                case 5:
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
            else if(char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if(c == '-')
            {
                return 1;
            }
            else if(c == '>')
            {
                return 2;
            }
            else if (char.IsLetter(c))
            {
                return 3;
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
        }

        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}