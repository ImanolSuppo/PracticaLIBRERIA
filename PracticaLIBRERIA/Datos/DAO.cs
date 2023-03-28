using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using PracticaLIBRERIA.Dominio;

namespace PracticaLIBRERIA.Datos
{
    internal class DAO
    {
        private static DAO instancia;
        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlTransaction trs;
        private DAO()
        {
            cnn= new SqlConnection(Properties.Resources.String1);
        }
        public static DAO CrearInstancia()
        {
            if(instancia==null)
                instancia= new DAO();
            return instancia;
        }
        public void ConfigurarComando(string SP)
        {
            cnn.Open();
            cmd =new SqlCommand(SP,cnn);
            cmd.CommandType = CommandType.StoredProcedure;
        }

        public DataTable ConsultarSP(string SP, string articulo)
        {
            ConfigurarComando(SP);
            DataTable dt = new DataTable();
            if(SP== "SP_CONSULTAR_ARTICULO")
                cmd.Parameters.AddWithValue("@descripcion", articulo);
            dt.Load(cmd.ExecuteReader());
            cnn.Close();
            return dt;
        }
        public bool AltaFactura(Facturacs factura)
        {
            bool result = true;
            try
            {
                cnn.Open();
                trs = cnn.BeginTransaction();
                cmd=new SqlCommand("SP_INSERTAR_FACTURA", cnn,trs);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fecha", factura.Fecha);
                cmd.Parameters.AddWithValue("@cliente", factura.IdCliente);
                cmd.Parameters.AddWithValue("@vendedor",factura.IdVendedor);
                SqlParameter param = cmd.Parameters.Add("@nro_factura", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int nroFactura = (int)param.Value;
                for (int i = 0; i < factura.DetalleFacturas.Count; i++)
                {
                    cmd = new SqlCommand("SP_INSERTAR_DETALLE", cnn, trs);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nro_factura", nroFactura);
                    cmd.Parameters.AddWithValue("@id_articulo", factura.DetalleFacturas[i].Articulo.IdArticulo);
                    cmd.Parameters.AddWithValue("@precio", factura.DetalleFacturas[i].Precio);
                    cmd.Parameters.AddWithValue("@cantidad", factura.DetalleFacturas[i].Cantidad);
                    cmd.ExecuteNonQuery();
                }
                trs.Commit();

            }
            catch (Exception ex)
            {
                trs.Rollback();
                MessageBox.Show("Error: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return result;
        }

        public int NextId(string SP)
        {
            ConfigurarComando(SP);
            SqlParameter param = cmd.Parameters.Add("@next", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            int next;
            if (param.Value != DBNull.Value)
                next = (int)param.Value;
            else
                next = 1;
            cnn.Close();
            return next;
        }
    }
}
