using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management_System.DB_layer
{
    internal class DBMain
    {
        string ConStr = "Data Source=LAPTOP-PSVNV44T;Initial Catalog=RMS;Integrated Security=True";
        SqlConnection conn = null;
        SqlCommand comm = null;
        SqlDataAdapter da = null;

        public DBMain()
        {
            conn = new SqlConnection(ConStr);
            comm = conn.CreateCommand();
        }
        public DataSet ExecuteQueryDataSet(string strSQL, CommandType ct, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                using (SqlCommand command = new SqlCommand(strSQL, connection))
                {
                    command.CommandType = ct;
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public DataTable ExecuteQueryDataTable(string strSQL, CommandType ct, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                using (SqlCommand command = new SqlCommand(strSQL, connection))
                {
                    command.CommandType = ct;
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
        //Thêm phiên bản có tham số
        public bool MyExecuteNonQuery(string strSQL, CommandType ct, ref string error, SqlParameter[] param)
        {
            bool f = false;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            comm.Parameters.Clear(); 

            if (param != null)
                comm.Parameters.AddRange(param);

            try
            {
                comm.ExecuteNonQuery();
                f = true;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return f;
        }
        public void CBFill(string strSQL, ComboBox cb)
        {
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cb.DisplayMember = "name";
                    cb.ValueMember = "id";
                    cb.DataSource = dt;
                    cb.SelectedIndex = -1;
                }
            }
        }
    }
}

      