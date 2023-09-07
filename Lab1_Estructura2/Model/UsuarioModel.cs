using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_Estructura2.Model
{
    public class UsuarioModel : IComparable<UsuarioModel>
    {
        public string nombre { get; set; }
        public string dpi { get; set; }
        public string nacimiento { get; set; }
        public string direccion { get; set; }
        public int CompareTo(UsuarioModel other)
        {
            int result = this.nombre.CompareTo(other.nombre);
            return result;
        }
    }
}
