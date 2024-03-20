using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangBanDoChoi.Forms
{
    public partial class frmChonNCC : Form
    {
        public frmChonNCC()
        {
            InitializeComponent();
        }
        public string MANSX;
        public string TENNSX;
        private void frmChonNCC_Load(object sender, EventArgs e)
        {
            cboNCC.DataSource = NSXBL.GetInstance.GetDanhSachNSX();
            cboNCC.DisplayMember = "Tên Nhà Sản Xuất";
            cboNCC.ValueMember = "Mã Nhà Sản Xuất";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmThemNCC frm = new frmThemNCC();
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                cboNCC.DataSource = NSXBL.GetInstance.GetDanhSachNSX();
                cboNCC.SelectedIndex = cboNCC.Items.Count - 1;
            }
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            MANSX = cboNCC.SelectedValue.ToString();
            TENNSX = cboNCC.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
