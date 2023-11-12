using DTO;
using QuanLyCuaHangBanDoChoi;
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
    public class NCCDL
    {
        private static NCCDL Instance;
        public static NCCDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new NCCDL();
                }
                return Instance;
            }
        }
        private NCCDL() { }

        #region Lấy Danh Sách NCC
        public DataTable GetDanhSachNCC()
        {
            try
            {
                DataTable dt = new DataTable();
                string sql = "EXEC GetNSX";
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

        #region Thêm NCC
        public bool ThemNCC(NhaSanXuat nsx)
        {
            try
            {
                string sql = "EXEC ThemNhaSanXuat @TenNSX, @DiaChi, @Sdt";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@TenNSX", nsx.TenNSX);
                cmd.Parameters.AddWithValue("@DiaChi", nsx.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Sdt", nsx.Sdt ?? (object)DBNull.Value);
                cmd.Connection = con;
                int rows = cmd.ExecuteNonQuery();
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

        #region Thêm NCC Full
        public bool ThemNCCFull(NhaCungCapDTO nccDTO)
        {
            try
            {
                string sql = "INSERT INTO NCC(TENNCC,DIACHI,SDT,Email,NgungHopTac) VALUES(N'" + nccDTO.TenNCC + "',N'" + nccDTO.DiaChi + "','" + nccDTO.SDT + "',N'" + nccDTO.Email + "',0)";
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

        #region Ngừng Hợp Tác NCC
        public bool NgungHopTacNCC(string MANCC)
        {
            try
            {
                string sql = "UPDATE NCC SET NGUNGHOPTAC=1 WHERE MANCC='" + MANCC + "'";
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

        #region Cập Nhật NCC
        public bool CapNhatNCC(NhaSanXuat nsx)
        {
            try
            {
                string sql = "Exec CapNhatNSX @MaNSX, @TenNSX, @Diachi, @Sdt";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@MaNSX", nsx.MaNSX);
                cmd.Parameters.AddWithValue("@TenNSX", nsx.TenNSX);
                cmd.Parameters.AddWithValue("@Diachi", nsx.DiaChi);
                cmd.Parameters.AddWithValue("@Sdt", nsx.Sdt);
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

        #region Kiểm Tra Mã NCC
        public bool CheckMaNCC(string MANCC)
        {
            try
            {
                string sql = "SELECT * FROM NCC WHERE MANCC='" + MANCC + "'";
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

        #region Lấy Tên NCC
        public string GetTenNCC(int MANCC)
        {
            try
            {
                string sql = "SELECT TENNCC FROM NCC WHERE MANCC=" + MANCC;
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
        #region Lấy Tên MaNSX_TenNSX
        public string GetMaNSX_TenNSX(string tenNSX)
        {
            try
            {
                string sql = "SELECT dbo.GetMaNSX(N'" + tenNSX + "')";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = con;
                object result = cmd.ExecuteScalar();
                con.Close();
                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return null;
            }
        }
        #endregion
    }
}

