using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PracticaLIBRERIA.Datos;
using PracticaLIBRERIA.Presentacion;

namespace PracticaLIBRERIA
{
    public partial class FrmConsultarArticulo : Form
    {
        private DAO dao;
        public FrmConsultarArticulo()
        {
            InitializeComponent();
            dao = DAO.CrearInstancia();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void FrmConsultarArticulo_Load(object sender, EventArgs e)
        {

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtArticulo.Text))
            {
                MessageBox.Show("Debe poner un articulo", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string articulo = txtArticulo.Text;
            DataTable tabla = dao.ConsultarSP("SP_CONSULTAR_ARTICULO", articulo);
            dataGridView1.Rows.Clear();
            if (tabla.Rows.Count == 0)
            {
                MessageBox.Show("No esta el articulo: " + articulo, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            foreach (DataRow item in tabla.Rows)
            {
                string nombre = item["descripcion"].ToString();
                string observaciones = item["observaciones"].ToString();
                int stock = Convert.ToInt32(item["stock"]);
                double precio = Convert.ToDouble(item["pre_unitario"]);
                dataGridView1.Rows.Add(nombre, observaciones, stock, precio);
                //dataGridView1.Rows.Add(item);
            }

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FrmFactura frmFactura = new FrmFactura();
            frmFactura.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro de salir?", "Pregunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
    }
}
