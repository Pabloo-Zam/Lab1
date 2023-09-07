using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Node RotarIzquierda(Node x)
        {
            Node y = x.der;
            Node T2 = y.iz;

            y.iz = x;
            x.der = T2;

            x.altura = Math.Max(Altura(x.iz), Altura(x.der)) + 1;
            y.altura = Math.Max(Altura(y.iz), Altura(y.der)) + 1;

            return y;
        }
        //FUNCION INSERTAR
        public Node Insertar(Node node, UsuarioModel dato)
        {
            if (node == null)
            {
                return new Node(dato);
            }

            if (dato.nombre.CompareTo(node.dato.nombre) < 0)
            {
                node.iz = Insertar(node.iz, dato);
                node.iz.padre = node;
            }
            else if (dato.nombre.CompareTo(node.dato.nombre)>0)
            {
                node.der = Insertar(node.der, dato);
                node.der.padre = node;
            }
            else
            {
                // Valor duplicado, si es necesario, maneja la lógica aquí
                return node;
            }

            node.altura = 1 + Math.Max(Altura(node.iz), Altura(node.der));

            int balance = FactorBalance(node);

            // Casos de rotación
            if (balance > 1)
            {
                if (dato.nombre.CompareTo(node.iz.dato.nombre)<0)
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
    }

   
}
