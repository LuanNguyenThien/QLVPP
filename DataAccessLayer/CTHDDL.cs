
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
    public class CTHDDL
    {
        private static CTHDDL Instance;
        public static CTHDDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new CTHDDL();
                }
                return Instance;
            }
        }
        private CTHDDL() { }

        #region Thêm Chi Tiết Hóa Đơn
        public bool ThemCTHD(DataTable dt, string SOHD, decimal THANHTIEN)
        {
            try
            {
                int rows = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sql = "EXEC ThemChiTietHD @MaHD, @MaSP, @SoLuong, @TongTien";
                    SqlConnection con = DataProvider.Openconnect();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@MaHD", SOHD);
                    cmd.Parameters.AddWithValue("@MaSP", dt.Rows[i][0].ToString());
                    cmd.Parameters.AddWithValue("@SoLuong", dt.Rows[i][4].ToString());
                    cmd.Parameters.AddWithValue("@TongTien", dt.Rows[i][5].ToString());
                    //string sql = "INSERT INTO CTHD VALUES('" + SOHD + "','" + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][4].ToString() + "')";
                    cmd.Connection = con;
                    rows = cmd.ExecuteNonQuery();
                }
                if (rows > 0)
                {
                    try
                    {
                        string sql = "UPDATE HoaDon SET TrangThai=N'Đã Thanh Toán', TriGiaHoaDon="+ THANHTIEN+ " WHERE MaHD='" + SOHD + "'";
                        int r = DataProvider.JustExcuteNoParameter(sql);
                        if (r > 0)
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

        #region Lấy MaCTHD Max
        public string GetMACTHDMax()
        {
            string sql = "SELECT MAX(MACTHD) FROM CTHD";
            DataTable dt = new DataTable();
            dt = DataProvider.GetTable(sql);
            return dt.Rows[0][0].ToString();
        }
        #endregion
    }
}
