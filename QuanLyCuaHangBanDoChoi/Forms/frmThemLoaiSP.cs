﻿
using BusinessLogicLayer;
using DTO;
using System;
using System.Windows.Forms;

namespace QuanLyCuaHangBanDoChoi.Forms
{
    public partial class frmThemLoaiSP : Form
    {
        public frmThemLoaiSP()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (txtTenLoaiSP.Text == "")
            {
                frmThongBao frm = new frmThongBao();
                frm.lblThongBao.Text = "Bạn chưa nhập tên loại sản phẩm!";
                frm.ShowDialog();
                return;
            }
            LoaiSanPhamNew lsp = new LoaiSanPhamNew();
            lsp.TenLoaiSP = txtTenLoaiSP.Text;
            if (LoaiSanPhamBL.GetInstance.ThemLoaiSanPham(lsp))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
