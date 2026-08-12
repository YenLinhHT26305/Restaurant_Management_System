using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management_System.DB_layer;

namespace Restaurant_Management_System.BS_layer
{
    public class Login
    {
        private DBMain db = new DBMain();

        public bool CheckLogin(string username, string password)
        {
            string sql = $"SELECT * FROM users WHERE username = '{username}' AND upass = '{password}'";
            DataSet ds = db.ExecuteQueryDataSet(sql, CommandType.Text, null);

            return ds.Tables[0].Rows.Count > 0;
        }
    }
}

