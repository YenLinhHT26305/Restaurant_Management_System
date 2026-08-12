using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management_System.DB_layer;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Security.Cryptography;
using System.Web.UI.WebControls;

namespace Restaurant_Management_System.BS_layer
{
    class Table
    {
        DBMain db = null;

        public Table()
        {
            db = new DBMain();
        }
        public DataSet LoadAvailableTables()
        {
            string sql = "SELECT * FROM tables WHERE tStatus = 'Available'";
            return db.ExecuteQueryDataSet(sql, CommandType.Text, null);
        }
        public int GetIDtable_from_tblMain(int mainID)
        {
            string sql = @"SELECT t.tID FROM tables t JOIN tblMain m ON t.tName = m.TableName WHERE m.MainID = @mainID";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@mainID", mainID)
            };

            DataTable dt = db.ExecuteQueryDataSet(sql, CommandType.Text, param).Tables[0];

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["tID"]);
            }
            else
            {
                return -1; 
            }
        }

        public DataSet LoadTables()
        {
            string sql = "SELECT * FROM tables";
            return db.ExecuteQueryDataSet(sql, CommandType.Text, null);
        }

        public bool AddTable(string name, string status, int seats, ref string err)
        {
            string sql = "INSERT INTO tables(tName, tStatus, tSeats) VALUES(@name, @status, @seats)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name", name),
                new SqlParameter("@status", status),
                new SqlParameter("@seats", seats)
            };
            return db.MyExecuteNonQuery(sql, CommandType.Text, ref err, parameters);
        }
        public bool UpdateTableStatus(int tableID, string newStatus, ref string err)
        {
            string sql = "UPDATE tables SET tStatus = @status WHERE tID = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@status", newStatus),
        new SqlParameter("@id", tableID)
            };
            return db.MyExecuteNonQuery(sql, CommandType.Text, ref err, parameters);
        }

        public bool UpdateTable(int id, string name, string status, int seats, ref string err)
        {
            string sql = "UPDATE tables SET tName = @name, tStatus = @status, tSeats = @seats WHERE tID = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name", name),
                new SqlParameter("@status", status),
                new SqlParameter("@seats", seats),
                new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sql, CommandType.Text, ref err, parameters);
        }

        public bool DeleteTable(int id, ref string err)
        {
            string sql = "DELETE FROM tables WHERE tID = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sql, CommandType.Text, ref err, parameters);
        }

        public DataTable SearchTableByName(string keyword)
        {
            string sql = "SELECT * FROM tables WHERE tName LIKE @keyword";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@keyword", "%" + keyword + "%")
            };
            return db.ExecuteQueryDataTable(sql, CommandType.Text, parameters);
        }
    }
}
