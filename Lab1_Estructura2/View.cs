﻿using Lab1_Estructura2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Microsoft.Win32;
using System.Security.Claims;
using System.Numerics;
using System.Security.Cryptography;


namespace Lab1_Estructura2
{
    public class CompressedCompany
    {
        public string CompressedData { get; set; }
    }
    

    public class View
    {

        //public static string jsonData;
        public static Arbol arbol = new Arbol();
        static List<UsuarioModel> lista = new List<UsuarioModel>();
        
        static void InsertarRegistro(Arbol arbol, string jsonData)
        {
            try
            {
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                arbol.Insertar(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar: " + ex.Message);
            }
        }

        static void ActualizarRegistro(Arbol arbol, string jsonData)
        {
            try
            {
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);

                if (usuario != null)
                {
                    // Llamamos al método ActualizarPorNombreDPI para realizar la actualización
                    arbol.BuscarPorNombreDPI(usuario);
                    //Console.WriteLine("Registro actualizado exitosamente.");
                }
                else
                {
                    Console.WriteLine("No se pudo actualizar el registro.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar: " + ex.Message);
            }
        }


        static void EliminarRegistro(Arbol arbol, string jsonData)
        {
            try
            {
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);

                if (usuario != null)
                {
                    // Llamamos al método EliminarPorNombreDPI para eliminar el registro existente
                    arbol.Eliminar(usuario.nombre, usuario.dpi);
                    //Console.WriteLine("Registro eliminado exitosamente.");
                }
                else
                {
                    Console.WriteLine("No se pudo eliminar el registro.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar: " + ex.Message);
            }
        }
        //FUNCION LZ77
        private static CompressedCompany ComprimirCompanies(List<string> companies)
        {
            // Convierte la lista de empresas en una cadena de texto
            string companiesTexto = string.Join(",", companies);

            // Aplica el algoritmo LZ77 para comprimir los datos
            string companiesComprimidas = LZ77.Comprimir(companiesTexto);

            // Crea un objeto CompressedCompany para almacenar los datos comprimidos
            CompressedCompany compressedCompany = new CompressedCompany
            {
                CompressedData = companiesComprimidas
            };

            return compressedCompany;
        }
        static void DecodificarCompanies(UsuarioModel usuario)
        {
            try
            {
                // Decodificar las companies de usuario
                List<string> companiesDecodificadas = new List<string>();

                foreach (string companyCodificada in usuario.companies)
                {
                    string companyDecodificada = LZ77.Descomprimir(companyCodificada);
                    companiesDecodificadas.Add(companyDecodificada);
                }

                // Imprimir las companies decodificadas
                Console.WriteLine("Companies decodificadas:");
                foreach (string company in companiesDecodificadas)
                {
                    Console.WriteLine(company);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al decodificar las companies: " + ex.Message);
            }
        }
        //CIFRADO
        static string CifrarContenido(string contenido)
        {
            // Longitud de la clave de cifrado (número de columnas)
            int claveLongitud = 5; // Puedes ajustar este valor según tus necesidades

            // Calcular el número de filas necesario para acomodar el contenido
            int filas = (int)Math.Ceiling((double)contenido.Length / claveLongitud);

            // Crear una matriz para almacenar el contenido cifrado
            char[,] matriz = new char[filas, claveLongitud];

            // Inicializar la matriz con espacios en blanco
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < claveLongitud; j++)
                {
                    matriz[i, j] = ' ';
                }
            }

            // Rellenar la matriz con el contenido original
            int indice = 0;
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < claveLongitud; j++)
                {
                    if (indice < contenido.Length)
                    {
                        matriz[i, j] = contenido[indice++];
                    }
                }
            }

            // Construir el contenido cifrado leyendo la matriz por columnas
            StringBuilder contenidoCifrado = new StringBuilder(contenido.Length);
            for (int j = 0; j < claveLongitud; j++)
            {
                for (int i = 0; i < filas; i++)
                {
                    contenidoCifrado.Append(matriz[i, j]);
                }
            }

            return contenidoCifrado.ToString();

        }

        //DESCIFRADO
        static string DescifrarContenido(string contenidoCifrado)
        {
            int claveLongitud = 5; // Asegúrate de que este valor coincida con la longitud de la clave utilizada en el cifrado

            int filas = (int)Math.Ceiling((double)contenidoCifrado.Length / claveLongitud);
            char[,] matriz = new char[filas, claveLongitud];

            // Construir la matriz a partir del contenido cifrado
            int indice = 0;
            for (int j = 0; j < claveLongitud; j++)
            {
                for (int i = 0; i < filas; i++)
                {
                    if (indice < contenidoCifrado.Length)
                    {
                        matriz[i, j] = contenidoCifrado[indice++];
                    }
                }
            }

            // Construir el contenido descifrado leyendo la matriz por filas
            StringBuilder contenidoDescifrado = new StringBuilder(contenidoCifrado.Length);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < claveLongitud; j++)
                {
                    contenidoDescifrado.Append(matriz[i, j]);
                }
            }

            return contenidoDescifrado.ToString().Trim(); // Puedes eliminar los espacios en blanco finales
        }
        static RandomNumberGenerator rng = RandomNumberGenerator.Create();

        //GENERAR NUMEROS PRIMOS
        static Random random = new Random();

        static bool IsPrime(BigInteger number, int witnessCount)
        {
            if (number <= 1)
                return false;
            if (number <= 3)
                return true;

            BigInteger d = number - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            for (int i = 0; i < witnessCount; i++)
            {
                BigInteger a = RandomBigInteger(2, number - 2);
                BigInteger x = BigInteger.ModPow(a, d, number);

                if (x == 1 || x == number - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == 1)
                        return false;
                    if (x == number - 1)
                        break;
                }

                if (x != number - 1)
                    return false;
            }

            return true;
        }

        static BigInteger RandomBigInteger(BigInteger min, BigInteger max)
        {
            byte[] data = new byte[max.ToByteArray().LongLength];
            BigInteger value;
            do
            {
                random.NextBytes(data);
                value = new BigInteger(data);
            } while (value < min || value >= max);
            return value;
        }

        static BigInteger GeneratePrime(int bitLength)
        {
            BigInteger prime;
            do
            {
                prime = RandomBigInteger(BigInteger.One << (bitLength - 1), BigInteger.One << bitLength);
            } while (!IsPrime(prime, 5));
            return prime;
        }
        static void Main(string[] args)
        {
            // Crea un árbol AVL
            Arbol arbol = new Arbol();
            List<string> companies = new List<string>();
            string carpeta = @"D:\Desktop\2do ciclo 2023\Estructura de datos II\inputs";
            string carpetaOutput = @"D:\Desktop\Output";
            string carpetaOutput2 = @"D:\Desktop\Output2";


            string csvFilePath = "D:\\Desktop\\2do ciclo 2023\\Estructura de datos II\\input2.csv";

            
            foreach (string line in File.ReadLines(csvFilePath))
            {
                string[] parts = line.Split(';');

                if (parts.Length == 2)
                {
                    string action = parts[0].Trim();
                    string jsonData = parts[1].Trim();

                    switch (action.ToUpper())
                    {
                        case "INSERT":
                            // Convierte el JSON en un objeto UsuarioModel
                            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                            InsertarRegistro(arbol,jsonData);
                            break;
                        case "PATCH":
                            usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                            ActualizarRegistro(arbol, jsonData);
                            break;
                        case "DELETE":
                            usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                            EliminarRegistro(arbol, jsonData);
                            break;
                        default:
                            Console.WriteLine("Acción no reconocida en la línea: " + line);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Formato incorrecto en la línea: " + line);
                }
            }
            CompressedCompany compressedCompany = View.ComprimirCompanies(companies);
            while (true)
            {
                Console.Write("Ingrese un dpi para buscar (o 'salir' para salir): ");
                string dpi = Console.ReadLine();
                if (dpi.ToLower() == "salir")
                    break;

                List<UsuarioModel> resultados = arbol.BuscarPorDPI(dpi);

            //IMPLEMENTACION CIFRAR
                try
                {
                    List<string> nombresArchivos = Directory.GetFiles(carpeta, "*.txt")

                    .ToList();

                    foreach (string rutaArchivo in nombresArchivos)
                    {
                        string nombreArchivoSinRuta = Path.GetFileName(rutaArchivo);
                        
                    }
                    List<string> nombresArchivosCoincidentes = nombresArchivos
                .Where(nombreArchivo => nombreArchivo.Contains("-" + dpi + "-"))
                .ToList();

                    if (nombresArchivosCoincidentes.Count > 0)
                    {
                        Console.WriteLine("Archivos asociados al DPI " + dpi + ":");
                        foreach (string nombreArchivo in nombresArchivosCoincidentes)
                        {
                            Console.WriteLine(nombreArchivo);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontraron archivos asociados al DPI " + dpi + ".");
                    }


                    //CIFRAR
                    if (!Directory.Exists(carpetaOutput))
                    {
                        Directory.CreateDirectory(carpetaOutput);
                    }

                    foreach (string nombreArchivo in nombresArchivos)
                    {
                        string nombreArchivoSinRuta = Path.GetFileName(nombreArchivo);

                        if (nombreArchivoSinRuta.Contains("-" + dpi + "-"))
                        {
                            string contenidoOriginal = File.ReadAllText(nombreArchivo);

                            // Cifrar el contenido (implementa tu algoritmo de cifrado)
                            string contenidoCifrado = CifrarContenido(contenidoOriginal);

                            // Crear el nuevo nombre del archivo cifrado
                            string nuevoNombreArchivoCifrado = nombreArchivoSinRuta.Replace("REC-", "CIF-");
                            string rutaArchivoCifrado = Path.Combine(carpetaOutput, nuevoNombreArchivoCifrado);

                            // Guardar el contenido cifrado en el nuevo archivo
                            File.WriteAllText(rutaArchivoCifrado, contenidoCifrado);

                            Console.WriteLine("Archivo cifrado y guardado como: " + nuevoNombreArchivoCifrado);
                        }
                    }

                    Console.WriteLine("Proceso de cifrado completo.");
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("La carpeta no existe.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                //IMPLEMENTACION DESCIFRAR
                try
                {
                    List<string> nombresArchivosCifrados = Directory.GetFiles(carpetaOutput, "*CIF*.txt")
                        .ToList();

                    foreach (string nombreArchivoCifrado in nombresArchivosCifrados)
                    {
                        string nombreArchivoSinRuta = Path.GetFileName(nombreArchivoCifrado);

                        if (nombreArchivoSinRuta.Contains("-" + dpi + "-"))
                        {
                            string contenidoCifrado = File.ReadAllText(nombreArchivoCifrado);

                            // Descifrar el contenido (implementa tu algoritmo de descifrado)
                            string contenidoDescifrado = DescifrarContenido(contenidoCifrado);

                            // Crear el nuevo nombre del archivo descifrado
                            string nuevoNombreArchivoDescifrado = nombreArchivoSinRuta.Replace("CIF-", "DESCIF-");
                            string rutaArchivoDescifrado = Path.Combine(carpetaOutput2, nuevoNombreArchivoDescifrado);

                            // Guardar el contenido descifrado en el nuevo archivo
                            File.WriteAllText(rutaArchivoDescifrado, contenidoDescifrado);

                            Console.WriteLine("Archivo descifrado y guardado como: " + nuevoNombreArchivoDescifrado);
                        }
                    }

                    Console.WriteLine("Proceso de descifrado completo.");
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("La carpeta no existe.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                //
                if (resultados.Count == 0)
                {
                    Console.WriteLine("No se encontraron resultados para el dpi: " + dpi);
                }
                else
                {
                    Console.WriteLine("Que desea hacer?");
                    Console.WriteLine("1.Codificacion de companies");
                    Console.WriteLine("2.Decodificacion de companies");
                    Console.WriteLine("3.Cifrar cartas");
                    Console.WriteLine("4.Descifrar cartas");
                    int action = Convert.ToInt32(Console.ReadLine());
                    if (action == 1)
                    {
                        Console.WriteLine("Registros asociados a " + dpi + ":");
                        foreach (var usuario in resultados)
                        {
                            Console.WriteLine($"Nombre: {usuario.nombre}, DPI: {usuario.dpi}, Fecha de Nacimiento: {usuario.nacimiento}, Dirección: {usuario.direccion}");
                            if (usuario.companies != null && usuario.companies.Any())
                            {
                                Console.WriteLine("Companies Codificadas:");
                                CompressedCompany compressedCompanies = ComprimirCompanies(usuario.companies);
                                Console.WriteLine(compressedCompanies.CompressedData);
                            }
                        }
                    }
                    else if (action == 2)
                    {
                        foreach (var usuario in resultados)
                        {
                            Console.WriteLine($"Nombre: {usuario.nombre}, DPI: {usuario.dpi}, Fecha de Nacimiento: {usuario.nacimiento}, Dirección: {usuario.direccion}");

                            // Decodificar y mostrar las companies
                            DecodificarCompanies(usuario);
                        }
                    }
                    else if (action == 3)
                    {
                        string[] nombresArchivosCifrados = Directory.GetFiles(carpetaOutput, "CIF-*.txt");

                        foreach (string rutaArchivoCifrado in nombresArchivosCifrados)
                        {
                            string nombreArchivoSinRuta = Path.GetFileName(rutaArchivoCifrado);

                            if (nombreArchivoSinRuta.Contains("-" + dpi + "-"))
                            {
                                string contenidoCifrado = File.ReadAllText(rutaArchivoCifrado);
                                Console.WriteLine("Contenido del archivo cifrado " + nombreArchivoSinRuta + ":");
                                Console.WriteLine(contenidoCifrado);
                                Console.WriteLine("---------------------------------------------------------------");
                            }
                        }
                    }
                    else if (action == 4)
                    {
                        string[] nombresArchivosDescifrados = Directory.GetFiles(carpetaOutput2, "DESCIF-*.txt");

                        foreach (string rutaArchivoDescifrado in nombresArchivosDescifrados)
                        {
                            string nombreArchivoSinRuta = Path.GetFileName(rutaArchivoDescifrado);

                            if (nombreArchivoSinRuta.Contains("-" + dpi + "-"))
                            {
                                string contenidoDescifrado = File.ReadAllText(rutaArchivoDescifrado);

                                Console.WriteLine("Contenido del archivo descifrado " + nombreArchivoSinRuta + ":");
                                Console.WriteLine(contenidoDescifrado);
                                Console.WriteLine("---------------------------------------------------------------");
                            }
                        }
                    }
                    else {
                        Console.WriteLine("Accion lo valida");
                    }
                }
                Console.WriteLine("-------------------------------------------------------------------------------");
            }
            Console.ReadKey();
        }

    }
}
