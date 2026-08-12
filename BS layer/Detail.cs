using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management_System.DB_layer;

namespace Restaurant_Management_System.BS_layer
{
    class Detail
    {
        DBMain db = null;

        public Detail()
        {
            db = new DBMain();
        }

        public bool AddDetail(int mainID, int proID, int qty, int price, int amount, ref string err)
        {
            string sqlString = "INSERT INTO tblDetails(MainID, proID, qty, price, amount) " +
                               "VALUES(@mainID, @proID, @qty, @price, @amount)";
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@mainID", mainID),
            new SqlParameter("@proID", proID),
            new SqlParameter("@qty", qty),
            new SqlParameter("@price", price),
            new SqlParameter("@amount", amount)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public bool DeleteDetailByMainID(ref string err, int mainID)
        {
            string sqlString = "DELETE FROM tblDetails WHERE MainID = @mainID";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@mainID", mainID)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }


        public bool DeleteDetailByProID(int proID, ref string err)
        {
            string sqlString = "DELETE FROM tblDetails WHERE proID = @proID";
            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@proID", proID)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }
    }
}
