using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restaurant_Management_System.BS_layer;
using Restaurant_Management_System.Interface.Add;
using Restaurant_Management_System.Interface.View;
using Restaurant_Management_System.Usercontrol;

namespace Restaurant_Management_System.Interface
{
    public partial class frmPos : Form
    {
        DataTable dt = null;
        Category cate = new Category();

        string err;
        public string strID;
        public bool edit = false;
        public int MainID = 0; // kiểm tra xem có đang tạo đơn mới hay không
        public bool detailID = true; // kiểm tra xem có đang tạo chi tiết đơn mới hay không 
        public string OrderType = "";
        public int Initial_Amount = 0; // tiền ban đầu đã thanh toán (lưu lại để tính cho việc edit lại đơn)
        public int Final_Amount = 0; // tiền sau khi edit đơn (lưu lại để tính cho việc edit lại đơn)
        public int tableselect; // ID bàn đang được chọn   
        public bool isCheckOut = false; // kiểm tra xem thanh toán chưa 
        public frmPos()
        {
            InitializeComponent();
        }
        
        private void frmPos_Load(object sender, EventArgs e)
        {
            AddCategory();
            ProductPanel.Controls.Clear();
            LoadProduct();
            btnKot.Enabled = false; // Vô hiệu hóa KOT khi mới mở form

        }
        // Hàm đánh số thứ tự 
        private void UpdateSerialNumbers()
        {
            for (int i = 0; i < dgvPos.Rows.Count; i++)
            {
                dgvPos.Rows[i].Cells["dgvSno"].Value = i + 1;
            }
        }
        private void AddCategory()
        {
            try
            {
                // Tải dữ liệu danh mục từ cơ sở dữ liệu
                DataSet ds = cate.LoadCategory();

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    // Xóa các button cũ trên giao diện
                    CategoryPanel.Controls.Clear();

                    // Tạo button danh mục mới nếu có dữ liệu
                    foreach (DataRow row in dt.Rows)
                    {
                        var b = new Guna.UI2.WinForms.Guna2Button
                        {
                            FillColor = Color.FromArgb(29, 83, 126),
                            Size = new Size(147, 48),
                            ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                            Text = row["catName"].ToString()
                        };

                        b.Click += new EventHandler(b_Click);
                        CategoryPanel.Controls.Add(b);
                    }
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
        // Lọc sản phẩm theo danh mục (category) khi người dùng click vào nút danh mục (button)
        private void b_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (UcProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        private void AddItems(string id, string name, string cat, int price, Image pimage)
        {
            var w = new UcProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(id)
            };

            ProductPanel.Controls.Add(w);

            // Gán sự kiện khi người dùng chọn sản phẩm
            w.onSelect += (sender, args) =>
            {
                var wdg = (UcProduct)sender;

                frmCheckOut frmCheckOut = new frmCheckOut();

                // Không cho thêm nếu đã thanh toán
                if (this.isCheckOut == true)
                {
                    MessageBox.Show("The order has been paid. You cannot add more items.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra xem sản phẩm đã có trong giỏ chưa
                foreach (DataGridViewRow item in dgvPos.Rows)
                {
                    if (Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.id)
                    {
                        int qty = Convert.ToInt32(item.Cells["dgvQty"].Value) + 1;
                        item.Cells["dgvQty"].Value = qty;
                        item.Cells["dgvAmount"].Value = qty * Convert.ToDouble(item.Cells["dgvPrice"].Value);
                        GetTotal(); //Cập nhật lại tổng tiền sau khi thay đổi
                        return; // Đã cập nhật xong, không cần thêm mới
                    }
                }

                // Nếu sản phẩm chưa có thì thêm mới
                dgvPos.Rows.Add(new object[] { 0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                UpdateSerialNumbers();
                GetTotal();
            };
        }

        private void LoadProduct()
        {
            try
            {
                Product pro = new Product();
                DataSet ds = pro.LoadProductsWithCategory();

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow item in dt.Rows)
                    {
                        byte[] imageBytes = (byte[])item["pImage"];
                        Image img = Image.FromStream(new MemoryStream(imageBytes));

                        AddItems(
                            item["pID"].ToString(),
                            item["pName"].ToString(),
                            item["catName"].ToString(),
                            Convert.ToInt32(item["pPrice"]),
                            img
                        );
                    }
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            foreach (Control item in ProductPanel.Controls)
            {
                if (item is UcProduct pro)
                {
                    // So sánh dựa trên tên sản phẩm hiển thị, không phải tên control
                    pro.Visible = pro.PName.ToLower().Contains(keyword);
                }
            }
        }
        // Tính tổng tiền 
        private void GetTotal()
        {
            int total = 0;
            lblTotal.Text = "";
            foreach (DataGridViewRow item in dgvPos.Rows)
            {
                total += Convert.ToInt32(item.Cells["dgvAmount"].Value);
            }
            lblTotal.Text = total.ToString("N0", new CultureInfo("vi-VN")) + " đ";
        }

        // Tạo lại hóa đơn mới 
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmCheckOut frmCheckOut = new frmCheckOut();
            dgvPos.Rows.Clear();

            lblTable.Text = "";
            lblWaiter.Text = "";
            OrderType = "";
            MainID = 0;
            lblTotal.Text = "0";

            this.edit = false;
            this.isCheckOut = false;
            btnKot.Enabled = false;
            btnCheckOut.Enabled = true;
            dgvPos.Columns["dgvdel"].Visible = true;
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            OrderType = "Delivery";
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            OrderType = "Take Away";
        }

        public void btnExit_Click(object sender, EventArgs e)
        {
            frmKitchen frmKitchen = new frmKitchen();
            this.DialogResult = DialogResult.OK;
            frmKitchen.GetOrder();
            this.Close();
        }
        // Dùng tại quán
        private void btnDin_Click(object sender, EventArgs e)
        {
            OrderType = "Din In";
            frmTableSelect frmT = new frmTableSelect();
            frmT.ShowDialog();
            if (frmT.tableName !="")
            {
                lblTable.Text = frmT.tableName;
                this.tableselect = frmT.tableID;
            }
            else
            {
                lblTable.Text = "";
            }

            frmWaiterSelect frmW = new frmWaiterSelect();
            frmW.ShowDialog();
            if (frmW.waiterName != "")
            {
                lblWaiter.Text = frmW.waiterName;
            }
            else
            {
                lblWaiter.Text = "";
            }
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frmBillList = new frmBillList();
            var result = frmBillList.ShowDialog();
        }

        // Thanh toán tiền
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            frmCheckOut frmCheckOut = new frmCheckOut();
            frmCheckOut.edit = this.edit;
            // làm sạch chuỗi và chuyển thành số
            string raw = lblTotal.Text.Replace(".", "")
                                      .Replace(",", "")
                                      .Replace("đ", "")
                                      .Trim();
            if (this.edit == false) // TH đơn hàng mới 
            {          
                int total = 0;
                int.TryParse(raw, out total);
                frmCheckOut.txtBillAmount.Text = total.ToString();
                frmCheckOut.ShowDialog();
            }
            else // TH sửa đơn cũ
            {
                int.TryParse(raw, out Final_Amount);
                frmCheckOut.txtBillAmount.Text = Final_Amount.ToString();
                frmCheckOut.txtReceived.Text = Initial_Amount.ToString();
                frmCheckOut.ShowDialog();
            }
            // Chỉ khi đã thanh toán xong mới cho phép bấm KOT
            // Đã thanh toán xong thì không được xóa hóa đơn đã thanh toán (không thể bấm tạo hóa đơn mới) 
            if (frmCheckOut.check == true)
            {    btnNew.Enabled = false;
                 btnKot.Enabled = true;
                 btnCheckOut.Enabled = false;
                 this.isCheckOut = true;
                 dgvPos.Columns["dgvdel"].Visible = false;
            }
        }

        // Gửi về bếp để chuẩn bị đơn
        private void btnKot_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            Detail detail = new Detail();
            string err = "";

            if (OrderType =="" && edit == false) // TH đang tạo đơn mới, kiểm tra OrderType có bị rỗng không
            {
                MessageBox.Show("Please select an order type (Delivery / Take Away / Dine In)!");
                return;
            }

            DateTime orderTime = DateTime.Now;
            string tableName = lblTable.Text;
            string waiterName = lblWaiter.Text;
            string orderType = OrderType;
            string status = "Pending";

            int total = 0;
            foreach (DataGridViewRow row in dgvPos.Rows)
            {
                if (row.IsNewRow) continue;

                int amount = Convert.ToInt32(row.Cells["dgvAmount"].Value);
                total += amount;
            }

            // Cập nhật lại lblTotal.Text để phản ánh đúng total vừa tính
            lblTotal.Text = total.ToString("N0", new CultureInfo("vi-VN")) + " đ";

            int received = 0;
            int change = 0;

            // Nếu là đơn mới
            if (MainID == 0)
            {
                main.AddOrder(orderTime, tableName, waiterName, status, orderType, total, received, change, ref err);
                MainID = main.GetLatestMainID();
            }
            else // Cập nhật đơn cũ 
            {
                detail.DeleteDetailByMainID(ref err, MainID); // Xóa chi tiết cũ trước
            }

            // Thêm chi tiết mới
            foreach (DataGridViewRow row in dgvPos.Rows)
            {
                if (row.IsNewRow) continue;

                int proID = Convert.ToInt32(row.Cells["dgvproID"].Value);
                int qty = Convert.ToInt32(row.Cells["dgvQty"].Value);
                int price = Convert.ToInt32(row.Cells["dgvPrice"].Value);
                int amount = Convert.ToInt32(row.Cells["dgvAmount"].Value);

                detail.AddDetail(MainID, proID, qty, price, amount, ref err);
            }
            if (this.edit)
            {
                // Cập nhật lại 
                main.UpdateOrder(MainID, orderTime, tableName, waiterName, status, orderType, total, received, change, ref err);
                this.edit = false;
            }

            MessageBox.Show("Saved successful!");

            if (OrderType == "Din In") // Cập nhật trạng thái bàn 
            {
                frmTableSelect frmTableSelect = new frmTableSelect();
                frmTableSelect.Update_Table_Status(tableselect, "Occupied");
            }

            btnNew.Enabled = true;

            // Tự động tạo hóa đơn mới
            btnNew_Click(sender, e);
        }


        // LoadMain lại dữ liệu lên để edit 
        public void LoadEntries()
        {
            btnKot.Enabled = false;
            btnCheckOut.Enabled = true;
            btnNew.Enabled = false;
            btnBill.Enabled = false;

            this.edit = true;
            try
            {
                dgvPos.Rows.Clear(); // Xóa dữ liệu cũ

                Kitchen kitchen = new Kitchen();
                Main main = new Main();
                Table table = new Table();
                DataSet ds = kitchen.ReLoad_for_Update(MainID); // Chi tiết đơn

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
                        dgvPos.Rows.Add(obj);
                    }

                    UpdateSerialNumbers(); 
                    GetTotal(); // Cập nhật tổng tiền
                    foreach (DataGridViewRow item in dgvPos.Rows)
                    {
                        Initial_Amount += Convert.ToInt32(item.Cells["dgvAmount"].Value);
                    }
                }

                // LoadMain thông tin bàn/phục vụ/loại order 
                DataSet ds1 = main.LoadMain(MainID);
                if (ds1 != null && ds1.Tables.Count > 0)
                {
                    DataTable dt1 = ds1.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        DataRow row1 = dt1.Rows[0];
                        lblTable.Text = row1["TableName"].ToString();
                        lblWaiter.Text = row1["WaiterName"].ToString();
                        OrderType = row1["orderType"].ToString();
                        if ( OrderType =="Din In")
                        {
                            tableselect = table.GetIDtable_from_tblMain(MainID);
                            table.UpdateTableStatus(tableselect, "Available", ref err);
                        }
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
        // Xóa nội dung đang chọn 
        private void dgvPos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvPos.Columns[e.ColumnIndex].Name;
            int r = dgvPos.CurrentCell.RowIndex;
            int proID = Convert.ToInt32(dgvPos.Rows[r].Cells["dgvproID"].Value); 

            if (columnName == "dgvdel")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete?", "Confirm",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        string err = "";
                        Detail detail = new Detail(); // Hoặc lớp xử lý DB bạn đang dùng

                        // Gọi hàm xóa dữ liệu theo ID từ CSDL
                        bool result = detail.DeleteDetailByProID(proID, ref err);

                        if (result)
                        {
                            dgvPos.Rows.RemoveAt(r); // Xóa khỏi DataGridView
                            UpdateSerialNumbers();   // Cập nhật lại số thứ tự
                            GetTotal();              // Cập nhật lại tổng tiền
                            MessageBox.Show("Deleted successfully.");
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
    }
}
