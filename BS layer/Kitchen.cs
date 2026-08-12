using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management_System.DB_layer;

namespace Restaurant_Management_System.BS_layer
{
    class Kitchen
    {
        DBMain db = null;

        public Kitchen()
        {
            db = new DBMain();
        }
        public DataSet AddProduct_into_Kitchen(int mid)
        {
            string sql = @"SELECT p.pName, d.qty
                            FROM tblMain m
                            INNER JOIN tblDetails d ON m.MainID = d.MainID
                            INNER JOIN products p ON p.pID = d.proID
                            WHERE m.MainID = @mid";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@mid", mid)
            };
            return db.ExecuteQueryDataSet(sql, CommandType.Text, param);
        }
        public DataSet ReLoad_for_Update(int mid)
        {
            string sql = @" SELECT d.proID, p.pName,
                            SUM(d.qty) AS qty, 
                            d.price, 
                            SUM(d.amount) AS amount
                            FROM tblMain m
                            INNER JOIN tblDetails d ON m.MainID = d.MainID
                            INNER JOIN products p ON p.pID = d.proID
                            WHERE m.MainID = @mid
                            GROUP BY d.proID, p.pName, d.price";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@mid", mid)
            };
            return db.ExecuteQueryDataSet(sql, CommandType.Text, param);
        }

        public bool UpdateStatus(int id, string status, ref string err)
        {
            string sqlString = "UPDATE tblMain SET status = @status WHERE MainID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@status", status),
                new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }
    }
}
