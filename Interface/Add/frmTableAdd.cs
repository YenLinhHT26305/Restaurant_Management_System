using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.Add
{
    public partial class frmTableAdd : Form
    {
        DataTable dt = null;
        string err;
        public bool add;
        public frmTableAdd()
        {
            InitializeComponent();
        }

        private void frmTableAdd_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra tên bàn
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Table name is required.");
                return;
            }

            // Kiểm tra số ghế
            if (string.IsNullOrWhiteSpace(txtSeat.Text) || !int.TryParse(txtSeat.Text.Trim(), out int seatCount) || seatCount <= 0)
            {
                MessageBox.Show("Seats must be a positive number.");
                return;
            }

            // Kiểm tra trạng thái
            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a table status.");
                return;
            }

            Table table = new Table();
            string name = txtName.Text.Trim();
            string status = cmbStatus.SelectedItem.ToString();
            string err = "";

            if (!this.add) // cập nhật bàn
            {
                int id = Convert.ToInt32(txtID.Text.Trim());
                if (table.UpdateTable(id, name, status, seatCount, ref err))
                {
                    MessageBox.Show("Update successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error: " + err);
                }
            }
            else // thêm mới bàn
            {
                if (table.AddTable(name, status, seatCount, ref err))
                {
                    MessageBox.Show("Add a new table successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error: " + err);
                }
            }
        }

    }
}