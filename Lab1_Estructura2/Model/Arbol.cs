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
    }

   
}
