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
    public class PhieuNhapDL
    {
        private static PhieuNhapDL Instance;
        public static PhieuNhapDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new PhieuNhapDL();
                }
                return Instance;
            }
        }
        private PhieuNhapDL() { }

        #region Thêm Phiếu Nhập
        public bool ThemPhieuNhap(PhieuNhapDTO pnDTO)
        {
            try
            {
                string sql = "EXEC ThemDonNhapHang @MaNV, @MaNSX, @NgayNhap, @TrangThai";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@MaNV", pnDTO.MaNV);
                cmd.Parameters.AddWithValue("@MaNSX", pnDTO.MaNSX);
                cmd.Parameters.AddWithValue("@NgayNhap", pnDTO.NgayNhap);
                cmd.Parameters.AddWithValue("@TrangThai", pnDTO.TrangThai);
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

        #region Lấy Danh Sách Phiếu Nhập
        public DataTable GetDanhSachPhieuNhap()
        {
            try
            {
                string sql = "SELECT * FROM GetDonNhapHang() WHERE [Trạng Thái]=N'Chưa Nhập'";
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

        #region Xác Nhận Phiếu Nhập
        public bool XacNhan(string MaPhieu)
        {
            try
            {
                string sql = "UPDATE DonNhapHang SET TrangThai=N'Đã Nhập' WHERE MaDNH = '" + MaPhieu + "'";
                int rows = DataProvider.JustExcuteNoParameter(sql);
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

        #region Cập Nhật Số Lượng Khi Đã Nhập
        public bool CapNhatSoLuong(string MaPhieu)
        {
            try
            {
                DataTable dt = CTPNDL.GetInstance.GetDanhSachPhieuNhap(MaPhieu);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SanPhamDL.GetInstance.CapNhatSoLuong(dt.Rows[i][0].ToString(), int.Parse(dt.Rows[i][1].ToString()));
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

        #region Xóa Phiếu Nhập
        public bool XoaPN(string MAPN)
        {
            try
            {
                string sql = "DELETE DonNhapHang WHERE MaDNH = '" + MAPN + "'";
                int rows = DataProvider.JustExcuteNoParameter(sql);
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

        #region Lấy Mã Phiếu Nhập Max
        public string GetMAPNMax()
        {
            try
            {
                string sql = "SELECT MAX(MaDNH) FROM DonNhapHang";
                DataTable dt = new DataTable();
                dt = DataProvider.GetTable(sql);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                return "1";
            }
        }
        #endregion
    }
}
