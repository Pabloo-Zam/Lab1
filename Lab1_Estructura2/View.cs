using Lab1_Estructura2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace Lab1_Estructura2
{
    public class View
    {
        public static Arbol arbol = new Arbol();
        public static List<UsuarioModel> listaUsuario = new List<UsuarioModel>();
        static void Main(string[] args)
        {
            string filePath = "D:\\Desktop\\2do ciclo 2023\\Estructura de datos II\\datos.json"; // Ruta al archivo JSON

            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    //string jsonData = File.ReadAllText(filePath);
                    // Deserializar el JSON a un objeto C#
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');

                        if (parts.Length == 2)
                        {
                            string action = parts[0].Trim();
                            string jsonData = parts[1].Trim();
                            switch (action)
                            {
                                case "INSERT":
                                    Insertar(jsonData);

                                    break;
                                case "PATCH":
                                    Actualizar(jsonData);


                                    break;
                                case "DELETE":
                                    Eliminar(jsonData);
                                    break;


                                default:
                                    Console.WriteLine($"Acción no válida: {action}");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Formato de línea incorrecto: {line}");
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo JSON: " + ex.Message);
            }
        }

        static void Insertar(string jsonData)
        {
            try
            {
                UsuarioModel userData = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                arbol.raiz = arbol.Insertar(arbol.raiz, userData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar: {ex.Message}");
            }
        }

        static void Actualizar(string jsonData)
        {
            try
            {
                UsuarioModel userData = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                arbol.raiz = arbol.Actualizar(arbol.raiz, userData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
            }
        }

        static void Eliminar(string jsonData) 
        {
            try
            {
                UsuarioModel userData = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                arbol.raiz = arbol.Eliminar(arbol.raiz, userData);
            }
            catch 
            { 
                
            }
        }
    }
}