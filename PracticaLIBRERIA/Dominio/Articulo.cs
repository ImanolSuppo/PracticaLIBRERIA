using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaLIBRERIA.Dominio
{
    internal class Articulo
    {
        public int IdArticulo { get; set; }
        public string Nombre { get; set; }
        public Articulo(int idArticulo, string nombre)
        {
            IdArticulo = idArticulo;
            Nombre = nombre;
        }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
