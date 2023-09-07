using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_Estructura2.Model
{
    public class Node
    {
        public Node(UsuarioModel user) {
            dato = user;
            iz = null;
            der = null;
            padre = null;
            lista = new List<UsuarioModel> {user};
            this.altura = 0;
        }
        public UsuarioModel dato { get; set; }
        public Node iz { get; set; }
        public Node der { get; set; }
        public Node padre { get; set; }
        public List<UsuarioModel> lista { get; set; }
        public int altura { get; set; }


    }
}
