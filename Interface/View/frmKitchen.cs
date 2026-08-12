using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmKitchen : Form
    {
        DataSet ds = null;
        DataTable dt = null;
        DataSet ds1 = null;
        DataTable dt1 = null;
        string err;
        int mid = 0;
        public frmKitchen()
        {
            InitializeComponent();
        }

        private void frmKitchen_Load(object sender, EventArgs e)
        {
            GetOrder();
        }
        public void GetOrder()
        {
            flowLayoutPanel1.Controls.Clear();
            Kitchen kitchen = new Kitchen();
            Main main = new Main();
            ds = main.LoadforKitchen();
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FlowLayoutPanel p1 = new FlowLayoutPanel();
                    p1.AutoSize = true;
                    p1.Width = 450;
                    p1.FlowDirection = FlowDirection.TopDown;
                    p1.BorderStyle = BorderStyle.FixedSingle;
                    p1.Margin = new Padding(10);
                    p1.Padding = new Padding(0);

                    // Panel thông tin đơn hàng (nền xanh đậm)
                    FlowLayoutPanel p2 = new FlowLayoutPanel();
                    p2.BackColor = Color.FromArgb(27, 32, 64); // #1B2040
                    p2.AutoSize = true;
                    p2.FlowDirection = FlowDirection.TopDown;
                    p2.Margin = new Padding(0);
                    p2.Padding = new Padding(10);
                    p2.Width = 420;

                    string[] labels =
                    {
                        "Table : " + dt.Rows[i]["TableName"].ToString(),
                        "Waiter Name : " + dt.Rows[i]["WaiterName"].ToString(),
                        "Order Time : " + Convert.ToDateTime(dt.Rows[i]["orderTime"]).ToShortTimeString(),
                        "Order Type : " + dt.Rows[i]["orderType"].ToString(),
                        "Total : " + Convert.ToInt32(dt.Rows[i]["total"]).ToString("N0", new System.Globalization.CultureInfo("vi-VN")) + " đ"
                    };
                    int a = Convert.ToInt32(dt.Rows[i]["total"]);
                    foreach (var text in labels)
                    {
                        Label label = new Label();
                        label.ForeColor = Color.White;
                        label.Font = new Font("Segoe UI", 12, FontStyle.Regular);
                        label.Margin = new Padding(3);
                        label.AutoSize = true;
                        label.Text = text;
                        p2.Controls.Add(label);
                    }

                    // Panel danh sách món ăn (nền trắng, nằm dưới p2)
                    FlowLayoutPanel pFood = new FlowLayoutPanel();
                    pFood.AutoSize = true;
                    pFood.FlowDirection = FlowDirection.TopDown;
                    pFood.Width = 420;
                    pFood.BackColor = Color.White;
                    pFood.Margin = new Padding(0);
                    pFood.Padding = new Padding(10);

                    int mid = Convert.ToInt32(dt.Rows[i]["MainID"]);
                    ds1 = kitchen.AddProduct_into_Kitchen(mid);

                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        dt1 = ds1.Tables[0];

                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            Label lbFood = new Label();
                            lbFood.ForeColor = Color.Black;
                            lbFood.BackColor = Color.White;
                            lbFood.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                            lbFood.Margin = new Padding(8, 2, 3, 0);
                            lbFood.AutoSize = true;

                            string product = dt1.Rows[j]["pName"].ToString();
                            string qty = dt1.Rows[j]["qty"].ToString();
                            lbFood.Text = $"{product} {qty}";

                            pFood.Controls.Add(lbFood);
                        }
                    }

                    // Nút Complete
                    var b = new Guna.UI2.WinForms.Guna2Button();
                    b.Text = "Complete";
                    b.AutoRoundedCorners = true;
                    b.BorderRadius = 17;
                    b.Size = new Size(120, 35);
                    b.FillColor = Color.FromArgb(255, 102, 153); // màu hồng
                    b.ForeColor = Color.White;
                    b.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    b.Margin = new Padding(60, 10, 3, 5);
                    b.Tag = mid;
                    b.Click += new EventHandler(btnComplete_Click);

                    // Nút Edit
                    var btnEdit = new Guna.UI2.WinForms.Guna2Button();
                    btnEdit.Text = "Edit";
                    btnEdit.AutoRoundedCorners = true;
                    btnEdit.BorderRadius = 17;
                    btnEdit.Size = new Size(120, 35);
                    btnEdit.FillColor = Color.FromArgb(102, 153, 255); // màu xanh dương
                    btnEdit.ForeColor = Color.White;
                    btnEdit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    btnEdit.Margin = new Padding(60, 5, 3, 10);
                    btnEdit.Tag = mid;
                    btnEdit.Click += new EventHandler(btnEdit_Click);

                    // Thêm các phần vào panel chính (theo thứ tự dọc)
                    p1.Controls.Add(p2);    // thông tin
                    p1.Controls.Add(pFood); // danh sách món
                    p1.Controls.Add(b);     // nút complete
                    p1.Controls.Add(btnEdit); // nút edit

                    // Thêm panel đơn hàng vào giao diện chính
                    flowLayoutPanel1.Controls.Add(p1);
                }
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Ép kiểu sender về button
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;

            if (btn != null && btn.Tag != null)
            {
                int mainID = Convert.ToInt32(btn.Tag);

                frmPos frmPos = new frmPos();
                frmPos.MainID = mainID;
                frmPos.LoadEntries();
                frmPos.edit = true;

                Table table = new Table();
                int tableID = table.GetIDtable_from_tblMain( mainID);
                frmTableSelect frmTableSelect = new frmTableSelect();
                frmTableSelect.Update_Table_Status(tableID, "Available");

                // Hiển thị form chỉnh sửa đơn hàng
                DialogResult result = frmPos.ShowDialog();

                // Nếu người dùng đã chỉnh sửa xong -> load lại danh sách
                if (result == DialogResult.OK)
                {
                    GetOrder(); // LoadMain lại giao diện danh sách
                }
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());

            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

            if (guna2MessageDialog1.Show("Completed ?") == DialogResult.Yes)
            {
                string err = "";
                Kitchen kitchen = new Kitchen();
                bool result = kitchen.UpdateStatus(id, "Complete", ref err);

                Table table = new Table();
                int tableID = table.GetIDtable_from_tblMain(id);
                frmTableSelect frmTableSelect = new frmTableSelect();
                frmTableSelect.Update_Table_Status(tableID, "Available");
            }
            GetOrder();
        }
    }
}
  

