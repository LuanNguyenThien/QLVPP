
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class TaiKhoanBL
    {
        private static TaiKhoanBL Instance;
        public static TaiKhoanBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new TaiKhoanBL();
                }
                return Instance;
            }
        }
        private TaiKhoanBL() { }
        public bool CheckLogin(string manv, string mk)
        {
            return TaiKhoanDL.GetInstance.KiemTraDangNhap(manv.Trim().ToString(), mk.Trim().ToString());
        }
        public int GetMaQuyen(string manv)
        {
            return TaiKhoanDL.GetInstance.GetMaQuyen(manv);
        }
        public string GetTenQuyen(string maquyen)
        {
            return TaiKhoanDL.GetInstance.GetTenQuyen(maquyen);
        }
        public bool DoiMatKhau(string TenTK, string MatKhauMoi)
        {
            return TaiKhoanDL.GetInstance.DoiMatKhau(TenTK, MatKhauMoi);
        }

    }
}
