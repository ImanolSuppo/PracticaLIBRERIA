using PracticaLIBRERIA.Datos;
using PracticaLIBRERIA.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaLIBRERIA.Presentacion
{
    public partial class FrmFactura : Form
    {
        private DAO dao;
        private Facturacs factura;
        public FrmFactura()
        {
            InitializeComponent();
            dao= DAO.CrearInstancia();
        }

        private void FrmFactura_Load(object sender, EventArgs e)
        {
            CargarCombo("SP_CONSULTAR_VENDEDOR","cod_vendedor","ape_vendedor",cboVendedor);
            CargarCombo("SP_CONSULTAR_CLIENTE", "cod_cliente", "ape_cliente", cboCliente);
            CargarCombo("SP_CONSULTAR_ARTICULOS", "cod_articulo", "descripcion", cboArticulo);
            Limpiar();
            label1.Text = "PROXIMA FACTURA: N° " + dao.NextId("SP_PROXIMA_FACTURA");
        }

        private void CargarCombo(string SP, string valueMember, string displayMember, ComboBox comboBox)
        {
            comboBox.DataSource = dao.ConsultarSP(SP, null);
            comboBox.ValueMember = valueMember;
            comboBox.DisplayMember = displayMember;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void Limpiar()
        {
            cboCliente.SelectedIndex = -1;
            cboVendedor.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Now;
            cboArticulo.SelectedIndex = -1;
            txtPrecio.Clear();
            nudCantidad.Value = 0;
            dgvDetalle.Rows.Clear();
            factura = new Facturacs();
        }
        private bool ValidarDetalle()
        {
            if (cboArticulo.SelectedIndex == -1) return false;
            if (String.IsNullOrEmpty(txtPrecio.Text)) return false;
            if (nudCantidad.Value == 0) return false;
            return true;
        }
        private bool ValidarFactura()
        {
            if(cboCliente.SelectedIndex==-1) return false;
            if(cboVendedor.SelectedIndex==-1) return false;
            return true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDetalle())
            {
                foreach (DetalleFactura item in factura.DetalleFacturas)
                {
                    if (item.Articulo.IdArticulo.Equals(cboArticulo.ValueMember))
                    {
                        MessageBox.Show("Ya esta agregado ese articulo", "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                }
                int idArticulo = Convert.ToInt32(cboArticulo.SelectedValue);
                string nombre = cboArticulo.Text;
                Articulo articulo = new Articulo(idArticulo, nombre);
                double precio = Convert.ToDouble(txtPrecio.Text);
                int cantidad = (int)nudCantidad.Value;
                DetalleFactura detalleFactura = new DetalleFactura(articulo, precio, cantidad);
                factura.AgregarDetalle(detalleFactura);
                dgvDetalle.Rows.Add(detalleFactura.Articulo.Nombre, detalleFactura.Cantidad);
                label5.Text = "Total: " + (dgvDetalle.Rows.Count - 1).ToString();
            }
            else
                MessageBox.Show("Verifique los datos!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dgvDetalle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalle.CurrentCell.ColumnIndex == 2)
            {
                factura.EliminarDetalle(dgvDetalle.CurrentRow.Index);
                dgvDetalle.Rows.Remove(dgvDetalle.CurrentRow);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarFactura())
            {
                factura.IdCliente = Convert.ToInt32(cboCliente.SelectedValue);
                factura.IdVendedor = Convert.ToInt32(cboVendedor.SelectedValue);
                factura.Fecha = dtpFecha.Value;
                if (dao.AltaFactura(factura))
                    MessageBox.Show("Se guardó con exito", "Datos", MessageBoxButtons.OK);
                Limpiar();
                label1.Text = "PROXIMA FACTURA: N° " + dao.NextId("SP_PROXIMA_FACTURA");
            }
            else
                MessageBox.Show("Verifique los datos!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
