using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmWaiterSelect : Form
    {
        public string waiterName = "";
        public frmWaiterSelect()
        {
            InitializeComponent();
        }

        private void frmWaiterSelect_Load(object sender, EventArgs e)
        {
            try
            {
                Staff staff = new Staff();
                DataSet ds = staff.LoadWaiterStaff();

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();

                        b.Text = row["sName"].ToString();
                        b.Width = 150;
                        b.Height = 50;
                        b.FillColor = Color.FromArgb(103, 17, 76);
                        b.HoverState.FillColor = Color.FromArgb(50, 55, 89);
                        b.Margin = new Padding(10); // tạo khoảng cách giữa các nút

                        b.Click += new EventHandler(b_Click);

                        flowLayoutPanel1.Controls.Add(b); 
                    }
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị.");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Không lấy được nội dung. Lỗi rồi !!!\n" + ex.Message);
            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            waiterName = (sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            this.Close();
        }
    }
}
