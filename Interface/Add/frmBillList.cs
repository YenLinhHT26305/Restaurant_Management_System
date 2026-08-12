using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;
using Restaurant_Management_System.Interface.View;
namespace Restaurant_Management_System.Interface.Add
{
    public partial class frmBillList : Form
    {
        DataTable dt = null;
        string err;
        public int strID;
        public frmBillList()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;

            string columnName = dgvBill.Columns[e.ColumnIndex].Name;
            int r = dgvBill.CurrentCell.RowIndex;
            strID = Convert.ToInt32(dgvBill.Rows[r].Cells["dgvid"].Value);

            if (columnName == "dgvPrint")
            {
                frmBill frmBill = new frmBill();
                frmBill.MainID = strID;
                frmBill.ShowDialog();
            }
            if (columnName == "dgvdel")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete?", "Confirm",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        string err = "";
                        Detail detail = new Detail();
                        Main main = new Main();

                        bool result = detail.DeleteDetailByMainID(ref err, strID);
                        main.DeleteOrder(strID, ref err);
                        if (result)
                        {
                            dgvBill.Rows.RemoveAt(r); // Xóa dòng khỏi DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Delete failed: " + err);
                        }
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Delete failed due to SQL error.");
                    }
                }
            }
        }
        // Hàm đánh số thứ tự
        private void dgvBill_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvBill.Rows.Count; i++)
            {
                dgvBill.Rows[i].Cells["dgvSno"].Value = i + 1;
            }
        }
        private void LoadData()
        {
            Main main = new Main();
            Staff staff = new Staff();
            lbCashier.Text = staff.GetCashierName();
            try
            {
                // Xóa dữ liệu cũ
                if (dt == null) dt = new DataTable();
                dt.Clear();
                dgvBill.AutoGenerateColumns = false;

                DataSet ds = main.LoadProcessedOrders();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dgvBill.DataSource = dt;
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

        private void frmBillList_Load(object sender, EventArgs e)
        {
            dgvBill.DataBindingComplete += dgvBill_DataBindingComplete;
            LoadData(); 
        }

        private void lbCashier_Click(object sender, EventArgs e)
        {

        }
    }
}
