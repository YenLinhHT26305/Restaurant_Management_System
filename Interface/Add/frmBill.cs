using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.Add
{
    public partial class frmBill : Form
    {
        public int MainID;
        public frmBill()
        {
            InitializeComponent();
        }

        private void frmBill_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                dgvPayment.Rows.Clear(); // Xóa dữ liệu cũ

                Main main = new Main();

                Kitchen kitchen = new Kitchen();
                DataSet ds = kitchen.ReLoad_for_Update(MainID); // Chi tiết hóa đơn

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        string proID = row["proID"].ToString();
                        string pName = row["pName"].ToString();
                        string qty = row["qty"].ToString();
                        string price = row["price"].ToString();
                        string amount = row["amount"].ToString();

                        object[] obj = { 0, proID, pName, qty, price, amount };
                        dgvPayment.Rows.Add(obj);
                    }
                    UpdateSerialNumbers();
                }

                // LoadMain thông tin 
                Staff staff = new Staff();
                lbCashier.Text = staff.GetCashierName();

                DataSet ds1 = main.LoadMain(MainID);
                if (ds1 != null && ds1.Tables.Count > 0)
                {
                    DataTable dt1 = ds1.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        DateTime? orderTime = main.GetOrderTime(MainID);                   
                        lbTime.Text = orderTime.Value.ToString("dd/MM/yyyy HH:mm:ss");
                        DataRow row1 = dt1.Rows[0];
                        lbOrderType.Text = row1["orderType"].ToString();
                        if (lbOrderType.Text == "Delivery" || lbOrderType.Text == "Take Away")
                        {
                            lbTable.Text = "";
                            lbWaiter.Text = "";
                        }
                        else
                        {
                            lbTable.Text = row1["TableName"].ToString();
                            lbWaiter.Text = row1["WaiterName"].ToString();
                        }
                        int total = Convert.ToInt32(row1["total"]);
                        lbTotal.Text = total.ToString("N0", new CultureInfo("vi-VN")) + " đ";
                    }
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu bàn/phục vụ để hiển thị.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu hóa đơn: " + ex.Message);
            }
        }
        // Hàm đánh số thứ tự 
        private void UpdateSerialNumbers()
        {
            for (int i = 0; i < dgvPayment.Rows.Count; i++)
            {
                dgvPayment.Rows[i].Cells["dgvSno"].Value = i + 1;
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

