using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_Estructura2.Model
{
    public class UsuarioModel 
    {
        [JsonProperty("name")]
        public string nombre { get; set; }
        public string dpi { get; set; }

        [JsonProperty("dateBirth")]
        public string nacimiento { get; set; }
        [JsonProperty("address")]
        public string direccion { get; set; }
        public int CompareTo(UsuarioModel other)
        {
            if (this == null && other == null)
            {
                return 0; // Ambos objetos son nulos, considerarlos iguales.
            }
            else if (this == null)
            {
                return -1; // Este objeto es nulo, considerarlo menor que el otro.
            }
            else if (other == null)
            {
                return 1; // El otro objeto es nulo, considerarlo mayor que este.
            }
            else
            {
                if (this.dpi == null && other.dpi == null)
                {
                    return 0; // Ambos nombres son nulos, considerarlos iguales.
                }
                else if (this.dpi == null)
                {
                    return -1; // Este nombre es nulo, considerarlo menor que el otro.
                }
                else if (other.dpi == null)
                {
                    return 1; // El otro nombre es nulo, considerarlo mayor que este.
                }
                else
                {
                    return this.dpi.CompareTo(other.dpi); // Comparar los nombres no nulos.
                }
            }
        }
        /*public int CompareTo(UsuarioModel other)
        {
            if (this == null && other == null)
            {
                return 0; // Ambos objetos son nulos, considerarlos iguales.
            }

            if (this == null)
            {
                return -1; // Este objeto es nulo, considerarlo menor que el otro.
            }

            if (other == null)
            {
                return 1; // El otro objeto es nulo, considerarlo mayor que este.
            }

            string thisDPI = this.dpi ?? string.Empty;
            string otherDPI = other.dpi ?? string.Empty;

            return thisDPI.CompareTo(otherDPI); // Comparar los DPI no nulos.
        }*/

        ///

    }
}
