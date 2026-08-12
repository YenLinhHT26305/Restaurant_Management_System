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
    class Staff
    {
        DBMain db = null;

        public Staff()
        {
            db = new DBMain();
        }

        public DataSet LoadStaff()
        {
            return db.ExecuteQueryDataSet("SELECT staffID, sName, sGender, sDateOfBirth, sPhone, sRole FROM staff", CommandType.Text, null);
        }
        public DataSet LoadWaiterStaff()
        {
            string sql = "SELECT * FROM staff WHERE sRole = @role";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@role", "Waiter")
            };

            return db.ExecuteQueryDataSet(sql, CommandType.Text, parameters);
        }


        public bool AddStaff(string name, string gender, DateTime dateOfBirth, string phone, string role, ref string err)
        {
            string sqlString = "INSERT INTO staff(sName, sGender, sDateOfBirth, sPhone, sRole) VALUES(@name, @gender, @dob, @phone, @role)";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@name", name),
                new SqlParameter("@gender", gender),
                new SqlParameter("@dob", dateOfBirth),
                new SqlParameter("@phone", phone),
                new SqlParameter("@role", role)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public bool DelStaff(ref string err, int id)
        {
            string sqlString = "DELETE FROM staff WHERE staffID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public bool UpdateStaff(int id, string name, string gender, DateTime dateOfBirth, string phone, string role , ref string err)
        {
            string sqlString = "UPDATE staff SET sName = @name, sGender = @gender, sDateOfBirth = @dob, sPhone = @phone, sRole = @role WHERE staffID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@name", name),
                new SqlParameter("@gender", gender),
                new SqlParameter("@dob", dateOfBirth),
                new SqlParameter("@phone", phone),
                new SqlParameter("@role", role),
                new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public DataTable SearchStaffByName(string keyword)
        {
            string sqlString = "SELECT staffID, sName, sGender, sDateOfBirth, sPhone, sRole FROM staff WHERE sName LIKE @keyword";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@keyword", "%" + keyword + "%")
            };

            return db.ExecuteQueryDataTable(sqlString, CommandType.Text, parameters);
        }

        public string GetCashierName()
        {
            string sql = "SELECT sName FROM staff WHERE sRole = @role";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@role", "Cashier")
            };

            DataTable dt = db.ExecuteQueryDataTable(sql, CommandType.Text, param);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["sName"].ToString();
            }
            return null; 
        }

    }
}


