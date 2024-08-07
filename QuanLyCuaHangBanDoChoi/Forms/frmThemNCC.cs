﻿
using BusinessLogicLayer;
using DTO;
using System;
using System.Windows.Forms;

namespace QuanLyCuaHangBanDoChoi.Forms
{
    public partial class frmThemNCC : Form
    {
        public frmThemNCC()
        {
            InitializeComponent();
        }

        private void btnThemNCC_Click(object sender, EventArgs e)
        {
            if (txtTenNCC.Text == "")
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa nhập tên nhà cung cấp!";
                frm.ShowDialog();
                return;
            }
            NhaSanXuatNew nsx = new NhaSanXuatNew();
            nsx.TenNSX = txtTenNCC.Text;
            nsx.DiaChi = null;
            nsx.Sdt = null;
            if (NSXBL.GetInstance.ThemNSX(nsx))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
