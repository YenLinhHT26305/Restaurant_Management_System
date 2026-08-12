using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.BS_layer;

namespace Restaurant_Management_System.Interface.View
{
    public partial class frmProductAdd : Form
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }
        string err;
        string filePath;
        byte[] imageByteArray;
        public bool add;
        public int cID; // ID của sản phẩm
        public int categoryID; // ID của danh mục (CategoryID)

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Chọn hình ảnh sản phẩm";
            ofd.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                txtImage.Image = new Bitmap(filePath);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.");
                return;
            }

            if (!int.TryParse(txtPrice.Text.Trim(), out int price) || price <= 0)
            {
                MessageBox.Show("Price must be a positive number.");
                return;
            }


            if (cmbCate.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            // Chuyển ảnh sang byte[]
            byte[] img = null;
            if (txtImage.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    txtImage.Image.Save(ms, txtImage.Image.RawFormat);
                    img = ms.ToArray();
                }
            }

            Product pro = new Product();
            int cateID = cmbCate.Enabled ? Convert.ToInt32(cmbCate.SelectedValue) : -1;
            if (!this.add)  // đang sửa
            {
                pro.UpdateProduct( cID,txtName.Text.Trim(), price,
                    cateID, img, ref err);

                if (string.IsNullOrEmpty(err))
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
                pro.AddProduct(txtName.Text.Trim(), price,
                    cateID, img, ref err);

                if (string.IsNullOrEmpty(err))  // Kiểm tra nếu không có lỗi
                {
                    MessageBox.Show("Add a new product successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error: " + err);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            Product pro = new Product();
            pro.LoadProducts_forProAdd(cmbCate);
        }
    }
}
