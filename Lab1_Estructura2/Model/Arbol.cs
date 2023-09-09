using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab1_Estructura2.Model
{
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
        //FUNCION INSERTAR
        public void Insertar(UsuarioModel usuario)
        {
            if (usuario != null && usuario.nombre != null)
            {
                int dato = usuario.nombre.GetHashCode(); // Puedes usar el hash del nombre como dato
                raiz = Insertar(raiz, dato, usuario);
            }
        }

        private Node Insertar(Node node, int dato, UsuarioModel usuario)
        {

            if (node == null)
            {
                Node newNode = new Node(dato);
                newNode.lista.Add(usuario);
                return newNode;

            }

            if (dato < node.dato)
            {
                node.iz = Insertar(node.iz, dato, usuario);
            }
            else if (dato > node.dato)
            {
                node.der = Insertar(node.der, dato, usuario);
            }
            else
            {
                // Valor duplicado, si es necesario, maneja la lógica aquí
                node.lista.Add(usuario);
                return node;
            }

            node.altura = 1 + Math.Max(Altura(node.iz), Altura(node.der));

            int balance = FactorBalance(node);

            // Casos de rotación
            if (balance > 1)
            {
                if (dato < node.iz.dato)
                    return RotarDerecha(node);
                else
                {
                    node.iz = RotarIzquierda(node.iz);
                    return RotarDerecha(node);
                }
            }
            if (balance < -1)
            {
                if (dato > node.der.dato)
                    return RotarIzquierda(node);
                else
                {
                    node.der = RotarDerecha(node.der);
                    return RotarIzquierda(node);
                }
            }

            return node;
        }

        private Node EncontrarMinimo(Node node)
        {
            Node actual = node;
            while (actual.iz != null)
                actual = actual.iz;
            return actual;
        }
        //FUNCION ELIMINAR
        public void EliminarUsuarioPorNombre(string nombre)
        {
            raiz = EliminarUsuarioPorNombre(raiz, nombre);
        }
        private Node EliminarUsuarioPorNombre(Node node, string nombre)
        {
            if (node == null)
            {
                // No se encontró el nodo, no se hace ninguna acción
                return node;
            }
            if (nombre.CompareTo(node.lista[0].nombre) < 0)
            {
                // El nombre está en el subárbol izquierdo
                node.iz = EliminarUsuarioPorNombre(node.iz, nombre);
            }
            else if (nombre.CompareTo(node.lista[0].nombre) > 0)
            {
                // El nombre está en el subárbol derecho
                node.der = EliminarUsuarioPorNombre(node.der, nombre);
            }
            else
            {
                // Se encontró el nombre, elimina el nodo o ajusta la lista de usuarios según tus necesidades
                if (node.lista.Count > 1)
                {
                    node.lista.RemoveAt(0);
                }
                else
                {
                    // Si solo hay un usuario con el nombre, elimina el nodo completo
                    if (node.iz == null)
                        return node.der;
                    else if (node.der == null)
                        return node.iz;
                    // Nodo con dos hijos, obtén el sucesor inorden (nodo más pequeño en el subárbol derecho)
                    Node sucesor = EncontrarMinimo(node.der);

                    // Copia los datos del sucesor a este nodo
                    node.lista[0] = sucesor.lista[0];

                    // Elimina el sucesor
                    node.der = EliminarUsuarioPorNombre(node.der, sucesor.lista[0].nombre);
                }
            }
            // Actualiza la altura del nodo
            node.altura = 1 + Math.Max(Altura(node.iz), Altura(node.der));
            // Calcula el factor de equilibrio
            int balance = FactorBalance(node);

            // Casos de rotación si es necesario
            if (balance > 1)
            {
                if (nombre.CompareTo(node.iz.lista[0].nombre) < 0)
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
                if (nombre.CompareTo(node.der.lista[0].nombre) > 0)
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
        public void ActualizarUsuarioPorNombre(string nombre, UsuarioModel nuevoUsuario)
        {
            raiz = ActualizarUsuarioPorNombre(raiz, nombre, nuevoUsuario);
        }
        private Node ActualizarUsuarioPorNombre(Node node, string nombre, UsuarioModel nuevoUsuario)
        {

            if (node == null)
            {
                // No se encontró el nodo, no se hace ninguna acción
                return node;
            }
            if (nombre.CompareTo(node.lista[0].nombre) < 0)
            {
                // El nombre está en el subárbol izquierdo
                node.iz = ActualizarUsuarioPorNombre(node.iz, nombre, nuevoUsuario);
            }
            else if (nombre.CompareTo(node.lista[0].nombre) > 0)
            {
                // El nombre está en el subárbol derecho
                node.der = ActualizarUsuarioPorNombre(node.der, nombre, nuevoUsuario);

            }
            else
            {
                // Se encontró el nombre, actualiza la lista de usuarios
                node.lista.Clear();
                node.lista.Add(nuevoUsuario);
            }
            node.altura = 1 + Math.Max(Altura(node.iz), Altura(node.der));

            // Calcula el factor de equilibrio
            int balance = FactorBalance(node);



            // Casos de rotación si es necesario
            if (balance > 1)
            {
                if (nombre.CompareTo(node.iz.lista[0].nombre) < 0)
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
                if (nombre.CompareTo(node.der.lista[0].nombre) > 0)
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
        public Node Buscar(Node node, int dato)
        {
            if (node == null || node.dato == dato)
                return node;

            if (dato < node.dato)
                return Buscar(node.iz, dato);

            return Buscar(node.der, dato);
        }
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
        public List<UsuarioModel> listaOrdenada()
        {
            return InOrderAVL(raiz);
        }
        private List<UsuarioModel> InOrderAVL(Node nodoActual)
        {
            List<UsuarioModel> nodosInOrder = new List<UsuarioModel>();
            if (nodoActual != null)
            {
                nodosInOrder.AddRange(InOrderAVL(nodoActual.iz));
                nodosInOrder.AddRange(nodoActual.lista);
                nodosInOrder.AddRange(InOrderAVL(nodoActual.der));
            }
            return nodosInOrder;
        }


    }
}
