using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab1_Estructura2.Model
{
    public class CompressedCompany
    {
        public string CompressedData { get; set; }
    }
    public class Arbol
    {
        public Node raiz;
        public int Altura(Node node)
        {
            if (node == null)
                return 0;
            return node.altura;
        }

        public int FactorBalance(Node node)
        {
            if (node == null)
                return 0;
            return Altura(node.iz) - Altura(node.der);
        }
        //ROTACION DERECHA
        private Node RotarDerecha(Node node)
        {
            Node Nleft = node.iz;
            Node NRight = Nleft.der;

            Nleft.der = node;
            node.iz = NRight;

            node.altura = Math.Max(Altura(node.iz), Altura(node.der)) + 1;
            Nleft.altura = Math.Max(Altura(Nleft.iz), Altura(Nleft.der)) + 1;

            return Nleft;
        }
        //ROTACION IZQUIERDA
        private Node RotarIzquierda(Node node)
        {
            Node NRight = node.der;
            Node Nleft = NRight.iz;

            NRight.iz = node;
            node.der = Nleft;

            node.altura = Math.Max(Altura(node.iz), Altura(node.der)) + 1;
            NRight.altura = Math.Max(Altura(NRight.iz), Altura(NRight.der)) + 1;

            return NRight;
        }
        private Node EncontrarMinimo(Node node)
        {
            Node actual = node;
            while (actual.iz != null)
                actual = actual.iz;
            return actual;
        }
        //FUNCION INSERTAR
        public void Insertar(UsuarioModel dato)
        {
            if (dato != null && dato.nombre != null)
            {
                //Console.WriteLine("Insertando usuario: " + usuario.nombre);
                raiz = Insertar(raiz,dato);
            }
            else
            {
                //Console.WriteLine("NO se inserto el dato");
            }
            
        }

        private Node Insertar(Node node, UsuarioModel usuario)
        {
            if (node == null)
            {
                return new Node(usuario);
            }

            int comparacion = usuario.CompareTo(node.dato);

            if (comparacion < 0)
            {
                node.iz = Insertar(node.iz, usuario);
            }
            else if (comparacion > 0)
            {
                node.der = Insertar(node.der, usuario);
            }
            else
            {
                // Valor duplicado, si es necesario, maneja la lógica aquí
                node.lista.Add(usuario);
            }

            node.altura = 1 + Math.Max(Altura(node.iz), Altura(node.der));

            int balance = FactorBalance(node);

            // Casos de rotación
            if (balance > 1)
            {
                if (usuario.CompareTo(node.iz.dato) < 0)
                {
                    return RotarDerecha(node);
                }
                else
                {
                    node.iz = RotarIzquierda(node.iz);
                    return RotarDerecha(node);
                }
            }
            if (balance < -1)
            {
                if (usuario.CompareTo(node.der.dato) > 0)
                {
                    return RotarIzquierda(node);
                }
                else
                {
                    node.der = RotarDerecha(node.der);
                    return RotarIzquierda(node);
                }
            }

            return node;
        }

        //FUNCION ELIMINAR
        public void Eliminar(string nombre, string dpi)
        {
            raiz = Eliminar(raiz, nombre, dpi);
        }

        private Node Eliminar(Node node, string nombre, string dpi)
        {
            if (node == null)
            {
                // El nodo no existe, no hacemos nada.
                return node;
            }

            // Comparamos el nombre actual con el nombre que buscamos.
            int comparacionNombre = nombre.CompareTo(node.lista[0].nombre);
            int comparacionDPI = dpi.CompareTo(node.lista[0].dpi);

            // Si el nombre o el DPI buscados son menores, buscamos en el subárbol izquierdo.
            if (comparacionNombre < 0 || (comparacionNombre == 0 && comparacionDPI < 0))
            {
                node.iz = Eliminar(node.iz, nombre, dpi);
            }
            // Si el nombre o el DPI buscados son mayores, buscamos en el subárbol derecho.
            else if (comparacionNombre > 0 || (comparacionNombre == 0 && comparacionDPI > 0))
            {
                node.der = Eliminar(node.der, nombre, dpi);
            }
            // Si encontramos el nombre y el DPI, eliminamos el nodo.
            else
            {
                // Si hay más de un usuario en la lista, simplemente lo removemos.
                if (node.lista.Count > 1)
                {
                    node.lista.RemoveAt(0);
                }
                else
                {
                    // Si solo hay un usuario con ese nombre y DPI, eliminamos el nodo completo.

                    // Caso 1: Sin hijos o solo un hijo.
                    if (node.iz == null)
                    {
                        return node.der;
                    }
                    else if (node.der == null)
                    {
                        return node.iz;
                    }

                    // Caso 2: Nodo con dos hijos, obtenemos el sucesor inorden (nodo más pequeño en el subárbol derecho).
                    Node sucesor = EncontrarMinimo(node.der);

                    // Copiamos los datos del sucesor al nodo actual.
                    node.lista[0] = sucesor.lista[0];

                    // Eliminamos el sucesor.
                    node.der = Eliminar(node.der, sucesor.lista[0].nombre, sucesor.lista[0].dpi);
                }
            }

            // Actualizamos la altura del nodo actual.
            node.altura = 1 + Math.Max(Altura(node.iz), Altura(node.der));

            // Calculamos el factor de equilibrio.
            int balance = FactorBalance(node);

            // Realizamos las rotaciones si es necesario.
            if (balance > 1)
            {
                if (nombre.CompareTo(node.iz.lista[0].nombre) < 0 || (nombre.CompareTo(node.iz.lista[0].nombre) == 0 && dpi.CompareTo(node.iz.lista[0].dpi) < 0))
                {
                    return RotarDerecha(node);
                }
                else
                {
                    node.iz = RotarIzquierda(node.iz);
                    return RotarDerecha(node);
                }
            }
            if (balance < -1)
            {
                if (nombre.CompareTo(node.der.lista[0].nombre) > 0 || (nombre.CompareTo(node.der.lista[0].nombre) == 0 && dpi.CompareTo(node.der.lista[0].dpi) > 0))
                {
                    return RotarIzquierda(node);
                }
                else
                {
                    node.der = RotarDerecha(node.der);
                    return RotarIzquierda(node);
                }
            }

            return node;
        }




        //FUNCION ACTUALIZAR 
        public void Actualizar(UsuarioModel dato, UsuarioModel nuevoDato)
        {
            raiz = Actualizar(dato, nuevoDato, raiz);
        }

        private Node Actualizar(UsuarioModel dato, UsuarioModel nuevoDato, Node node)
        {
            if (node == null)
                return node;

            int comparacion = dato.CompareTo(node.dato);

            if (comparacion < 0)
                node.iz = Actualizar(dato, nuevoDato, node.iz);
            else if (comparacion > 0)
                node.der = Actualizar(dato, nuevoDato, node.der);
            else
            {
                // Se encontró el usuario, actualiza la lista de usuarios
                node.lista.Clear();
                node.lista.Add(nuevoDato);
            }

            // Actualiza la altura del nodo
            node.altura = 1 + Math.Max(Altura(node.iz), Altura(node.der));

            // Calcula el factor de equilibrio
            int balance = FactorBalance(node);

            // Casos de rotación si es necesario
            if (balance > 1)
            {
                if (comparacion < 0)
                {
                    return RotarDerecha(node);
                }
                else
                {
                    node.iz = RotarIzquierda(node.iz);
                    return RotarDerecha(node);
                }
            }
            if (balance < -1)
            {
                if (comparacion > 0)
                {
                    return RotarIzquierda(node);
                }
                else
                {
                    node.der = RotarDerecha(node.der);
                    return RotarIzquierda(node);
                }
            }

            return node;
        }

        //BUSQUEDA POR NOMBRE
        public List<UsuarioModel> BuscarPorNombre(string nombre)
        {
            List<UsuarioModel> resultados = new List<UsuarioModel>();
            BuscarPorNombre(raiz, nombre, resultados);
            return resultados;
        }

        private void BuscarPorNombre(Node node, string nombre, List<UsuarioModel> resultados)
        {
            if (node != null)
            {
                // Busca el nombre en la lista de usuarios en este nodo
                foreach (var usuario in node.lista)
                {
                    if (usuario.nombre == nombre)
                    {
                        resultados.Add(usuario);
                    }
                }

                // Luego busca en los subárboles izquierdo y derecho
                BuscarPorNombre(node.iz, nombre, resultados);
                BuscarPorNombre(node.der, nombre, resultados);
            }
        }

        //BUSQUEDA POR DPI
        public List<UsuarioModel> BuscarPorDPI(string dpi)
        {
            List<UsuarioModel> resultados = new List<UsuarioModel>();
            BuscarPorDPI(raiz, dpi, resultados);
            return resultados;
        }

        private void BuscarPorDPI(Node node, string dpi, List<UsuarioModel> resultados) 
        {
            if (node == null)
            {
                return;
            }

            //int comparacion = dpi.CompareTo(node.dato.dpi);
            int comparacionDPI = string.Compare(dpi, node.lista[0].dpi);

            if (comparacionDPI == 0)
            {
                resultados.AddRange(node.lista);
            }
            // Continuar la búsqueda en ambos subárboles sin sobrescribir resultados.
            BuscarPorDPI(node.iz, dpi, resultados);
            BuscarPorDPI(node.der, dpi, resultados);
        }
        //BUSQUEDA PARA ACTUALIZAR
        public void BuscarPorNombreDPI(UsuarioModel nuevoDato)
        {
            if (nuevoDato != null && nuevoDato.nombre != null && nuevoDato.dpi != null)
            {
                // Realizar la búsqueda por nombre y DPI
                List<UsuarioModel> resultados = new List<UsuarioModel>();
                BuscarPorNombreDPI(raiz, nuevoDato.nombre, nuevoDato.dpi, resultados);

                // Eliminar los registros encontrados
                foreach (UsuarioModel registroExistente in resultados)
                {
                    Eliminar(registroExistente.nombre, registroExistente.dpi);
                }

                // Insertar el nuevo registro actualizado
                Insertar(nuevoDato);
            }
        }

        private void BuscarPorNombreDPI(Node node, string nombre, string dpi, List<UsuarioModel> resultados)
        {
            if (node != null)
            {
                // Buscar coincidencia por nombre y DPI en la lista de usuarios en este nodo
                foreach (var usuario in node.lista)
                {
                    if (usuario.nombre == nombre && usuario.dpi == dpi)
                    {
                        resultados.Add(usuario);
                    }
                }

                // Luego buscar en los subárboles izquierdo y derecho
                BuscarPorNombreDPI(node.iz, nombre, dpi, resultados);
                BuscarPorNombreDPI(node.der, nombre, dpi, resultados);
            }
        }


    }

    
  }
