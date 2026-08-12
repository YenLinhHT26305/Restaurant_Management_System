using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management_System.DB_layer;
using System.Windows.Forms;

namespace Restaurant_Management_System.BS_layer
{
    class Product
    {
        DBMain db = null;

        public Product()
        {
            db = new DBMain();
        }

        public DataSet LoadProductsWithCategory()
        {
            string sql = @"SELECT p.pID, p.pName, p.pPrice, p.CategoryID,p.pImage, c.catName
                   FROM products p
                   INNER JOIN category c ON c.catID = p.CategoryID";
            return db.ExecuteQueryDataSet(sql, CommandType.Text, null);
        }


        public void LoadProducts_forProAdd(ComboBox cb)
        {
            db.CBFill("SELECT catID AS id, catName AS name FROM category", cb);
        }

        public bool AddProduct(string name, int price, int categoryID, byte[] image, ref string err)
        {
            string sqlString = "INSERT INTO products(pName, pPrice, CategoryID, pImage) VALUES(@name, @price, @categoryID, @image)";
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@name", name),
            new SqlParameter("@price", price),
            new SqlParameter("@categoryID", categoryID),
            new SqlParameter("@image", image)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public bool DeleteProduct(ref string err, int id)
        {
            string sqlString = "DELETE FROM products WHERE pID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public bool UpdateProduct(int id, string name, int price, int categoryID, byte[] image, ref string err)
        {
            string sqlString = "UPDATE products SET pName = @name, pPrice = @price, CategoryID = @categoryID, pImage = @image WHERE pID = @id";
            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@name", name),
            new SqlParameter("@price", price),
            new SqlParameter("@categoryID", categoryID),
            new SqlParameter("@image", image),
            new SqlParameter("@id", id)
            };
            return db.MyExecuteNonQuery(sqlString, CommandType.Text, ref err, param);
        }

        public DataTable SearchProductByName(string keyword)
        {
            string sqlString = "SELECT * FROM products WHERE pName LIKE @keyword";
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@keyword", "%" + keyword + "%")
            };

            return db.ExecuteQueryDataTable(sqlString, CommandType.Text, parameters);
        }
    }
}
