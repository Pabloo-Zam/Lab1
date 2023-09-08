﻿using System;
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
                if (dato.nombre.CompareTo(node.der.dato.nombre)<=1)
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
        public Node Eliminar(Node node, UsuarioModel dato)
        {
            if (node == null)
            {
                return node;
            }
            //ELIMINACION RECURSIVA
            if (dato.nombre.CompareTo(node.dato.nombre) < 0)
            {
                node.iz = Eliminar(node.iz, dato);
            }
            else if (dato.nombre.CompareTo(node.dato.nombre) > 0)
            {
                node.der = Eliminar(node.der, dato);
            }
            else
            {
                if ((node.iz == null) || (node.der == null))
                {
                    Node temp = (node.iz != null) ? node.iz : node.der;

                    if (temp == null)
                    {
                        temp = node;
                        node = null;
                    }
                    else
                    {
                        node = temp;
                    }
                }
                else
                {
                    Node temp = EncontrarMinimo(node.der);
                    node.dato = temp.dato;
                    node.der = Eliminar(node.der, temp.dato);
                }
            }

            if (node == null)
                return node;

            node.altura = 1 + Math.Max(Altura(node.iz), Altura(node.der));

            int balance = FactorBalance(node);

            // Casos de rotación
            if (balance > 1)
            {
                if (FactorBalance(node.iz) >= 0)
                    return RotarDerecha(node);
                else
                {
                    node.iz = RotarIzquierda(node.iz);
                    return RotarDerecha(node);
                }
            }
            if (balance < -1)
            {
                if (FactorBalance(node.der) <= 0)
                    return RotarIzquierda(node);
                else
                {
                    node.der = RotarDerecha(node.der);
                    return RotarIzquierda(node);
                }
            }

            return node;
        }
        //FUNCION ACTUALIZAR 
        public Node Actualizar(Node node, UsuarioModel oldData, UsuarioModel newData)
        {
            node = Eliminar(node, oldData);
            node = Insertar(node, newData);
            return node;
        }

        public Node Buscar(Node node, UsuarioModel dato)
        {
            if (node == null || node.dato == dato)
                return node;

            if (dato.nombre.CompareTo(node.dato.nombre)<0)
                return Buscar(node.iz, dato);

            return Buscar(node.der, dato);
        }

        public Node Buscar(UsuarioModel dato)
        {
            return Buscar(raiz, dato);
        }
    }

   
}
