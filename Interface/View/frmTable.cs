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
using Restaurant_Management_System.Interface.Add;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmTable : frmSampleView
    {
        DataTable dt = null;
        string err;
        public string strID;
        Table table  = new Table();

        public frmTable()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            try
            {
                // Xóa dữ liệu cũ
                if (dt == null) dt = new DataTable();
                dt.Clear();

                DataSet ds = table.LoadTables();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dgvTable.DataSource = dt;
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
        private void frmTableView_Load(object sender, EventArgs e)
        {
            LoadData();
            dgvTable.DataBindingComplete += dgvTable_DataBindingComplete;
        }
        private void dgvTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvTable.Rows.Count; i++)
            {
                dgvTable.Rows[i].Cells["dgvSno"].Value = i + 1;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmTableAdd frm = new frmTableAdd();
            frm.add = true;
            frm.txtID.Visible = true;
            frm.txtID.ReadOnly = true;
            frm.ShowDialog();
            LoadData();
        }

        private void dgvTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click header
            if (e.RowIndex < 0) return;

            string columnName = dgvTable.Columns[e.ColumnIndex].Name;
            int r = dgvTable.CurrentCell.RowIndex;
            int tableID = Convert.ToInt32(dgvTable.Rows[r].Cells["dgvid"].Value); // cột ID

            if (columnName == "dgvedit")
            {
                frmTableAdd frm = new frmTableAdd();
                frm.txtID.Text = tableID.ToString();
                frm.txtID.Visible = true;
                frm.txtID.ReadOnly = true;
                frm.add = false; // đang sửa

                // LoadMain thông tin từ dòng được chọn
                DataGridViewRow row = dgvTable.Rows[e.RowIndex];
                frm.txtName.Text = Convert.ToString(row.Cells["dgvName"].Value);
                frm.txtSeat.Text = Convert.ToString(row.Cells["dgvSeat"].Value);
                frm.cmbStatus.SelectedItem = Convert.ToString(row.Cells["dgvStatus"].Value);

                frm.ShowDialog();
                LoadData();
            }
            else if (columnName == "dgvdel")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this table?", "Confirm",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {
                    Table table = new Table();
                    string err = "";
                    try
                    {
                        table.DeleteTable(tableID, ref err);
                        if (string.IsNullOrEmpty(err))
                            MessageBox.Show("Deleted successfully.");
                        else
                            MessageBox.Show("Error: " + err);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Delete failed.");
                    }
                }
            }

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
                DataTable dtSearch = table.SearchTableByName(keyword);
                dgvTable.DataSource = dtSearch;

                // Gán lại số thứ tự
                for (int i = 0; i < dgvTable.Rows.Count; i++)
                {
                    dgvTable.Rows[i].Cells["dgvSno"].Value = i + 1;
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
