using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmProduct : frmSampleView
    {
        DataTable dt = null;
        string err;
        Product pro = new Product();
        public int strID;

        public frmProduct()
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
                dgvProduct.AutoGenerateColumns = false;

                DataSet ds = pro.LoadProductsWithCategory();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dgvProduct.DataSource = dt;

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmProductAdd frm = new frmProductAdd();
            frm.add = true;
            frm.ShowDialog();
            LoadData();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvProduct.Columns[e.ColumnIndex].Name;
            int r = dgvProduct.CurrentCell.RowIndex;
            this.strID = Convert.ToInt32(dgvProduct.Rows[r].Cells["dgvid"].Value);

            if (columnName == "dgvedit")
            {
                frmProductAdd frm = new frmProductAdd();
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                frm.txtName.Text = Convert.ToString(row.Cells["dgvName"].Value);
                frm.txtPrice.Text = Convert.ToString(row.Cells["dgvPrice"].Value);
                frm.cmbCate.SelectedValue = Convert.ToString(row.Cells["dgvCate"].Value);
                frm.cID = this.strID; // ID sản phẩm
                frm.categoryID = Convert.ToInt32(row.Cells["dgvid"].Value); // ID danh mục

                if (row.Cells["dgvpImage"].Value != DBNull.Value)
                {
                    byte[] imageByteArray = (byte[])row.Cells["dgvpImage"].Value;
                    frm.txtImage.Image = Image.FromStream(new MemoryStream(imageByteArray));
                }
                else
                {
                    frm.txtImage.Image = null;
                }
                frm.add = false;
                frm.ShowDialog();
            }
            else if (columnName == "dgvdel")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this product?", "Confirm",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        bool result = pro.DeleteProduct(ref err, strID);
                        if (result)
                            MessageBox.Show("Deleted successfully.");
                        else
                            MessageBox.Show("Delete failed: " + err);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Delete failed due to SQL error.");
                    }
                }
            }
            LoadData();
        }
        // Hàm đánh số thứ tự
        private void dgvProduct_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                dgvProduct.Rows[i].Cells["dgvSno"].Value = i + 1;
            }
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            dgvProduct.DataBindingComplete += dgvProduct_DataBindingComplete;
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
                DataTable dtSearch = pro.SearchProductByName(keyword);
                dgvProduct.DataSource = dtSearch;

                // Gán lại số thứ tự
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    dgvProduct.Rows[i].Cells["dgvSno"].Value = i + 1;
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
