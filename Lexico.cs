using System.IO;
//Ana Paola Morales Anaya
namespace Generador
{
    public class Lexico : Token
    {
        protected StreamReader archivo;
        protected StreamWriter log;
        protected StreamWriter asm;
        const int F = -1;
        const int E = -2;
        protected int linea, posicion = 0;
        int[,] TRAND = new int[,]
        {
            
        };
        public Lexico()
        {
            //C:\\Users\\HOME\\Documents\\PAOLA TAREAS\\5TO SEMESTRE\\AUTOMATAS II\\UNIDAD 2\\Semantica\\prueba.cpp
            linea = 1;
            string path = "c.gram";
            bool existencia = File.Exists(path);
            log = new StreamWriter("c.Log");

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

            asm.WriteLine(";Archivo: " + nombre);
            asm.WriteLine(";Fecha: " + DateTime.Now);

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
            asm.Close();
        }

        private void clasifica(int estado)
        {
            
        }
        private int columna(char c)
        {
            return 0;
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