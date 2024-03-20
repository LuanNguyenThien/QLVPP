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
    public class LoaiSanPhamDL
    {
        private static LoaiSanPhamDL Instance;
        public static LoaiSanPhamDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new LoaiSanPhamDL();
                }
                return Instance;
            }
        }
        private LoaiSanPhamDL() { }

        #region Lấy Danh Sách Loại Sản Phẩm
        public DataTable GetDanhSachLoaiSanPham()
        {
            try
            {
                string sql = "EXEC GetLoaiSanPham";
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

        #region Thêm Loại Sản Phẩm
        public bool ThemLoaiSanPham(LoaiSanPhamNew lsp)
        {
            try
            {
                string sql = "EXEC ThemLoaiSanPham @TenLoaiSP";
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@TenLoaiSP", lsp.TenLoaiSP);
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

        #region Lấy MALSP từ TENLSP
        public string GetMaLSP_TenLSP(string tenLSP)
        {
            try
            {
                string sql = "SELECT dbo.GetMaLoaiSP(N'"+tenLSP+"')";
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

        #region Ngừng Kinh Doanh Sản Phẩm
        public bool NgungKinhDoanh(string MALOAISP)
        {
            try
            {
                string sql = "UPDATE LOAISANPHAM SET NGUNGKINHDOANH=1 WHERE MALOAISP='" + MALOAISP+"'";
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

        #region Cập Nhật Loại Sản Phẩm
        public bool CapNhatLoaiSanPham(LoaiSanPhamNew lspDTO)
        {
            try
            {
                string sql = "UPDATE LoaiSanPham SET TenLoaiSP=N'"+lspDTO.TenLoaiSP+"' WHERE MaLoaiSP='" + lspDTO.MaLoaiSP + "'";
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

        #region Kiểm Tra Mã Loại Sản Phẩm
        public bool CheckMaLoaiSP(string MALOAISP)
        {
            try
            {
                string sql = "SELECT * FROM LOAISANPHAM WHERE MALOAISP='" + MALOAISP + "'";
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
    }
}
