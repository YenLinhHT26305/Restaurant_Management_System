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
    class Main
    {
        DBMain db = null;

        public Main()
        {
            db = new DBMain();
        }
        public DataSet LoadforKitchen()
        {
            string sql = "SELECT * FROM tblMain where status = 'Pending' ";
            return db.ExecuteQueryDataSet(sql, CommandType.Text, null);
        }

        public DataSet LoadMain(int mainID)
        {
            string sql = "SELECT * FROM tblMain where MainID= @mainID ";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@mainID", mainID)
            };
            return db.ExecuteQueryDataSet(sql, CommandType.Text, param);
        }

        // Thêm hóa đơn mới
        public bool AddOrder(DateTime orderTime, string tableName, string waiterName, string status, string orderType, int total, int received, int change, ref string err)
        {
            string sqlString = "INSERT INTO tblMain(orderTime, TableName, WaiterName, status, orderType, total, received, change) " +
                               "VALUES(@orderTime, @tableName, @waiterName, @status, @orderType, @total, @received, @change)";
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@orderTime", orderTime),
            new SqlParameter("@tableName", tableName),
            new SqlParameter("@waiterName", waiterName),
            new SqlParameter("@status", status),
            new SqlParameter("@orderType", orderType),
            new SqlParameter("@total", total),
            new SqlParameter("@received", received),
            new SqlParameter("@change", change)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }
        // Xóa hóa đơn theo MainID
        public bool DeleteOrder(int mainID, ref string err)
        {
            string sqlString = "DELETE FROM tblMain WHERE MainID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@id", mainID)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        // Cập nhật đơn
        public bool UpdateOrder(int mainID, DateTime orderTime, string tableName, string waiterName, string status, string orderType, int total, int received, int change, ref string err)
        {
            string sqlString = "UPDATE tblMain SET orderTime = @orderTime, TableName = @tableName, WaiterName = @waiterName, status = @status, " +
                               "orderType = @orderType, total = @total, received = @received, change = @change WHERE MainID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@orderTime", orderTime),
            new SqlParameter("@tableName", tableName),
            new SqlParameter("@waiterName", waiterName),
            new SqlParameter("@status", status),
            new SqlParameter("@orderType", orderType),
            new SqlParameter("@total", total),
            new SqlParameter("@received", received),
            new SqlParameter("@change", change),
            new SqlParameter("@id", mainID)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }
        // Lấy MainID mới nhất (có giá trị cao nhất)
        public int GetLatestMainID()
        {
            string sql = "SELECT TOP 1 MainID FROM tblMain ORDER BY MainID DESC";
            DataTable dt = db.ExecuteQueryDataTable(sql, CommandType.Text, null);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["MainID"]);
            }
            return -1; // hoặc 0 nếu bạn muốn
        }

        // LoadMain các hóa đơn có trạng thái khác 'Pending'
        public DataSet LoadProcessedOrders()
        {
            string sql = "SELECT * FROM tblMain WHERE status = 'Complete'";
            return db.ExecuteQueryDataSet(sql, CommandType.Text, null);
        }


        // Lấy thời gian đặt hàng (orderTime) của một hóa đơn theo MainID
        public DateTime? GetOrderTime(int mainID)
        {
            string sql = "SELECT orderTime FROM tblMain WHERE MainID = @mainID";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@mainID", mainID)
            };

            DataTable dt = db.ExecuteQueryDataTable(sql, CommandType.Text, param);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToDateTime(dt.Rows[0]["orderTime"]);
            }
            return null; // không tìm thấy hóa đơn
        }
    }
}
