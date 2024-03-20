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
    public class KhachHangDL
    {
        private static KhachHangDL Instance;
        public static KhachHangDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new KhachHangDL();
                }
                return Instance;
            }
        }
        private KhachHangDL() { }

        #region Lấy Mã Khách Hàng MAX
        public int GetMaKHMax()
        {
            try
            {
                string sql = "SELECT MAX(MAKH) FROM KHACHHANG";
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

        #region Lấy Tên Khách Hàng
        public string GetTenKhachHang(string SDT)
        {
            try
            {
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.TimKiemKhachHangTheoSDT(@ChuoiTimKiem)", con);
                cmd.Parameters.AddWithValue("@ChuoiTimKiem", SDT);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                string ten = dt.Rows[0][1].ToString();
                return ten;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Lấy Tên Khách Hàng
        public string GetTenMaKH(string SDT)
        {
            try
            {
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.TimKiemKhachHangTheoSDT(@ChuoiTimKiem)", con);
                cmd.Parameters.AddWithValue("@ChuoiTimKiem", SDT);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                string ten = dt.Rows[0][0].ToString();
                return ten;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Kiểm Tra Mã Khách Hàng
        public bool CheckMaKH(string MAKH)
        {
            try
            {
                string sql = "SELECT * FROM KHACHHANG WHERE MAKH='" + MAKH + "'";
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

        #region Lấy Danh Sách Khách Hàng
        public DataTable GetDanhSachKhachHang()
        {
            try
            {
                 string sqlQuery = @"SELECT * FROM dbo.GetAllKhachHang()";
                 SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, DataProvider.Openconnect());
                 DataTable dt = new DataTable();
                 adapter.Fill(dt);
                // Đổi tên các cột trong DataTable
                foreach (DataColumn column in dt.Columns)
                {
                    switch (column.ColumnName)
                    {
                        case "MaKH":
                            column.ColumnName = "Mã KH";
                            break;
                        case "Hoten":
                            column.ColumnName = "Tên KH";
                            break;
                        case "Sdt":
                            column.ColumnName = "SĐT";
                            break;
                        case "GioiTinh":
                            column.ColumnName = "Giới Tính";
                            break;
                        case "NgayDangKi":
                            column.ColumnName = "Ngày Đăng Kí";
                            break;
                        case "TongDoanhThu":
                            column.ColumnName = "Doanh Số";
                            break;

                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                 MessageBox.Show("Lỗi database: " + ex.Message);
                 return null;
            }
        }
        #endregion

        #region Lấy Danh Sách Khách Hàng Tìm Kiếm
        public DataTable GetDanhSachKhachHangTimKiem(string tenkh)
        {
            try
            {
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.TimKiemKhachHangTheoTenHoacSDT(@ChuoiTimKiem)", con);
                cmd.Parameters.AddWithValue("@ChuoiTimKiem", tenkh);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                con.Close();

                // Đổi tên các cột trong DataTable
                foreach (DataColumn column in dt.Columns)
                {
                    switch (column.ColumnName)
                    {
                        case "MaKH":
                            column.ColumnName = "Mã KH";
                            break;
                        case "Hoten":
                            column.ColumnName = "Tên KH";
                            break;
                        case "Sdt":
                            column.ColumnName = "SĐT";
                            break;
                        case "GioiTinh":
                            column.ColumnName = "Giới Tính";
                            break;
                        case "NgayDangKi":
                            column.ColumnName = "Ngày Đăng Kí";
                            break;
                        case "TongDoanhThu":
                            column.ColumnName = "Doanh Số";
                            break;

                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Thêm Khách Hàng
        public bool ThemKhachHang(DTO.KhachHangDTO khDTO)
        {
            try
            {
                SqlConnection connection = DataProvider.Openconnect();
                String sqlQuery = "PC_ThemKhachHang";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Hoten", khDTO.tenkh);
                cmd.Parameters.AddWithValue("@Sdt", khDTO.sdt);
                cmd.Parameters.AddWithValue("@GioiTinh", khDTO.gioitinh);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true; // Thêm khách hàng thành công
                }
                else
                {
                    return false; // Không có dòng nào được thêm vào
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Xóa Khách Hàng
        public bool XoaKhachHang(string MAKH)
        {
            try
            {
                SqlConnection connection = DataProvider.Openconnect();
                String sqlQuery = "XoaKhachHang";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaKH", MAKH);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true; // Thêm khách hàng thành công
                }
                else
                {
                    return false; // Không có dòng nào được thêm vào
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Sửa Thông Tin Khách Hàng
        public bool SuaThongTinKhachHang(DTO.KhachHangDTO khDTO)
        {
            try
            {
                SqlConnection connection = DataProvider.Openconnect();
                String sqlQuery = "PC_CapNhatKhachHang";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaKH", khDTO.makh);
                cmd.Parameters.AddWithValue("@Hoten", khDTO.tenkh);
                cmd.Parameters.AddWithValue("@Sdt", khDTO.sdt);
                cmd.Parameters.AddWithValue("@GioiTinh", khDTO.gioitinh);
                cmd.Parameters.AddWithValue("@NgayDangKi", khDTO.ngaydangky);
                cmd.Parameters.AddWithValue("@TongDoanhThu", khDTO.doanhso);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return true; // Thêm khách hàng thành công
                }
                else
                {
                    return false; // Không có dòng nào được thêm vào
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Cập Nhật Doanh Số Khách Hàng
        public bool CapNhatDoanhSoKhachHang(int MAKH,decimal DOANHSO)
        {
            try
            {
                string sql = "UPDATE KHACHHANG SET DOANHSO=DOANHSO+@DOANHSO WHERE MAKH = @MAKH";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@MAKH", MAKH);
                cmd.Parameters.AddWithValue("@DOANHSO", DOANHSO);
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
    }
}
