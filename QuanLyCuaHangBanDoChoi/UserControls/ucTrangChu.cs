using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;
using DTO;

namespace QuanLyCuaHangBanDoChoi.UserControls
{
    public partial class ucTrangChu : UserControl
    {
        public ucTrangChu()
        {
            InitializeComponent();
        }
        private void ucTrangChu_Load(object sender, EventArgs e)
        {
            LoadData();
            cboDoanhThu.Visible = false;
            cboTopSanPham1.Visible = false;
            cboTopSanPham2.Visible = false;
            btnRefresh.PerformClick();
        }
        private string Convert(double gia)
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
                    result += '.';
                    d = 0;
                }
            }
            char[] charArray = result.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        private void LoadData()
        {
            Cursor = Cursors.AppStarting;
            lbSanPhanDaBan.Text = TrangChuBL.GetInstance.GetTongSanPhamDaBan().ToString();
            lbTongDoanhThu.Text = Convert(TrangChuBL.GetInstance.GetTongDoanhThu()) + " ₫";
            lbTongKhachHang.Text = TrangChuBL.GetInstance.GetTongKhachHang().ToString();
            loadChartDoanhThu();
            loadChartTopSP();
            Cursor = Cursors.Default;


        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void loadChartTopSP()
        {
            chartTopSP.Series.Clear();
            chartTopSP.Series.Add("Top 10 Sản Phẩm");
            chartTopSP.Series["Top 10 Sản Phẩm"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            chartTopSP.Series["Top 10 Sản Phẩm"].Font = new Font("UTM Avo", 12, FontStyle.Bold);
            chartTopSP.Series["Top 10 Sản Phẩm"].LabelForeColor = Color.White;
            chartTopSP.Series["Top 10 Sản Phẩm"].BorderColor = Color.White;
            chartTopSP.Series["Top 10 Sản Phẩm"].BorderWidth = 2;

            List<SanPhamNew> lstSP = TrangChuBL.GetInstance.GetTop10SPTheoSoLuongThangTruoc();
            int i = 0;
            foreach (SanPhamNew spDTO in lstSP)
            {
                chartTopSP.Series["Top 10 Sản Phẩm"].Points.Add(spDTO.SoLuong);
                chartTopSP.Series["Top 10 Sản Phẩm"].Points[i].AxisLabel = spDTO.MaSP;
                chartTopSP.Series["Top 10 Sản Phẩm"].Points[i].LegendText = spDTO.TenSP;
                chartTopSP.Series["Top 10 Sản Phẩm"].Points[i].Label = spDTO.SoLuong + "";
                i++;
            }
        }

        private void loadChartDoanhThu()
        {
            chartDoanhThu.Series.Clear();
            chartDoanhThu.Series.Add("Doanh Thu");
            chartDoanhThu.Series["Doanh Thu"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            chartDoanhThu.Series["Doanh Thu"].Font = new Font("UTM Avo", 10, FontStyle.Bold);
            chartDoanhThu.Series["Doanh Thu"].BorderColor = Color.Orange;
            chartDoanhThu.Series["Doanh Thu"].BorderWidth = 2;

            List<DoanhThuDTO> lstdt = new List<DoanhThuDTO>();
            DoanhThuDTO dtDTO;
            lstdt = TrangChuBL.GetInstance.GetDoanhThuThangTruoc();
            if (lstdt.Count > 0)
            {
                int n = 0;
                for (int l = lstdt.Count - 1; l >= 0; l--)
                {
                    dtDTO = lstdt[l];
                    chartDoanhThu.Series["Doanh Thu"].Points.Add(dtDTO.doanhthu);
                    chartDoanhThu.Series["Doanh Thu"].Points[n].AxisLabel = dtDTO.ngay.ToShortDateString();
                    chartDoanhThu.Series["Doanh Thu"].Points[n].LegendText = dtDTO.ngay.ToShortDateString();
                    chartDoanhThu.Series["Doanh Thu"].Points[n].LabelForeColor = Color.OrangeRed;
                    chartDoanhThu.Series["Doanh Thu"].Points[n].Label = Convert(dtDTO.doanhthu).ToString() + " ₫";
                    n++;
                }
            }
            else
            {
                chartDoanhThu.Series["Doanh Thu"].Points.Add(0);
                chartDoanhThu.Series["Doanh Thu"].Points[0].LabelForeColor = Color.OrangeRed;
                chartDoanhThu.Series["Doanh Thu"].Points[0].Label = "0 ₫";
            }
        }

        private void cboDoanhThu_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void cboTopSanPham1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void lblTopSP_Click(object sender, EventArgs e)
        {

        }

        private void cboTopSanPham2_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboDoanhThu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

