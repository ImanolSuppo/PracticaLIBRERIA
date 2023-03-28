using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaLIBRERIA.Dominio
{
    internal class DetalleFactura
    {
        public Articulo Articulo { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public DetalleFactura(Articulo articulo, double precio, int cantidad)
        {
            Articulo = articulo;
            Precio = precio;
            Cantidad = cantidad;
        }
        public override string ToString()
        {
            return Articulo.Nombre + " | $" + Precio + " | ";
        }
    }
}
