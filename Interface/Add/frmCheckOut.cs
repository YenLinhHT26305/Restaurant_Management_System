using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.Add
{
    public partial class frmCheckOut : Form
    {
        public bool check; // kiểm tra xem đã thanh toán chưa 
        public bool edit = false;

        public frmCheckOut()
        {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            check = false;
            this.Close();
        }
        private void txtReceived_TextChanged(object sender, EventArgs e)
        {
            int total = ParseAmount(txtBillAmount.Text);
            int receipt = ParseAmount(txtReceived.Text);
            int change = receipt - total;
            lblChange1.Visible = true; // Change 
            lblChange2.Visible = false; // Make an additional payment
            // Trường hợp chỉnh sửa (edit = true)
            if (this.edit)
            {
                if (change < 0) // TH khách bù thêm tiền 
                {
                    lblChange1.Visible = false;
                    lblChange2.Visible = true;
                    change = -change; // Đổi dấu để hiển thị
                }
                else
                {
                    lblChange1.Visible = true;
                    lblChange2.Visible = false;
                }
            }
            txtChange.Text = change.ToString("N0");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int total = ParseAmount(txtBillAmount.Text);
            int receipt = ParseAmount(txtReceived.Text);
            int change = receipt - total;

            // Kiểm tra hợp lệ
            if (receipt <= 0)
            {
                MessageBox.Show("Please enter a valid received amount (positive integer).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReceived.Focus();
                return;
            }
            if (change < 0 && this.edit == false)
            {
                MessageBox.Show("The received amount is not enough to complete the payment.", "Insufficient Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtReceived.Focus();
                return;
            }

            txtChange.Text = change.ToString("N0");
            check = true;
            this.Close();
        }

        // Hàm dùng chung để xử lý chuỗi tiền về số
        private int ParseAmount(string raw)
        {
            int.TryParse(raw.Replace(",", "").Trim(), out int amount);
            return amount;
        }
    }
}
