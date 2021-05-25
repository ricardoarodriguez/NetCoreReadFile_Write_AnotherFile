using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
namespace Read_Write_File_Console_Aplicattion
{
        class Program
    {
        static void Main(string[] args)
        {
            string inicioAplicacion = "***** Aplicación Iniciada *****";
            Console.WriteLine(inicioAplicacion);                                
            leerDocumento dat = new leerDocumento();                                    /* Instancio la clase leerDocumento */
            string[] InformacionCargada = dat.ReadFile();                               /* guardo información que me retorna el método*/
            // Event handler
            LogData ld = new LogData();
            // Logger
            Logger lo = new Logger();
            // Subscribe to event
            ld.MyEvent += lo.OnMyEvent;
            // Thread loop
            int cnt = 1;
            while (cnt < 5)
            {
                Thread t = new Thread(() => RunMe(cnt, ld, InformacionCargada));        /* Se crean las Instancias requeridas para el proceso */
                t.Start();
                cnt++;
            }
            Console.WriteLine("While end");
        }

        // Thread worker
        public static void RunMe(int cnt, LogData ld, string[] InfoCargada)
        {
            int nr = 0;
            while (true)
            {
                if (cnt == 1)
                {
                    PalabrasqueContienenN pl = new PalabrasqueContienenN();
                    Console.WriteLine(pl.calcularPalabras_n(InfoCargada));
                    // Add user and fire event
                    ld.AddToLog(new User() { Name = "Log to file Thread 1 : "+ pl.calcularPalabras_n(InfoCargada) });
                    Thread.Sleep(1);
                }
                else if (cnt == 2)
                {
                    Numero_Oraciones no = new Numero_Oraciones();
                    Console.WriteLine(no.calcularNumeroOraciones(InfoCargada));
                    ld.AddToLog(new User() { Name = "Log to file Thread 2 : " + no.calcularNumeroOraciones(InfoCargada) });
                    Thread.Sleep(1);
                }
                else if (cnt == 3)
                {
                    Numero_Parrafos parag = new Numero_Parrafos();
                    Console.WriteLine(parag.calcularParrafos(InfoCargada));
                    ld.AddToLog(new User() { Name = "Log to file Thread 3 : " + parag.calcularParrafos(InfoCargada) });
                    Thread.Sleep(1);
                }
                else {
                    PalabrasDiferentes_N pal = new PalabrasDiferentes_N();
                    Console.WriteLine(pal.calcularAlfanumericos(InfoCargada));
                    ld.AddToLog(new User() { Name = "Log to file Thread 4 : " + pal.calcularAlfanumericos(InfoCargada) });
                }
                nr++;
            }
        }
        }

        /****** Lectura del Documento ***************/        
        class leerDocumento
        {
            public string[] ReadFile()                                                                              /* Creo el Método ReadFile a partir del cual es posible acceder de manera publica con el fin de que se lleve a cabo lea la información y esta sea almacenada en una variable */
            {    
                string path = ("C:\\Users\\richa\\Desktop\\Proyectos\\NetCore5\\Don_Quijote_de_la_mancha.txt");     /* Indico la Ruta en mi equipo donde es posible acceder para leer el archivo *///Don_Quijote_de_la_Mancha.txt
                FileStream fileStream = new FileStream(path, FileMode.Open);                                        /* Instancio la clase FileStream de la libreria System.IO con el fin de proveer un transmisor para un archivo */                                                                     
                using (StreamReader reader = new StreamReader(fileStream))                                          /* Instancio la clase StreamReader con el fin de hacer uso del transmisor con los atributos fijados*/
                {
                    string[] words = new string[586];                                                               /* Defino una variable tipo array con su longuitud con el fin de que me guarde la Información que me comenzará a leer la clase StreamReader */
                    for (int j = 0; j <= reader.BaseStream.Length - 1; j++)
                    {
                        string line = reader.ReadLine();                                                            /* Por medio de este método leo la Información y la almaceno en mi variable line */       
                        if (line != null && line.Length == 0)                                                       /* Si hay un nuevo parrafo  encuentra un parrafo realiza un salto de linea*/
                        Console.WriteLine("\n");
                        else if (line == null)
                            break;                                                                                  /* Si llega al final del documento finaliza el proceso */
                        else
                            words[j] = line;                                                                        /* voy almacenando la información cargada en la variable */
                    }
                    return words;
                }
            }
        }
        
        /************** Punto 1 ********/
        class PalabrasqueContienenN
        {
            public int calcularPalabras_n(string[] obj)
            {
                int contador_Palabras_n = 0;                                        // Se Inicializa la variable que será retornada                                         
                for (int j = 0; j <= obj.Length - 1; j++)                           
                {                                                                   
                    string line = obj[j];                                           // se captura la fila en la variable 
                    if (line != null)                                               // se valida que no esté vacia o sea
                    {                                                               // se toma la fila y se realiza partición por espacio
                        string[] words = line.Split(' ');                           // inicializo mi variable que ayudará a iterar mi ciclo while
                        int i = 0;                                                   
                        while (i < words.Length)                                    
                        {                                                           
                            int contador = 0;                                       // creo un contador para realizar validar del ultimo caracter en la pila
                            foreach (char c in words[i])                            
                            {                                                       
                                contador++;                                         // lo incremento con el fin de llegar a la ultima ubicación de la palabra
                                if (contador == words[i].Length && c == 'n')        // comparo el contador  y el tamaño de la palabra y valido que esta sea n
                                    contador_Palabras_n++;                          // aumento el contador 'Contador_Palabras_n'
                            }                                                       
                            i++;                                                    // aumento el contador auxiliar del ciclo while con el fin de que itere
                        }
                    }
                }
                return contador_Palabras_n;
            }
        }

        /************** Punto 2 ********/
        class Numero_Oraciones
        {
        public int calcularNumeroOraciones(string[] obj)
        {
            int contadorOraciones = 0;                                                                      // Inicializo la variable que posteriormente será retornada
            for (int j = 0; j <= obj.Length - 1; j++)                                                       
            {                                                                                               
                string line = obj[j];                                                                       // Por cada iteraciòn capturo la informacion de la fila en la varible
                if (line != null)                                                                           // si Line trae algo ingresa de lo contrario continua iterando
                {                                                                                           
                    string[] sentencs = line.Split('.');                                                    // Defino la variable sentencs tipo array y me paticiona las oraciones por un punto
                    string[] words = line.Split(' ');                                                       // Defino la variable words para posteriormente realizar la validación de la cantidad de palabras
                    bool contador = true;                                                                   // defino el controlador del ciclo while tipo bool
                                                                                                            
                    int i = 0;                                                                              // inicializo el auxiliar del ciclo for
                    while (contador)                                                                        
                    {                                                                                       
                        if (sentencs.Length == i)                                                           // si completo todas las iteraciones y el axiliar es igual al tamaño del array 
                        {                                                                                   
                            contador = false;                                                               //entonces se cieera el ciclo
                        }                                                                                   
                        else if (sentencs != null && words.Length > 15 && i < sentencs.Length-1)            // se valida que sentencs tenga informaciòn y se valida la cantidad de palabras y que aun el iterador 
                        {                                                                                   // no llegue a al final del array
                            contadorOraciones++;                                                            // incremento el contador de las oraciones para calcular el dato
                            i++;                                                                            // incremento el auxiliar para que continue iterando
                        }                                                                                   
                        else                                                                                
                            i++;                                                                            
                    }                                                                                       
                                                                                                            
                }                                                                                           
                                                                                                            
            }
            Console.WriteLine(" *** Theard 2 Número de Oraciones : "+ contadorOraciones );
            return contadorOraciones;                                                                       // retorno el calculo de la operación
        }
    }

        /************** Punto 3 ********/
        class Numero_Parrafos
        {
        public int calcularParrafos(string[] obj)                                   // Declaro metodo 
        {                                                                           
            int contadorParrafos = 0;                                               // Inicializo variable que proximamente estaría retornando
            for (int j = 0; j <= obj.Length - 1; j++)                               
            {                                                                       
                string line = obj[j];                                               // almaceno información fila a fila de lo que contiene el objeto
                if (line != null)                                                   // si line contiene información ejecuta internamente el código
                {                                                                   
                    string[] paragraf = line.Split('\n');                           // El texto leido va a ser separado por parrafos
                    bool contador = true;                                           // Declaro valor a contador de tipo bool
                                                                                    
                    int i = 0;                                                      // Inicializo el auxiliar del Ciclo While
                    while (contador)                                                
                    {                                                               
                        if (paragraf.Length == i)                                   // Si las Iteraciones han llegado al final del Objeto 
                        {                                                            
                            contador = false;                                       // cambio estado con el fin de que no vuelva a Iterar
                        }                                                           
                        else if (paragraf != null                                   // Si trae algo paragraf entonces va a incrementar el contador ...
                        && i < paragraf.Length)                                     
                        {                                                           
                            contadorParrafos++;                                     
                            i++;                                                    // y la variable auxiliar
                        }                                                           
                        else                                                        
                            i++;                                                    
                        }                                                           
                                                                                    
                    }                                                               
                                                                                    
                }
                Console.WriteLine(" *** Theard 3 Número de Parrafos : " + contadorParrafos );
                return contadorParrafos;                                        // retorno el resultado de la operación
            }
        }

        /************** Punto 4 ********/
        class PalabrasDiferentes_N {                                            
            public int calcularAlfanumericos(string[] obj)                      // Creo metodo de la clase 
            {                                                                    
                int contador_alfanumerico_No_nN = 0;                            // Inicializo la variable que estaré retornando proximamente
                for (int j = 0; j <= obj.Length - 1; j++)                       
                {                                                               
                    string line = obj[j];                                       // Guardo la iteración fila a fila  
                    if (line != null)                                           // si la fila trae información comienza flujo
                    {                                                           
                        string[] words = line.Split(' ');                       // las palabras de la fila las separo por espacio
                        int i = 0;                                              // inicializo el auxiliar del ciclo while
                        while (i < words.Length)                                
                        {                                                       //
                            foreach (char c in words[i]) {                      // Comienzo a iterar cada letra de cada palabra y confirmo 
                                if (                                            // que no contengan la N, n, ., , ' ' ó - 
                                    (c == 'N' || c == 'n' || c == '.' ||        
                                    c == ',' || c == ' ' || c == '-')           
                                    == false)                                   
                                    contador_alfanumerico_No_nN++;              // Incremento el valor de la variable que estaría retornando
                            }                                                   
                            i++;                                                // Incremento el valor del auxiliar del Ciclo For
                        }                                                       
                    }                                                           
                }
            Console.WriteLine(" *** Theard 3 Número de Parrafos : " + contador_alfanumerico_No_nN);
            return contador_alfanumerico_No_nN;                             // retorno el valor de la variable al proceso
            }
        }

        class LogData
        {
            public delegate void MyEventHandler(object o, User u);
            public event MyEventHandler MyEvent;

            protected virtual void OnEvent(User u)
            {
                if (MyEvent != null)
                {
                    MyEvent(this, u);
                }

            }

            // Wywołaj
            public void AddToLog(User u)
            {
                Console.WriteLine("Add to log.");

                // Odpal event
                OnEvent(u);

                Console.WriteLine("Added.");
            }
        }

        class User
        {
            public string Name = "";
            public string Email = "";
        }

        class Logger
        {
            // Catch event
            public void OnMyEvent(object o, User u)
            {
                try
                {
                    Console.WriteLine("Added to file log! " + u.Name + " " + u.Email);
                    File.AppendAllText("C:\\Users\\richa\\Desktop\\Proyectos\\NetCore5\\FileGenerated.txt", "Added to file log! " + u.Name + " " + u.Email + "\r\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error file log " + e);
                }
            }
        }
    }


