
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
    public class NSXBL
    {
        private static NSXBL Instance;
        public static NSXBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new NSXBL();
                }
                return Instance;
            }
        }
        private NSXBL() { }
        public DataTable GetDanhSachNSX()
        {
            return NSXDL.GetInstance.GetDanhSachNSX();
        }
        public bool ThemNSX(NhaSanXuatNew nsx)
        {
            return NSXDL.GetInstance.ThemNSX(nsx);
        }
        public string GetMaNSX_TenNSX(string tenNSX)
        {
            return NSXDL.GetInstance.GetMaNSX_TenNSX(tenNSX);
        }
        public bool CapNhatNSX(NhaSanXuatNew nsx)
        {
            return NSXDL.GetInstance.CapNhatNSX(nsx);
        }
    }
}
