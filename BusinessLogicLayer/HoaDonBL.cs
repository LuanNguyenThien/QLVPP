using DataAccessLayer;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class HoaDonBL
    {
        private static HoaDonBL Instance;
        public static HoaDonBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new HoaDonBL();
                }
                return Instance;
            }
        }
        private HoaDonBL() { }
        public bool ThemHoaDon(HoaDonNew hd)
        {
            return HoaDonDL.GetInstance.ThemHoaDon(hd);
        }
        public string GetSOHDMAX()
        {
            return HoaDonDL.GetInstance.GetSOHDMax();
        }
        public bool XoaHD(string SOHD)
        {
            return HoaDonDL.GetInstance.XoaHD(SOHD);
        }
        public DataSet InHoaDon(string SOHD)
        {
            return HoaDonDL.GetInstance.InHoaDon(SOHD);
        }
        public bool CapNhatSoLuongTienKhachHang(string SOHD, decimal TienKhachHangTra, decimal TienThua)
        {
            return HoaDonDL.GetInstance.CapNhatSoLuongTienKhachHang(SOHD, TienKhachHangTra, TienThua);
        }
    }
}
