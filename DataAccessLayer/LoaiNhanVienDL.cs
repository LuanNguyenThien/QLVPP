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
    public class LoaiNhanVienDL
    {
        private static LoaiNhanVienDL Instance;
        public static LoaiNhanVienDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new LoaiNhanVienDL();
                }
                return Instance;
            }
        }
        private LoaiNhanVienDL() { }

        #region Lấy Danh Sách Loại Nhân Viên
        public DataTable GetDanhSachLoaiNhanVien()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = DataProvider.Openconnect())
            {
                using (SqlCommand cmd = new SqlCommand("GetLoaiCongViec", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dataTable);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL: " + ex.Message);
                    }
                }
            }

            return dataTable;
        }
        #endregion
    }
}
