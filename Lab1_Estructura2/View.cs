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
                    // Llamamos al método BuscarPorNombreDPI para encontrar el registro existente.
                    UsuarioModel usuarioExistente = arbol.BuscarPorNombreDPI(usuario.nombre, usuario.dpi);

                    if (usuarioExistente != null)
                    {
                        // Realizamos la actualización eliminando el registro existente y agregando el nuevo.
                        arbol.Eliminar(usuarioExistente.nombre, usuarioExistente.dpi);
                        arbol.Insertar(usuario);
                        //Console.WriteLine("Registro actualizado exitosamente.");
                    }
                    else
                    {
                        //Console.WriteLine("No se encontró un registro con el nombre y DPI proporcionados.");
                    }
                }
                else
                {
                    //Console.WriteLine("No se pudo actualizar el registro.");
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
                    // Llamamos al método Eliminar del árbol pasando el nombre y DPI del usuario.
                    arbol.Eliminar(usuario.nombre, usuario.dpi);
                }
                else {
                    //Console.WriteLine("No se pudo eliminar el registro");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar: " + ex.Message);
            }
        }
        static void Main(string[] args)
        {
            // Crea un árbol AVL
            Arbol arbol = new Arbol();

            // Lee el archivo CSV
            string csvFilePath = "D:\\Desktop\\2do ciclo 2023\\Estructura de datos II\\datos.txt";

            // Lee cada línea del archivo CSV
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

            while (true)
            {
                Console.Write("Ingrese un dpi para buscar (o 'salir' para salir): ");
                string dpi = Console.ReadLine();
                if (dpi.ToLower() == "salir")
                    break;

                List<UsuarioModel> resultados = arbol.BuscarPorDPI(dpi);
                if (resultados.Count == 0)
                {
                    Console.WriteLine("No se encontraron resultados para el dpi: " + dpi);
                }
                else
                {
                    Console.WriteLine("Registros asociados a " + dpi + ":");
                    foreach (var usuario in resultados)
                    {
                        Console.WriteLine($"Nombre: {usuario.nombre}, DPI: {usuario.dpi}, Fecha de Nacimiento: {usuario.nacimiento}, Dirección: {usuario.direccion}");
                    }
                }
            }
            Console.ReadKey();
        }

    }
}
