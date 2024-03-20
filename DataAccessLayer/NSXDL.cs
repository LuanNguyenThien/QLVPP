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
    public class NSXDL
    {
        private static NSXDL Instance;
        public static NSXDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new NSXDL();
                }
                return Instance;
            }
        }
        private NSXDL() { }

        #region Lấy Danh Sách NSX
        public DataTable GetDanhSachNSX()
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

        #region Thêm NSX
        public bool ThemNSX(NhaSanXuatNew nsx)
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

        #region Cập Nhật NSX
        public bool CapNhatNSX(NhaSanXuatNew nsx)
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

