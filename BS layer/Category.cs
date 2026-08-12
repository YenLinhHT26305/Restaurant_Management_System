using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management_System.DB_layer;
using Restaurant_Management_System.Interface;

namespace Restaurant_Management_System.BS_layer
{
    class Category
    {
        DBMain db = null;

        public Category()
        {
            db = new DBMain();
        }

        public DataSet LoadCategory()
        {
            return db.ExecuteQueryDataSet("SELECT catID, catName FROM category", CommandType.Text, null);
        }

        public bool AddCategory(string name, ref string err)
        {
            string sqlString = "INSERT INTO category(catName) VALUES(@name)";
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@name", name)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public bool DelCategory(ref string err, string id)
        {
            string sqlString = "DELETE FROM category WHERE catID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }
        public bool UpdateCategory(string id, string name, ref string err)
        {
            string sqlString = "UPDATE category SET catName = @name WHERE catID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@name", name),
                new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public DataTable SearchCategoryByName(string keyword)
        {
            string sqlString = "SELECT * FROM category WHERE catName LIKE @keyword";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@keyword", "%" + keyword + "%")
            };

            return db.ExecuteQueryDataTable(sqlString, CommandType.Text, parameters);
        }
    }
}