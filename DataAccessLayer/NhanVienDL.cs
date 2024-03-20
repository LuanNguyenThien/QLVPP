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
    public class NhanVienDL
    {
        private static NhanVienDL Instance;
        public static NhanVienDL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new NhanVienDL();
                }
                return Instance;
            }
        }
        private NhanVienDL() { }

        #region Thêm Nhân Viên
        public bool ThemNhanVien(NhanVienDTO nvDTO)
        {
            try
            {
                using (SqlConnection con = DataProvider.Openconnect())
                {
                    using (SqlCommand cmd = new SqlCommand("spThemNhanVien", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@HOTEN", nvDTO.tennv);
                        cmd.Parameters.AddWithValue("@SDT", nvDTO.sdt);
                        cmd.Parameters.AddWithValue("@NGAYSINH", nvDTO.ngaysinh);
                        cmd.Parameters.AddWithValue("@GIOITINH", nvDTO.gioitinh);
                        cmd.Parameters.AddWithValue("@DIACHI", nvDTO.diachi);
                        cmd.Parameters.AddWithValue("@NGAYTUYENDUNG", nvDTO.ngaytuyendung);
                        cmd.Parameters.AddWithValue("@TRANGTHAI", nvDTO.trangthai);
                        cmd.Parameters.AddWithValue("@MACV", nvDTO.macv);
                        cmd.Parameters.AddWithValue("@TenTK", string.IsNullOrEmpty(nvDTO.tentk) ? DBNull.Value : (object)nvDTO.tentk);
                        cmd.Parameters.Add("@HINHANH", SqlDbType.Image).Value = nvDTO.hinhanh ?? (object)DBNull.Value;
                        //cmd.Parameters.AddWithValue("@HINHANH", nvDTO.hinhanh);
                        SqlParameter successParam = new SqlParameter("@Success", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(successParam);
                        cmd.ExecuteNonQuery();
                        return Convert.ToBoolean(successParam.Value);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Lấy Danh Sách Nhân Viên Theo Bộ Lọc
        public DataTable GetDanhSachNhanVienTheoBoLoc(string TENNV, string MALOAI)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = DataProvider.Openconnect())
            {
                using (SqlCommand cmd = new SqlCommand("GetDanhSachNhanVienTheoBoLoc", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@HoTen", TENNV);
                    cmd.Parameters.AddWithValue("@MaCV", MALOAI);

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

        #region Lấy Danh Sách Nhân Viên Theo Mã
        public DataTable GetDanhSachNhanVienTheoMa(string MANV)
        {
            try
            {
                using (SqlConnection conn = DataProvider.Openconnect())
                {
                    string sql = "SELECT * FROM GetNhanVienTheoMa(@MANV)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@MANV", MANV));

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return null;
            }
        }

        #endregion

        #region Lấy Hình Ảnh Nhân Viên
        public byte[] GetHinhNhanVien(string manv)
        {
            try
            {
                using (SqlConnection conn = DataProvider.Openconnect())
                {
                    string sql = "SELECT dbo.GetHinhNhanVien(@MANV)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@MANV", manv));

                        byte[] img = (byte[])cmd.ExecuteScalar();

                        return img;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Lấy Mã Nhân Viên Max

        #endregion

        #region Kiểm Tra Mã Nhân Viên
        public bool CheckMaNV(string MANV)
        {
            try
            {
                using (SqlConnection conn = DataProvider.Openconnect())
                {
                    using (SqlCommand cmd = new SqlCommand("CheckMaNV", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MANV", MANV));

                        SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        cmd.ExecuteNonQuery();

                        bool result = (bool)cmd.Parameters["@Result"].Value;

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region Thôi Việc Nhân Viên
        public bool ThoiViecNhanVien(string MANV)
        {
            using (SqlConnection conn = DataProvider.Openconnect())
            {
                SqlCommand cmd = new SqlCommand("CapNhatTrangThaiNhanVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MaNV", MANV));
                cmd.Parameters.Add(new SqlParameter("@TrangThaiMoi", "Đã nghỉ"));

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi database: " + ex.Message);
                    return false;
                }
            }
        }
        #endregion

        #region Sửa Thông Tin Nhân Viên
        public bool SuaThongTinNhanVien(NhanVienDTO nvDTO)
        {
            try
            {
                SqlConnection con = DataProvider.Openconnect();
                SqlCommand cmd = new SqlCommand("SuaThongTinNhanVien", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNV", nvDTO.manv);
                cmd.Parameters.AddWithValue("@HoTen", nvDTO.tennv);
                cmd.Parameters.AddWithValue("@Sdt", nvDTO.sdt);
                cmd.Parameters.Add("@NgaySinh", SqlDbType.Date).Value = nvDTO.ngaysinh.Date;
                cmd.Parameters.AddWithValue("@GioiTinh", nvDTO.gioitinh);
                cmd.Parameters.AddWithValue("@DiaChi", nvDTO.diachi);
                cmd.Parameters.Add("@NgayTuyenDung", SqlDbType.Date).Value = nvDTO.ngaytuyendung.Date;
                cmd.Parameters.AddWithValue("@TrangThai", nvDTO.trangthai);
                cmd.Parameters.AddWithValue("@MaCV", nvDTO.macv);
                cmd.Parameters.AddWithValue("@TenTK", string.IsNullOrEmpty(nvDTO.tentk) ? DBNull.Value : (object)nvDTO.tentk);
                cmd.Parameters.Add("@HINHANH", SqlDbType.Image).Value = nvDTO.hinhanh ?? (object)DBNull.Value;
                //cmd.Parameters.AddWithValue("@HinhAnh", nvDTO.hinhanh);
                SqlParameter successParam = new SqlParameter("@Success", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(successParam);
                cmd.ExecuteNonQuery();
                return Convert.ToBoolean(successParam.Value);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Lấy Tên Nhân Viên
        public string GetTenNhanVien(string tenTK)
        {
            string hoTen = string.Empty;

            try
            {
                using (SqlConnection con = DataProvider.Openconnect())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT dbo.GetNameNVTheoTK(@TenTK)", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TenTK", tenTK);
                        hoTen = cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
            }

            return hoTen;
        }
        #endregion

        #region Lấy Mã Nhân Viên
        public string GetMaNhanVien(string tenTK)
        {
            string maNV = string.Empty;

            try
            {
                using (SqlConnection con = DataProvider.Openconnect())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT dbo.GetMaNV(@TenTK)", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TenTK", tenTK);
                        maNV = cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
            }

            return maNV;
        }
        #endregion
    }
}
