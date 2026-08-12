using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.DB_layer;
using Restaurant_Management_System.Interface.View;
using Restaurant_Management_System.BS_layer;
using System.Data.SqlClient;
using System.Collections;
namespace Restaurant_Management_System.Interface.View
{
    public partial class frmCategory : frmSampleView 
    {
        DataTable dt = null;
        string err;
        public string strID;
        Category cate = new Category();

        public frmCategory()
        {
            InitializeComponent();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            dgvCategory.DataBindingComplete += dgvCategory_DataBindingComplete;
            LoadData();
        }
        // Hàm đánh số thứ tự
        private void dgvCategory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvCategory.Rows.Count; i++)
            {
                dgvCategory.Rows[i].Cells["dgvSno"].Value = i + 1;
            }
        }
        private void LoadData()
        {
            try
            {
                // Xóa dữ liệu cũ
                if (dt == null) dt = new DataTable();
                dt.Clear();

                DataSet ds = cate.LoadCategory();  
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dgvCategory.DataSource = dt;
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            frmCategoryAdd frm = new frmCategoryAdd();
            frm.add = true;            
            frm.txtID.Visible = true;
            frm.txtID.ReadOnly = true;
            frm.ShowDialog();
            LoadData();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click header
            if (e.RowIndex < 0) return;

            string columnName = dgvCategory.Columns[e.ColumnIndex].Name;
            int r = dgvCategory.CurrentCell.RowIndex;
            this.strID = dgvCategory.Rows[r].Cells["dgvid"].Value.ToString();

            if (columnName == "dgvedit")
            {
                frmCategoryAdd frm = new frmCategoryAdd();
                frm.add = false;
                frm.strID = this.strID;
                frm.txtID.Text = this.strID;
                frm.txtName.Text = Convert.ToString(dgvCategory.Rows[e.RowIndex].Cells["dgvName"].Value);
                frm.txtID.Visible = true;
                frm.txtID.ReadOnly = true;
                frm.ShowDialog();
            }

            else if (columnName == "dgvdel")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this category?", "Confirm",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        cate.DelCategory(ref err, strID);
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
                DataTable dtSearch = cate.SearchCategoryByName(keyword);
                dgvCategory.DataSource = dtSearch;

                // Gán lại số thứ tự
                for (int i = 0; i < dgvCategory.Rows.Count; i++)
                {
                    dgvCategory.Rows[i].Cells["dgvSno"].Value = i + 1;
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
