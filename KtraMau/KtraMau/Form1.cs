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

namespace KtraMau
{
    public partial class Form1 : Form
    {
        ProccessDatabase pd = new ProccessDatabase();
        private string appDirectory;
        string imageDirectory = "images"; 
        string imagepath;

        public Form1()
        {
            InitializeComponent();
            appDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        }

        private void LoadDL()
        {
            dgvNhanVien.DataSource = pd.DocBang("Select * From tblNhanVien");
        }

        private void Reset()
        {
            txtMaNV.Text = txtMucLuong.Text = txtSoDT.Text = txtTenNV.Text = cbPhongBan.Text = "";
            rdbNam.Checked = rdbNu.Checked = false;
            pbAnh = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        // thêm
        private bool KiemTraDL(string value)
        {
            return int.TryParse(value, out _);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "" || txtMucLuong.Text == "" || txtSoDT.Text == "" 
                || txtTenNV.Text == "" || cbPhongBan.Text == "" || (rdbNam.Checked == false && rdbNu.Checked == false))
            {
                MessageBox.Show("Hãy nhập đủ dữ liệu !", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            else
            {
                DataTable dt = pd.DocBang("Select * From tblNhanVien Where MaNV = N'" + txtMaNV.Text + "'");
                if(dt.Rows.Count > 0)
                {
                    MessageBox.Show("Mã nhân viên đã tồn tại !", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                else
                {
                    if (!KiemTraDL(txtMucLuong.Text))
                    {
                        MessageBox.Show("Mức lương là 1 số !", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string gt;
                        if (rdbNam.Checked == true)
                        {
                            gt = "Nam";
                        }
                        else
                        {
                            gt = "Nữ";
                        }
                        string imagePath = Path.Combine(imageDirectory, Path.GetFileName(imagepath));
                        string sql = "Insert into tblNhanVien Values(@manv, @tennv, @sodt, @gioitinh, @phongban, @mucluong, @anh)";
                        SqlParameter[] parameter = new SqlParameter[]
                        {
                            new SqlParameter("@manv", txtMaNV.Text),
                            new SqlParameter("@tennv", txtTenNV.Text),
                            new SqlParameter("@sodt", txtSoDT.Text),
                            new SqlParameter("@gioitinh", gt),
                            new SqlParameter("@phongban", cbPhongBan.Text),
                            new SqlParameter("@mucluong", txtMucLuong.Text),
                            new SqlParameter("@anh", imagePath)
                        };
                        pd.CapNhatTS(sql, parameter);

                        MessageBox.Show("Thêm mới thành công !", "Thông báo", MessageBoxButtons.OK);

                        LoadDL();
                    }
                }
            }
        }

        // sửa
        private void button3_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có muốn sửa không ?", "Thông báo", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string gt;
                if (rdbNam.Checked == true)
                {
                    gt = "Nam";
                }
                else
                {
                    gt = "Nữ";
                }
                string imagePath = Path.Combine(imageDirectory, Path.GetFileName(imagepath));
                string sql = "Update tblNhanVien Set TenNV = @tennv, SoDT = @sodt, GioiTinh = @gioitinh, PhongBan = @phongban, " +
                    "MucLuong = @mucluong, Anh = @anh Where MaNV = @manv";
                SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@manv", txtMaNV.Text),
                    new SqlParameter("@tennv", txtTenNV.Text),
                    new SqlParameter("@sodt", txtSoDT.Text),
                    new SqlParameter("@gioitinh", gt),
                    new SqlParameter("@phongban", cbPhongBan.Text),
                    new SqlParameter("@mucluong", txtMucLuong.Text),
                    new SqlParameter("@anh", imagePath)
                };
                pd.CapNhatTS(sql, parameter);

                MessageBox.Show("Cập nhật thành công !", "Thông báo", MessageBoxButtons.OK);

                LoadDL();
            }
        }

        // xóa
        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa không ?", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "Delete tblNhanVien Where MaNV = @manv";
                SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@manv", txtMaNV.Text)
                };
                pd.CapNhatTS(sql, parameter);

                MessageBox.Show("Xóa dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK);

                LoadDL();
            }
        }

        // thoát
        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void cbPhongBan_DropDown(object sender, EventArgs e)
        {
            cbPhongBan.Items.Clear();
            cbPhongBan.Items.Add("Thu ngân");
            cbPhongBan.Items.Add("Bảo vệ");
            cbPhongBan.Items.Add("Kế toán");
        }

        private void btnAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagepath = openFileDialog.FileName;
                pbAnh.ImageLocation = imagepath;
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNV.Text = dgvNhanVien.CurrentRow.Cells[0].Value.ToString();
            txtTenNV.Text = dgvNhanVien.CurrentRow.Cells[1].Value.ToString();
            txtSoDT.Text = dgvNhanVien.CurrentRow.Cells[2].Value.ToString();
            cbPhongBan.Text = dgvNhanVien.CurrentRow.Cells[4].Value.ToString();
            txtMucLuong.Text = dgvNhanVien.CurrentRow.Cells[5].Value.ToString();

            string gioiTinh = dgvNhanVien.CurrentRow.Cells[3].Value.ToString();

            if (gioiTinh == "Nam")
            {
                rdbNam.Checked = true;
                rdbNu.Checked = false;
            }
            else
            {
                rdbNam.Checked = false;
                rdbNu.Checked = true;
            }

            string imagePath = dgvNhanVien.CurrentRow.Cells[6].Value.ToString();

            DisplayImage(imagePath);
        }

        private void DisplayImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                // Chuyển đường dẫn thành byte array
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                // Chuyển byte array thành hình ảnh
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    pbAnh.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pbAnh.Image = null;
            }
        }
    }
}
