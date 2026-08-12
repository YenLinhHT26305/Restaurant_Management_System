using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.Interface.View;

namespace Restaurant_Management_System.Interface
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        public static void AddControls(Form f, Panel panel)
        {
            panel.Controls.Clear();                         // Xóa control cũ
            f.TopLevel = false;                             // Quan trọng: để nhúng vào Panel
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;                        // Phủ kín Panel
            panel.Controls.Add(f);                          // Thêm form vào Panel
            f.Show();                                      
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            AddControls(new frmCategory(), CenterPanel);
        }

        private void Staff_Click(object sender, EventArgs e)
        {
            AddControls(new frmStaff(), CenterPanel);
        }

        private void Product_Click(object sender, EventArgs e)
        {
            AddControls(new frmProduct(), CenterPanel);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            frmPos frmPos = new frmPos();
            frmPos.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            AddControls(new frmTable(), CenterPanel);
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            AddControls(new frmKitchen(), CenterPanel);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
