using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmCategoryAdd : Form
    {
        DataTable dt = null;
        string err;
        Category cate = new Category();
        public bool add;
        public string strID;
        public frmCategoryAdd()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {          
            if (!this.add)  // đang sửa
            {
                // ID được truyền từ frmCategory, không cần tạo mới frmCategory
                cate.UpdateCategory(this.strID, this.txtName.Text.Trim(), ref err);
                MessageBox.Show("Update successfully !");
            }
            else  // thêm mới
            {
                cate.AddCategory(this.txtName.Text.Trim(), ref err);
                MessageBox.Show("Add a new category successfully !");
            }

            if (!string.IsNullOrEmpty(err))
                MessageBox.Show("Error: " + err);
            this.Close();
        }
    }
}
