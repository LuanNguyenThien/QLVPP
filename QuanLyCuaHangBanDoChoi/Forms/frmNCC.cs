using BusinessLogicLayer;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCuaHangBanDoChoi.Forms
{
    public partial class frmNCC : Form
    {
        public frmNCC()
        {
            InitializeComponent();
        }
        public bool b = false;
        private void btnThem_Click(object sender, EventArgs e)
        {
            int k = 0;
            if (txtTen.Text == "")
            {
                txtTen.BackColor = Color.OrangeRed;
                k = 1;
            }
            if (txtDiaChi.Text == "")
            {
                txtDiaChi.BackColor = Color.OrangeRed;
                k = 1;
            }
            if (txtSDT.Text == "")
            {
                txtSDT.BackColor = Color.OrangeRed;
                k = 1;
            }
            if (k == 1)
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa nhập đủ thông tin nhà cung cấp";
                frm.ShowDialog();
                return;
            }
            if (txtTen.Text.Length < 50)
            {
                if (txtDiaChi.Text.Length <= 200)
                {
                    if (txtSDT.Text.Length == 8 || txtSDT.Text.Length == 10)
                    {
                        NhaSanXuatNew nsx = new NhaSanXuatNew();
                        nsx.TenNSX = txtTen.Text;
                        nsx.DiaChi = txtDiaChi.Text;
                        nsx.Sdt = txtSDT.Text;
                        if (NSXBL.GetInstance.ThemNSX(nsx))
                        {
                            this.Alert("Thêm thành công...", frmPopupNotification.enmType.Success);
                            LoadDataGridView();
                            txtDiaChi.Text = "";
                            txtSDT.Text = "";
                            txtTen.Text = "";
                            b = true;
                        }
                    }
                    else
                    {
                        frmThongBao frm = new frmThongBao();
                        frm.lblThongBao.Text = "Số điện thoại phải chứa 8 hoặc 10 số!";
                        frm.ShowDialog();
                    }
                }
                else
                {
                    frmThongBao frm = new frmThongBao();
                    frm.lblThongBao.Text = "Địa chỉ tối đa 200 ký tự!";
                    frm.ShowDialog();
                }
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Tên Nhà sản xuất chỉ được tối đa 50 ký tự!";
                frm.ShowDialog();
            }
        }
        public void Alert(string msg, frmPopupNotification.enmType type)
        {
            frmPopupNotification frm = new frmPopupNotification();
            frm.TopMost = true;
            frm.showAlert(msg, type);
        }

        private void LoadDataGridView()
        {
            dgvNCC.DataSource = NSXBL.GetInstance.GetDanhSachNSX();
        }
        string mancc = "NSX0001";
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            int k = 0;
            if (txtTen.Text == "")
            {
                txtTen.BackColor = Color.OrangeRed;
                k = 1;
            }
            if (txtDiaChi.Text == "")
            {
                txtDiaChi.BackColor = Color.OrangeRed;
                k = 1;
            }
            if (txtSDT.Text == "")
            {
                txtSDT.BackColor = Color.OrangeRed;
                k = 1;
            }
            if (k == 1)
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa nhập đủ thông tin nhà cung cấp";
                frm.ShowDialog();
                return;
            }
            if (txtTen.Text.Length < 50)
            {
                if (txtDiaChi.Text.Length <= 200)
                {
                    if (txtSDT.Text.Length == 8 || txtSDT.Text.Length == 10)
                    {
                        NhaSanXuatNew nsx = new NhaSanXuatNew();
                        //nccDTO.MaNCC = mancc;
                        nsx.MaNSX = mancc;
                        nsx.TenNSX = txtTen.Text;
                        nsx.DiaChi = txtDiaChi.Text;
                        nsx.Sdt = txtSDT.Text;
                        if (NSXBL.GetInstance.CapNhatNSX(nsx))
                        {
                            this.Alert("Đã cập nhật thành công...", frmPopupNotification.enmType.Success);
                            LoadDataGridView();
                            txtDiaChi.Text = "";
                            txtSDT.Text = "";
                            txtTen.Text = "";
                            b = true;
                        }
                    }
                    else
                    {
                        frmThongBao frm = new frmThongBao();
                        frm.lblThongBao.Text = "Số điện thoại phải chứa 8 hoặc 10 số!";
                        frm.ShowDialog();
                    }
                }
                else
                {
                    frmThongBao frm = new frmThongBao();
                    frm.lblThongBao.Text = "Địa chỉ tối đa 200 ký tự!";
                    frm.ShowDialog();
                }
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Tên NCC chỉ được tối đa 50 ký tự!";
                frm.ShowDialog();
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {}

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvNCC_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNCC.SelectedRows.Count == 1)
                {
                    if (txtTen.BackColor == Color.OrangeRed)
                    {
                        txtTen.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    if (txtDiaChi.BackColor == Color.OrangeRed)
                    {
                        txtDiaChi.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    if (txtSDT.BackColor == Color.OrangeRed)
                    {
                        txtSDT.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    DataGridViewRow dr = dgvNCC.SelectedRows[0];
                    mancc = dr.Cells["Mã Nhà Sản Xuất"].Value.ToString().Trim();
                    txtTen.Text = dr.Cells["Tên Nhà Sản Xuất"].Value.ToString().Trim();
                    txtDiaChi.Text = dr.Cells["Địa Chỉ"].Value.ToString().Trim();
                    txtSDT.Text = dr.Cells["Số Điện Thoại"].Value.ToString().Trim();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void frmNCC_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
        }
    }
}
