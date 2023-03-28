using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.ComponentModel.Com2Interop;

namespace PracticaLIBRERIA.Dominio
{
    internal class Facturacs
    {
        public List<DetalleFactura> DetalleFacturas { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        public int IdVendedor { get; set; }
        public Facturacs()
        {
            DetalleFacturas = new List<DetalleFactura>();
        }
        public Facturacs(List<DetalleFactura> detalleFacturas, DateTime fecha, int idCliente, int idVendedor)
        {
            DetalleFacturas = new List<DetalleFactura>();
            Fecha = fecha;
            IdCliente = idCliente;
            IdVendedor = idVendedor;
        }
        public void AgregarDetalle(DetalleFactura detalleFactura)
        {
            DetalleFacturas.Add(detalleFactura);
        }
        public void EliminarDetalle(int indice)
        {
            DetalleFacturas.RemoveAt(indice);
        }
        public override string ToString()
        {
            return Fecha+" | "+DetalleFacturas;
        }
    }
}
