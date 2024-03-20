using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public class SanPhamDL
    {
        private static SanPhamDL Instance;
        public static SanPhamDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new SanPhamDL();
                }
                return Instance;
            }
        }
        private SanPhamDL() { }

        #region Lấy Tổng Sản Phẩm Đã Bán
        public int GetTongSanPhamDaBan()
        {
            string sql = "SELECT SUM(SOLUONG) FROM CTHD";
            DataTable dt = new DataTable();
            dt = DataProvider.GetTable(sql);
            int sl = int.Parse(dt.Rows[0][0].ToString());
            return sl;
        }
        #endregion

        #region Lấy Mã Sản Phẩm Max
        public int GetMaSPMax()
        {
            try
            {
                string sql = "SELECT MAX(MASP) FROM SANPHAM";
                DataTable dt = new DataTable();
                dt = DataProvider.GetTable(sql);
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Lấy Tên Sản Phẩm
        public string GetTenSP(int MASP)
        {
            try
            {
                string sql = "SELECT TENSP FROM SANPHAM WHERE MASP="+ MASP;
                DataTable dt = new DataTable();
                dt = DataProvider.GetTable(sql);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        #region Kiểm Tra Mã Sản Phẩm
        public bool CheckMaSP(string MASP)
        {
            try
            {
                string sql = "SELECT * FROM SANPHAM WHERE MASP='"+MASP+"'";
                DataTable dt = new DataTable();
                dt = DataProvider.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Lấy Danh Sách Sản Phẩm
        public DataTable GetDanhSachSanPham()
        {
            try
            {
                string sql = "Select * from viewSanPham";
                DataTable dt = new DataTable();
                dt = DataProvider.GetTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Lấy Danh Sách Sản Phẩm Theo NCC
        public DataTable GetDanhSachSanPhamTheoNSX(string TENNSX)
        {
            try
            {
                string sql = "Select * from viewSanPham Where [Tên Nhà Sản Xuất] = N'" + TENNSX + "'";
                DataTable dt = new DataTable();
                dt = DataProvider.GetTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Lấy Danh Sách Sản Phẩm Theo Bộ Lọc
        public DataTable GetDanhSachSanPhamTheoBoLoc(string TENSP,string TENLOAISP,string TENNCC)
        {
            try
            {
                DataTable dt=new DataTable();
                string sql = "";
                if (TENSP == "")
                    TENSP = "NULL";
                else
                    TENSP = "N'"+TENSP.Trim()+"'";
                if(TENLOAISP == "Tất cả")
                    TENLOAISP = "NULL";
                else
                    TENLOAISP = "N'"+TENLOAISP.Trim()+"'";
                if(TENNCC == "Tất cả")
                    TENNCC = "NULL";
                else
                    TENNCC = "N'"+TENNCC.Trim()+"'";
                sql = "EXEC GetSPtheoBoLoc " + TENNCC + ", " + TENLOAISP + ", " + TENSP;
                dt = new DataTable();
                dt = DataProvider.GetTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Ngừng Kinh Doanh Sản Phẩm
        public bool NgungKinhDoanhSanPham(string MASP)
        {
            try
            {
                string sql = "EXEC DoiTrangThaiSanPham @MaSP";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@MaSP", MASP);
                cmd.Connection = con;
                int rows = cmd.ExecuteNonQuery();
                DataProvider.Disconnect(con);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Thêm Sản Phẩm
        public bool ThemSanPham(SanPhamNew sp, NhaSanXuatNew nsx, LoaiSanPhamNew lsp)
        {
            try
            {
                string sql = "EXEC ThemSPMOI @TenSP, @MaLoaiSP, @TenLoaiSP, @TenNSX, @DonViTinh, @GiaTien, @GiaNhap, @SoLuong, @LoiNhuan, @KhuyenMai, @TinhTrang, @HinhAnh";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                cmd.Parameters.AddWithValue("@MaLoaiSP", sp.MaLoaiSP);
                cmd.Parameters.AddWithValue("@TenLoaiSP", lsp.TenLoaiSP);
                cmd.Parameters.AddWithValue("@TenNSX", nsx.TenNSX);
                cmd.Parameters.AddWithValue("@DonViTinh", sp.DonViTinh);
                cmd.Parameters.AddWithValue("@GiaTien", sp.GiaTien);
                cmd.Parameters.AddWithValue("@GiaNhap", sp.GiaNhap);
                cmd.Parameters.AddWithValue("@SoLuong", sp.SoLuong);
                cmd.Parameters.AddWithValue("@LoiNhuan", sp.LoiNhuan);
                cmd.Parameters.AddWithValue("@KhuyenMai", sp.KhuyenMai);
                cmd.Parameters.AddWithValue("@TinhTrang", sp.TinhTrang);
                cmd.Parameters.AddWithValue("@HinhAnh", sp.HinhAnh);
                cmd.Connection = con;
                int rows = cmd.ExecuteNonQuery();
                DataProvider.Disconnect(con);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Sửa Sản Phẩm
        public bool SuaSanPham(SanPhamNew sp, NhaSanXuatNew nsx, LoaiSanPhamNew lsp)
        {
            try
            {
                string sql = "EXEC CapNhatSP @MaSP, @TenSP, @MaLoaiSP, @TenLoaiSP, @TenNSX, @DonViTinh, @GiaTien, @GiaNhap, @SoLuong, @LoiNhuan, @KhuyenMai, @TinhTrang, @HinhAnh";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@MaSP", sp.MaSP);
                cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                cmd.Parameters.AddWithValue("@MaLoaiSP", sp.MaLoaiSP);
                cmd.Parameters.AddWithValue("@TenLoaiSP", lsp.TenLoaiSP);
                cmd.Parameters.AddWithValue("@TenNSX", nsx.TenNSX);
                cmd.Parameters.AddWithValue("@DonViTinh", sp.DonViTinh);
                cmd.Parameters.AddWithValue("@GiaTien", sp.GiaTien);
                cmd.Parameters.AddWithValue("@GiaNhap", sp.GiaNhap);
                cmd.Parameters.AddWithValue("@SoLuong", sp.SoLuong);
                cmd.Parameters.AddWithValue("@LoiNhuan", sp.LoiNhuan);
                cmd.Parameters.AddWithValue("@KhuyenMai", sp.KhuyenMai);
                cmd.Parameters.AddWithValue("@TinhTrang", sp.TinhTrang);
                cmd.Parameters.AddWithValue("@HinhAnh", sp.HinhAnh);
                cmd.Connection = con;
                int rows = cmd.ExecuteNonQuery();
                DataProvider.Disconnect(con);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Cập Nhật Số Lượng
        public bool CapNhatSoLuong(string MaSP, int SoLuong)
        {
            try
            {
                string sql = "UPDATE SanPham SET SoLuong = SoLuong+@SoLuong WHERE MASP = @MASP";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@MASP", MaSP);
                cmd.Parameters.AddWithValue("@SOLUONG", SoLuong);
                cmd.Connection = con;
                int rows = cmd.ExecuteNonQuery();
                DataProvider.Disconnect(con);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Cập Nhật Số Lượng Khi Bán Hàng
        public bool CapNhatSoLuongKhiBanHang(string MaSP, int SoLuong)
        {
            try
            {
                string sql = "UPDATE SanPham SET SoLuong = SoLuong-@SoLuong WHERE MaSP = @MASP";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@MASP", MaSP);
                cmd.Parameters.AddWithValue("@SOLUONG", SoLuong);
                cmd.Connection = con;
                int rows = cmd.ExecuteNonQuery();
                DataProvider.Disconnect(con);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Lấy Tổng Doanh Thu
        public double GetTongDoanhThu()
        {
            string sql = "SELECT SUM(THANHTIEN) FROM HOADON";
            DataTable dt = new DataTable();
            dt = DataProvider.GetTable(sql);
            double doanhthu = double.Parse(dt.Rows[0][0].ToString());
            return doanhthu;
        }
        #endregion

        #region Lấy Tổng Khách Hàng
        public int GetTongKhachHang()
        {
            string sql = "SELECT COUNT(*) FROM KHACHHANG";
            DataTable dt = new DataTable();
            dt = DataProvider.GetTable(sql);
            int kh = int.Parse(dt.Rows[0][0].ToString());
            return kh;
        }
        #endregion

        #region Lấy Top 10 Sản Phẩm
        public List<SanPhamDTO> GetTop10SP(int top)
        {
            string sql = "SELECT TOP "+top+" cthd.MASP,sp.TENSP,SUM(cthd.SOLUONG) FROM CTHD cthd JOIN SANPHAM sp ON cthd.MASP=sp.MASP GROUP BY cthd.MASP, sp.TENSP ORDER BY SUM(cthd.SOLUONG)";
            DataTable dt = new DataTable();
            List<SanPhamDTO> lstSP = new List<SanPhamDTO>();
            dt = DataProvider.GetTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SanPhamDTO spDTO = new SanPhamDTO();
                spDTO.masp = int.Parse(dt.Rows[i][0].ToString());
                spDTO.tensp = dt.Rows[i][1].ToString();
                spDTO.soluong = int.Parse(dt.Rows[i][2].ToString());

                lstSP.Add(spDTO);
            }
            return lstSP;
        }
        #endregion

        #region Lấy Doanh Thu Hôm Nay
        public double GetDoanhThuHomNay()
        {
            try
            {
                string sql = "SELECT SUM(hd.THANHTIEN) FROM HOADON hd WHERE (YEAR(hd.NGAYLAP) = YEAR('" + DateTime.Now + "') AND MONTH(hd.NGAYLAP) = MONTH('" + DateTime.Now + "') AND DAY(hd.NGAYLAP) = DAY('" + DateTime.Now + "')) AND hd.DATHANHTOAN = '1'";
                DataTable dt = new DataTable();
                dt = DataProvider.GetTable(sql);
                double doanhthu = double.Parse(dt.Rows[0][0].ToString());
                return doanhthu;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        #endregion
    }
}
