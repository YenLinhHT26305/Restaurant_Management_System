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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmStaffAdd : Form
    {
        DataTable dt = null;
        string err;
        public bool add;
        public frmStaffAdd()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Full name is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{10}$"))
            {
                MessageBox.Show("Phone number must contain exactly 10 digits.");
                return;
            }

            if (cmbGender.SelectedIndex == -1) // Hoặc dùng cmbGender.SelectedItem == null
            {
                MessageBox.Show("Please select a gender.");
                return;
            }

            if (cmbRole.SelectedIndex == -1) 
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            if (!this.add)  // đang sửa
            {
                Staff staff = new Staff();
                int id = Convert.ToInt32(this.txtID.Text.Trim());

                staff.UpdateStaff(id, this.txtFullName.Text.Trim(),
                    cmbGender.SelectedItem?.ToString(), DateOfBirth.Value.Date,
                    txtPhone.Text.Trim(), cmbRole.SelectedItem?.ToString(), ref err);

                if (string.IsNullOrEmpty(err))  // Kiểm tra nếu không có lỗi
                {
                    MessageBox.Show("Update successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error: " + err);
                }
            }
            else  // thêm mới
            {
                Staff staff = new Staff();
                staff.AddStaff(this.txtFullName.Text.Trim(),
                    cmbGender.SelectedItem?.ToString(), DateOfBirth.Value.Date,
                    txtPhone.Text.Trim(), cmbRole.SelectedItem?.ToString(), ref err);

                if (string.IsNullOrEmpty(err))  // Kiểm tra nếu không có lỗi
                {
                    MessageBox.Show("Add a new staff successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error: " + err);
                }
            }
        }

        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }
    }
}