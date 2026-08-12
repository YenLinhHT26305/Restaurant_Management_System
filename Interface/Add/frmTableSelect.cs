using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;
using Restaurant_Management_System.Usercontrol;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmTableSelect : Form
    {
        public string tableName = "";
        public int tableID; 
        private string err = "";
        public frmTableSelect()
        {
            InitializeComponent();
        }

        private void frmTableSelect_Load(object sender, EventArgs e)
        {
            try
            {
                Table table = new Table();
                DataSet ds = table.LoadAvailableTables();

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button
                        {
                            Text = row["tName"].ToString(),
                            Width = 150,
                            Height = 50,
                            FillColor = Color.FromArgb(103, 17, 76),
                            HoverState = { FillColor = Color.FromArgb(50, 55, 89) },
                            Margin = new Padding(10),
                            Tag = row["tID"] // Gán ID bàn vào Tag để sử dụng sau
                        };

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
            frmPos frmPos = new frmPos();
            var button = sender as Guna.UI2.WinForms.Guna2Button;
            tableName = button.Text;
            tableID = Convert.ToInt32(button.Tag); // Lấy ID bàn từ Tag
            this.Close();           
        }
        public void Update_Table_Status(int tID, string status)
        {
            Table table = new Table();
            table.UpdateTableStatus(tID, status , ref err);
        }
    }
}
