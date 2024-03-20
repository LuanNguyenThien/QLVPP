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
    public class TaiKhoanDL
    {
        private static TaiKhoanDL Instance;
        public static TaiKhoanDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new TaiKhoanDL();
                }
                return Instance;
            }
        }
        private TaiKhoanDL() { }

        #region Kiểm Tra Đăng Nhập
        public bool KiemTraDangNhap(string manv, string mk)
        {
            try
            {
                string sqlCheckLogin = "SELECT dbo.CheckLogin(@TenTK, @MatKhau)";
                using (SqlConnection conn = DataProvider.Openconnect())
                {
                    using (SqlCommand cmd = new SqlCommand(sqlCheckLogin, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenTK", manv);
                        cmd.Parameters.AddWithValue("@MatKhau", mk);

                        bool ketQua = (bool)cmd.ExecuteScalar();
                        conn.Close();

                        return ketQua;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region Đổi Mật Khẩu
        /*public bool DoiMatKhau(string TenTK, string MatKhauMoi)
        {
            try
            {
                string sql = "EXEC dbo.ChangePasswordAndLogin @username, @password";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@username", TenTK);
                cmd.Parameters.AddWithValue("@password", MatKhauMoi);
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
        }*/
        public bool DoiMatKhau(string TenTK, string MatKhauMoi)
        {
            try
            {
                string sql = "UPDATE Taikhoan SET MatKhau = '" + MatKhauMoi + "' WHERE TenTK = @TenTK";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@TenTK", TenTK);
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

        #region Lấy Mã Quyền
        public int GetMaQuyen(string manv)
        {
            /*            string sql = "SELECT * FROM PHANQUYEN WHERE MANV = '" + manv + "'";
                        DataTable dt = new DataTable();
                        dt = DataProvider.GetTable(sql);
                        string maquyen = dt.Rows[0][1].ToString();
                        return int.Parse(maquyen);*/
            return 1;
        }
        #endregion

        #region Lấy Tên Quyền
        public string GetTenQuyen(string TenTK)
        {
            string sql = "SELECT Quyen FROM TaiKhoan WHERE TenTK = '" + TenTK + "'";
            DataTable dt = new DataTable();
            dt = DataProvider.GetTable(sql);
            string tenquyen = dt.Rows[0][0].ToString();
            return tenquyen;
        }
        #endregion
    }
}
