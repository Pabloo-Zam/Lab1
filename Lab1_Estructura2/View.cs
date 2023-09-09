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

        static void InsertarRegistro(Arbol tree, string jsonData)
        {
            try
            {
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                tree.Insertar(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar: " + ex.Message);
            }
        }

        static void ActualizarRegistro(Arbol tree, string jsonData)
        {
            try
            {
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                tree.EliminarUsuarioPorNombre(usuario.nombre);
                tree.Insertar(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar: " + ex.Message);
            }
        }
        static void EliminarRegistro(Arbol tree, string jsonData)
        {
            try
            {
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(jsonData);
                tree.EliminarUsuarioPorNombre(usuario.nombre);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar: " + ex.Message);
            }
        }
        /*static void BuscarYMostrarRegistros(string nombre)
        {
            List<UsuarioModel> resultados = arbol.Buscar(nombre);
            if (resultados != null && resultados.Count > 0)
            {
                Console.WriteLine($"Registros encontrados para el nombre '{nombre}':");
                foreach (var resultado in resultados)
                {
                    Console.WriteLine($"Nombre: {resultado.nombre}, DPI: {resultado.dpi}, Fecha de Nacimiento: {resultado.nacimiento}, Dirección: {resultado.direccion}");
                }
            }
            else
            {
                Console.WriteLine($"No se encontraron registros para el nombre '{nombre}'.");
            }
        }*/
        static void Main(string[] args)
        {
            Arbol arbol = new Arbol();
            string[] lines = File.ReadAllLines("D:\\Desktop\\2do ciclo 2023\\Estructura de datos II\\datos.txt");
            // Ruta al archivo JSON
            foreach (string line in lines){
                string[] parts = line.Split(';');
                if (parts.Length != 2) { 
                    Console.WriteLine("Formato incorrecto en la línea: " + line);
                    continue;
                }
                     string action = parts[0].Trim();
                     string jsonData = parts[1].Trim();


                switch (action.ToUpper())
                {
                    case "INSERT":
                        InsertarRegistro(arbol, jsonData);
                        break;
                    case "PATCH":
                        ActualizarRegistro(arbol, jsonData);
                        break;
                    case "DELETE":
                        EliminarRegistro(arbol, jsonData);
                        break;
                    default:
                        Console.WriteLine("Acción no reconocida en la línea: " + line);
                        break;
                }
            }
            while (true)
            {
                Console.Write("Ingrese un nombre para buscar (o 'salir' para salir): ");
                string nombre = Console.ReadLine();
                if (nombre.ToLower() == "salir")
                    break;

                List<UsuarioModel> resultados = arbol.BuscarPorNombre(nombre);
                if (resultados.Count == 0)
                {
                    Console.WriteLine("No se encontraron resultados para el nombre: " + nombre);
                }
                else
                {
                    Console.WriteLine("Registros asociados a " + nombre + ":");
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
