using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogicLayer;
using DTO;
using QuanLyCuaHangBanDoChoi.Forms;
using System.Text.RegularExpressions;

namespace QuanLyCuaHangBanDoChoi.UserControls
{
    public partial class ucKhachHang : UserControl
    {
        string makh = "KH001";
        public ucKhachHang()
        {
            InitializeComponent();
        }

        private void ucKhachHang_Load(object sender, EventArgs e)
        {
            LoadDgvKhachHang();
            btnCapNhat.Enabled = false;
            cboGioiTinh.SelectedIndex = 0;
        }

        private void LoadDgvKhachHang()
        {
            dgvKhachHang.DataSource = KhachHangBL.GetInstance.GetDanhSachKhachHang();
            dgvKhachHang.ClearSelection();
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            KhachHangDTO khDTO = new KhachHangDTO();
            khDTO.tenkh = txtTen.Text;
            khDTO.sdt = txtSoDienThoai.Text;
            khDTO.gioitinh = cboGioiTinh.Text;
            if (KhachHangBL.GetInstance.ThemKhachHang(khDTO))
            {
                LoadDgvKhachHang();
                LamMoi();
                this.Alert("Thêm khách hàng thành công...", frmPopupNotification.enmType.Success);
            }
            else
            {
                this.Alert("Thêm khách hàng thất bại...", frmPopupNotification.enmType.Success);
            }
        }


        private void LamMoi()
        {
            txtTen.Clear();
            txtDoanhSo.Clear();
            txtSoDienThoai.Clear();
            cboGioiTinh.SelectedIndex = 0;
            dateNgayDangKy.Value = DateTime.Now;
            ResetColorControls();
        }

        private void ResetColorControls()
        {
            foreach (Control ctrl in pnlThongTinKhachHang.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.BackColor == Color.OrangeRed)
                    {
                        ctrl.BackColor = Color.White;
                    }
                }
            }
        }

        private bool CheckControls()
        {
            int r = 0;
            foreach (Control ctrl in pnlThongTinKhachHang.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.Text == "")
                    {
                        ctrl.BackColor = Color.OrangeRed;
                        r = 1;
                    }
                }
            }
            if (r == 0)
                return true;
            return false;
        }

        private void dateNgayDangKy_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                MoveToNextRowAndFillData();
            }
        }

        private void MoveToNextRowAndFillData()
        {
            int currentRowIndex = dgvKhachHang.CurrentCell.RowIndex;
            int nextRowIndex = (currentRowIndex == dgvKhachHang.Rows.Count - 1) ? 0 : currentRowIndex + 1;

            // Lấy dữ liệu từ hàng mới
            DataGridViewRow nextRow = dgvKhachHang.Rows[nextRowIndex];
            dgvKhachHang.CurrentCell = nextRow.Cells[0]; // Chuyển đến ô đầu tiên của hàng mới
            FillDataFromRow(nextRow);
        }

        private void FillDataFromRow(DataGridViewRow row)
        {
            makh = row.Cells["Mã KH"].Value.ToString().Trim();
            txtTen.Text = row.Cells["Tên KH"].Value.ToString().Trim();
            txtSoDienThoai.Text = row.Cells["SĐT"].Value.ToString().Trim();
            cboGioiTinh.SelectedItem = row.Cells["Giới Tính"].Value.ToString();
            dateNgayDangKy.Value = Convert.ToDateTime(row.Cells["Ngày Đăng Kí"].Value);
            txtDoanhSo.Text = ConvertTien(Convert.ToDouble(row.Cells["Doanh Số"].Value.ToString().Trim()));
        }
        private void dgvKhachHang_Click(object sender, EventArgs e)
        {
            btnCapNhat.Enabled = true;
            try
            {
                if (dgvKhachHang.SelectedRows.Count == 1)
                {
                    ResetColorControls();
                    DataGridViewRow dr = dgvKhachHang.SelectedRows[0];
                    makh = dr.Cells["Mã KH"].Value.ToString().Trim();
                    txtTen.Text = dr.Cells["Tên KH"].Value.ToString().Trim();
                    txtSoDienThoai.Text = dr.Cells["SĐT"].Value.ToString().Trim();
                    cboGioiTinh.SelectedItem = dr.Cells["Giới Tính"].Value.ToString();
                    dateNgayDangKy.Value = Convert.ToDateTime(dr.Cells["Ngày Đăng Kí"].Value);
                    txtDoanhSo.Text = ConvertTien(Convert.ToDouble(dr.Cells["Doanh Số"].Value.ToString().Trim()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }
        }
        private string ConvertTien(double gia)
        {
            string giaban = gia.ToString();
            string result = "";
            int d = 0;
            for (int i = giaban.Length - 1; i >= 0; i--)
            {
                d++;
                result += giaban[i];
                if (d == 3 && i != 0)
                {
                    result += ',';
                    d = 0;
                }
            }
            char[] charArray = result.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            KhachHangDTO khDTO = new KhachHangDTO();
            khDTO.makh = makh;
            khDTO.tenkh = txtTen.Text;
            khDTO.gioitinh = cboGioiTinh.Text;
            khDTO.ngaydangky = dateNgayDangKy.Value;
            khDTO.sdt = txtSoDienThoai.Text;
            khDTO.doanhso = int.Parse(txtDoanhSo.Text);

            if (KhachHangBL.GetInstance.SuaThongTinKhachHang(khDTO))
                {
                    LoadDgvKhachHang();
                    LamMoi();
                    this.Alert("Cập nhât thành công...", frmPopupNotification.enmType.Success);
                }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (KhachHangBL.GetInstance.XoaKhachHang(makh.ToString()))
            {
                LamMoi();
                LoadDgvKhachHang();
                this.Alert("Xóa thành công...", frmPopupNotification.enmType.Success);
            }
            else
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Xóa khách hàng thất bại!";
                frm.ShowDialog();
            }
        }

        private void btnLamMoiThongTin_Click(object sender, EventArgs e)
        {
            txtTen.Focus();
            dgvKhachHang.CurrentCell = dgvKhachHang.Rows[0].Cells[0];
            btnCapNhat.Enabled = false;
            LamMoi();
        }

        private void txtTenNV_TextChanged(object sender, EventArgs e)
        {
            if (txtTenKH.Text != "")
            {
                dgvKhachHang.DataSource = KhachHangBL.GetInstance.GetDanhSachKhachHangTimKiem(txtTenKH.Text);
                dgvKhachHang.ClearSelection();
            }
            else
            {
                LoadDgvKhachHang();
            }
        }
        public void Alert(string msg, frmPopupNotification.enmType type)
        {
            frmPopupNotification frm = new frmPopupNotification();
            frm.TopMost = true;
            frm.showAlert(msg, type);
        }
    }
}
