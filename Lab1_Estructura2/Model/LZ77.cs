using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_Estructura2.Model
{
    public class LZ77
    {
        public static string Comprimir(string texto)
        {
            List<string> salida = new List<string>();
            int i = 0;

            while (i < texto.Length)
            {
                int longitud = 0;
                int offset = 0;
                char caracterActual = texto[i];

                for (int j = i - 1; j >= 0 && j >= i - 255; j--)
                {
                    if (texto[j] == caracterActual)
                    {
                        int k = j;
                        int l = i;

                        while (l < texto.Length && texto[k] == texto[l] && l < i + 255)
                        {
                            k++;
                            l++;
                            longitud++;
                        }

                        if (longitud > 1)
                        {
                            offset = i - j;
                            break;
                        }
                    }
                }

                if (longitud > 0)
                {
                    salida.Add($"({offset},{longitud},{caracterActual})");
                    i += longitud + 1;
                }
                else
                {
                    salida.Add($"(0,0,{caracterActual})");
                    i++;
                }
            }

            return string.Join("", salida);
        }

        public static string Descomprimir(string textoComprimido)
        {
            List<char> salida = new List<char>();
            int i = 0;

            while (i < textoComprimido.Length)
            {
                char c = textoComprimido[i];

                if (c == '(')
                {
                    int j = i + 1;
                    while (j < textoComprimido.Length && textoComprimido[j] != ')')
                    {
                        j++;
                    }

                    if (j < textoComprimido.Length && textoComprimido[j] == ')')
                    {
                        string[] partes = textoComprimido.Substring(i + 1, j - i - 1).Split(',');
                        int offset = int.Parse(partes[0]);
                        int longitud = int.Parse(partes[1]);
                        char caracter = partes[2][0];

                        for (int k = 0; k < longitud; k++)
                        {
                            salida.Add(salida[salida.Count - offset]);
                        }

                        salida.Add(caracter);

                        i = j + 1;
                    }
                    else
                    {
                        salida.Add(c);
                        i++;
                    }
                }
                else
                {
                    salida.Add(c);
                    i++;
                }
            }
            return new string(salida.ToArray());
        }
    }

 }

