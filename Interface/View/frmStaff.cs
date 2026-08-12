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
using Guna.UI2.WinForms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmStaff : frmSampleView
    {
        DataTable dt = null;
        string err;
        public int strID;
        Staff staff = new Staff();

        public frmStaff()
        {
            InitializeComponent();
        }
        private void frmStaff_Load(object sender, EventArgs e)
        {
            dgvStaff.DataBindingComplete += dgvStaff_DataBindingComplete;
            LoadData();
        }
        // Hàm đánh số thứ tự
        private void dgvStaff_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvStaff.Rows.Count; i++)
            {
                dgvStaff.Rows[i].Cells["dgvSno"].Value = i + 1;
            }
        }
        private void LoadData()
        {
            try
            {
                // Xóa dữ liệu cũ
                if (dt == null) dt = new DataTable();
                dt.Clear();
                dgvStaff.AutoGenerateColumns = false;

                DataSet ds = staff.LoadStaff();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dgvStaff.DataSource = dt;
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
        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click header
            if (e.RowIndex < 0) return;

            string columnName = dgvStaff.Columns[e.ColumnIndex].Name;
            int r = dgvStaff.CurrentCell.RowIndex;
            this.strID = Convert.ToInt32(dgvStaff.Rows[r].Cells["dgvid"].Value);

            if (columnName == "dgvedit")
            {
                frmStaffAdd frm = new frmStaffAdd();

                frm.txtID.Text = this.strID.ToString();
                frm.txtID.Visible = true;
                frm.txtID.ReadOnly = true;

                DataGridViewRow row = dgvStaff.Rows[e.RowIndex];
                frm.txtFullName.Text = Convert.ToString(row.Cells["dgvName"].Value);
                frm.txtPhone.Text = Convert.ToString(row.Cells["dgvPhone"].Value);
                frm.cmbGender.SelectedItem = Convert.ToString(row.Cells["dgvGender"].Value);
                frm.cmbRole.SelectedItem = Convert.ToString(row.Cells["dgvRole"].Value);
                frm.DateOfBirth.Value = Convert.ToDateTime(row.Cells["dgvDateofBirth"].Value);
                frm.ShowDialog();
                LoadData();
            }

            else if (columnName == "dgvdel")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this staff?", "Confirm",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        staff.DelStaff(ref err, strID);
                        MessageBox.Show("Deleted successfully.");
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Delete failed.");
                    }
                }
            }
            LoadData();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            frmStaffAdd frm = new frmStaffAdd();
            frm.add = true;
            frm.txtID.Visible = true;
            frm.txtID.ReadOnly = true;
            frm.ShowDialog();
            LoadData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData(); // Nếu không nhập gì thì load lại toàn bộ
                return;
            }

            try
            {
                DataTable dtSearch = staff.SearchStaffByName(keyword);
                dgvStaff.DataSource = dtSearch;

                // Gán lại số thứ tự
                for (int i = 0; i < dgvStaff.Rows.Count; i++)
                {
                    dgvStaff.Rows[i].Cells["dgvSno"].Value = i + 1;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Lỗi tìm kiếm dữ liệu.");
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSearch_TextChanged(sender, e);
            }
        }
    }
}
