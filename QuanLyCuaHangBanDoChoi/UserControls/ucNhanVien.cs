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
using System.IO;
using QuanLyCuaHangBanDoChoi.Forms;
using DTO;
using DataAccessLayer;

namespace QuanLyCuaHangBanDoChoi.UserControls
{
    public partial class ucNhanVien : UserControl
    {
        public ucNhanVien()
        {
            InitializeComponent();
        }

        private void ucNhanVien_Load(object sender, EventArgs e)
        {
            if (frmDangNhap.Quyen == "Admin")
            {
                LoadCboLoaiNV();
                LoadCboLocLoaiNV();
                LoadDataGridViewTheoBoLoc();
                cboGioiTinh.SelectedIndex = 0;
                btnCapNhat.Enabled = false;
                btnSaThai.Enabled = false;
            }
            else
            {
                txtTenTK.Enabled = false;
                dgvNhanVien.DataSource = NhanVienBL.GetInstance.GetDanhSachNhanVienTheoMa(frmDangNhap.MaNV);
                panelBoLoc.Enabled = false;
                btnSaThai.Enabled = false;
                btnThem.Enabled = false;
                LoadCboLoaiNV();
                cboLoai.Enabled = false;
                cboGioiTinh.SelectedIndex = 0;
                txtTen.Enabled = false;
                cboGioiTinh.Enabled = false;
                btnSaThai.Enabled = false;
                dateTuyenDung.Enabled = false;
            }
        }

        private void LoadDataGridViewTheoBoLoc()
        {
            dgvNhanVien.DataSource = NhanVienBL.GetInstance.GetDanhSachNhanVienTheoBoLoc(txtTenNV.Text.Trim(), cboLocLoaiNhanVien.SelectedValue.ToString().Trim());
            dgvNhanVien.ClearSelection();
        }

        private void LoadCboLocLoaiNV()
        {
            DataTable dt = LoaiNhanVienBL.GetInstance.GetDanhSachLoaiNhanVien();
            DataRow dr = dt.NewRow();
            dr["Mã CV"] = "";
            dr["Tên CV"] = "Tất cả";
            dt.Rows.Add(dr);
            cboLocLoaiNhanVien.DataSource = dt;
            cboLocLoaiNhanVien.DisplayMember = "Tên CV";
            cboLocLoaiNhanVien.ValueMember = "Mã CV";
            cboLocLoaiNhanVien.SelectedIndex = cboLocLoaiNhanVien.Items.Count - 1;
        }

        private void LoadCboGioiTinh()
        {
        }
        private void LoadCboLoaiNV()
        {
            cboLoai.DataSource = LoaiNhanVienBL.GetInstance.GetDanhSachLoaiNhanVien();
            cboLoai.DisplayMember = "Tên CV";
            cboLoai.ValueMember = "Mã CV";
        }
        string manv = "";
        private void dgvNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNhanVien.SelectedRows.Count == 1)
                {
                    btnCapNhat.Enabled = true;
                    btnSaThai.Enabled = true;
                    txtTenTK.Enabled = false;
                    if (frmDangNhap.Quyen != "Admin")
                    {
                        btnSaThai.Enabled = false;
                    }

                    ResetColorControls();
                    DataGridViewRow dr = dgvNhanVien.SelectedRows[0];
                    manv = dr.Cells["MaNV"].Value.ToString().Trim();
                    txtTen.Text = dr.Cells["HoTen"].Value.ToString().Trim();
                    cboLoai.SelectedValue = dr.Cells["MaCV"].Value.ToString().Trim();
                    cboGioiTinh.SelectedItem = dr.Cells["GioiTinh"].Value.ToString();
                    txtDiaChi.Text = dr.Cells["DiaChi"].Value.ToString().Trim();
                    txtSoDienThoai.Text = dr.Cells["Sdt"].Value.ToString().Trim();
                    dateNgaySinh.Value = Convert.ToDateTime(dr.Cells["NgaySinh"].Value);
                    dateTuyenDung.Value = Convert.ToDateTime(dr.Cells["NgayTuyenDung"].Value);
                    txtTenTK.Text = dr.Cells["TenTK"].Value.ToString().Trim();

                    MemoryStream ms = new MemoryStream((byte[])dgvNhanVien.CurrentRow.Cells["HinhAnh"].Value‌​);
                    if(ms!=null)
                        picHinhAnh.Image = Image.FromStream(ms);
                }

            }
            catch (Exception)
            {
                return;
            }
        }

        private void ResetColorControls()
        {
            foreach (Control ctrl in pnlThongTinNhanVien.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.BackColor == Color.OrangeRed)
                    {
                        ctrl.BackColor = Color.White;
                    }
                }
            }
            if (picHinhAnh.BackColor == Color.OrangeRed)
            {
                picHinhAnh.BackColor = Color.White;
            }
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            CheckControls();
            
                if (txtTen.Text.Length < 50)
                {
                    if (txtSoDienThoai.Text.Length <= 12 && txtSoDienThoai.Text.Length >= 10)
                    {
                        if (CheckDate())
                        {
                            NhanVienDTO nvDTO = new NhanVienDTO();
                            nvDTO.tennv = txtTen.Text.ToString().Trim();
                            nvDTO.macv = cboLoai.SelectedValue.ToString().Trim();
                            nvDTO.gioitinh = cboGioiTinh.Text.ToString().Trim();
                            nvDTO.ngaysinh = dateNgaySinh.Value;
                            nvDTO.ngaytuyendung = dateTuyenDung.Value;
                            nvDTO.diachi = txtDiaChi.Text.ToString().Trim();
                            nvDTO.sdt = txtSoDienThoai.Text.ToString().Trim();
                            nvDTO.tentk = txtTenTK.Text.ToString().Trim();
                            nvDTO.trangthai = "Đang làm";
                            cboLocLoaiNhanVien.SelectedIndex = cboLoai.SelectedIndex;
                            Image img = picHinhAnh.Image;
                            if (img == null)
                            {
                                nvDTO.hinhanh = null;
                            }
                            else
                            {
                                nvDTO.hinhanh = ImageToByteArray(img);
                            }
                            //nvDTO.hinhanh = ImageToByteArray(img);
                            if (NhanVienBL.GetInstance.ThemNhanVien(nvDTO))
                            {
                                this.Alert("Thêm nhân viên thành công...", frmPopupNotification.enmType.Success);
                                if (txtTenTK.Text != "")
                                {
                                    AlterLogin.GetInstance.GanquyenLogin(txtTenTK.Text.ToString().Trim());
                                }
                                LamMoi();
                                LoadCboLocLoaiNV();
                                LoadDataGridViewTheoBoLoc();
                                    
                            }
                        }
                        else
                        {
                            frmThongBao frm = new frmThongBao();
                            frm.lblThongBao.Text = "Mã nhân viên đã tồn tại";
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        frmThongBao frm = new frmThongBao();
                        frm.lblThongBao.Text = "Số điện thoại phải từ 10 đến 12 số!";
                        frm.ShowDialog();
                    }
                }
                else
                {
                    frmThongBao frm = new frmThongBao();
                    frm.lblThongBao.Text = "Họ tên chỉ được tối đa 50 ký tự!";
                    frm.ShowDialog();
                }
        }

        private void LamMoi()
        {
            txtTenTK.Enabled = true;
            btnCapNhat.Enabled = false;
            btnSaThai.Enabled = false;
            txtTen.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();
            txtTenTK.Clear();
            cboGioiTinh.SelectedIndex = 0;
            if (cboLoai.Items.Count > 0)
                cboLoai.SelectedIndex = 0;
            dateNgaySinh.Value = DateTime.Now;
            picHinhAnh.Image = null;
            ResetColorControls();
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private bool CheckDate()
        {
            if (dateNgaySinh.Value >= DateTime.Now)
            {
                return false;
            }
            return true;
        }

        private void CheckControls()
        {
            foreach (Control ctrl in pnlThongTinNhanVien.Controls)
            {
                if (ctrl is TextBox && ctrl != txtTenTK)
                {
                    if (ctrl.Text == "")
                    {
                        ctrl.BackColor = Color.OrangeRed;
                    }
                }
            }
            if (picHinhAnh.Image == null)
            {
                picHinhAnh.BackColor = Color.OrangeRed;
            }

        }

        private void picHinhAnh_Click(object sender, EventArgs e)
        {
            if (picHinhAnh.BackColor == Color.OrangeRed)
            {
                picHinhAnh.BackColor = Color.White;
            }
            frmLoadImage frm = new frmLoadImage();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                picHinhAnh.Image = frm.img;
            }
        }

        private void btnLamMoiThongTin_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnThoiViec_Click(object sender, EventArgs e)
        {
            if (NhanVienBL.GetInstance.ThoiViecNhanVien(manv))
            {
                LamMoi();
                LoadDataGridViewTheoBoLoc();
                this.Alert("Sa thải nhân viên thành công...", frmPopupNotification.enmType.Success);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            CheckControls();
            
                if (txtTen.Text.Length < 50)
                {
                    if (txtSoDienThoai.Text.Length >= 10 && txtSoDienThoai.Text.Length <= 12)
                    {
                        if (CheckDate())
                        {
                            NhanVienDTO nvDTO = new NhanVienDTO();
                            nvDTO.manv = manv;
                            nvDTO.tennv = txtTen.Text;
                            nvDTO.macv = cboLoai.SelectedValue.ToString().Trim();
                            nvDTO.macv = cboLoai.SelectedValue.ToString().Trim();
                            nvDTO.gioitinh = cboGioiTinh.Text;
                            nvDTO.ngaysinh = dateNgaySinh.Value;
                            nvDTO.diachi = txtDiaChi.Text;
                            nvDTO.sdt = txtSoDienThoai.Text;
                            nvDTO.tentk = txtTenTK.Text;
                            nvDTO.trangthai = "Đang làm";
                            nvDTO.ngaytuyendung = dateTuyenDung.Value;
                            Image img = picHinhAnh.Image;
                            if (img == null)
                            {
                                nvDTO.hinhanh = null;
                            }
                            else
                            {
                                nvDTO.hinhanh = ImageToByteArray(img);
                            }
                            if (frmDangNhap.Quyen == "Admin")
                                cboLocLoaiNhanVien.SelectedIndex = cboLoai.SelectedIndex;

                            if (NhanVienBL.GetInstance.SuaThongTinNhanVien(nvDTO))
                            {
                                if (frmDangNhap.Quyen == "Admin")
                                {
                                    LoadCboLocLoaiNV();
                                    LoadDataGridViewTheoBoLoc();
                                }
                                else
                                {
                                    dgvNhanVien.DataSource = NhanVienBL.GetInstance.GetDanhSachNhanVienTheoMa(frmDangNhap.MaNV);
                                }    
                                LamMoi();

                                this.Alert("Cập nhật thành công...", frmPopupNotification.enmType.Success);
                            }
                        }
                        else
                        {
                            frmThongBao frm = new frmThongBao();
                            frm.lblThongBao.Text = "Ngày sinh không hợp lệ";
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        frmThongBao frm = new frmThongBao();
                        frm.lblThongBao.Text = "Số điện thoại phải từ 10 đến 12 số";
                        frm.ShowDialog();
                    }
                }
                else
                {
                    frmThongBao frm = new frmThongBao();
                    frm.lblThongBao.Text = "Họ tên chỉ được tối đa 50 ký tự!";
                    frm.ShowDialog();
                }
        }

        private void txtTen_Click(object sender, EventArgs e)
        {
            if (txtTen.BackColor == Color.OrangeRed)
            {
                txtTen.BackColor = Color.White;
            }
        }

        private void cboLoai_Click(object sender, EventArgs e)
        {
            if (cboLoai.BackColor == Color.OrangeRed)
            {
                cboLoai.BackColor = Color.White;
            }
        }

        private void cboGioiTinh_Click(object sender, EventArgs e)
        {
            if (cboGioiTinh.BackColor == Color.OrangeRed)
            {
                cboGioiTinh.BackColor = Color.White;
            }
        }

        private void txtDiaChi_Click(object sender, EventArgs e)
        {
            if (txtDiaChi.BackColor == Color.OrangeRed)
            {
                txtDiaChi.BackColor = Color.White;
            }
        }

        private void txtSoDienThoai_Click(object sender, EventArgs e)
        {
            if (txtSoDienThoai.BackColor == Color.OrangeRed)
            {
                txtSoDienThoai.BackColor = Color.White;
            }
        }

        private void btnApDung_Click(object sender, EventArgs e)
        {
            LoadDataGridViewTheoBoLoc();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTenNV.Text = "";
            cboLocLoaiNhanVien.SelectedIndex = cboLocLoaiNhanVien.Items.Count - 1;
        }
        public void Alert(string msg, frmPopupNotification.enmType type)
        {
            frmPopupNotification frm = new frmPopupNotification();
            frm.TopMost = true;
            frm.showAlert(msg, type);
        }

        private void cboLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            label5.Visible = true;
            txtTenTK.Visible = true;
            if (cboLoai.SelectedValue.ToString().Trim() == "CV0004")
            {
                label5.Visible = false;
                txtTenTK.Visible = false;
            }
        }
    }
}
