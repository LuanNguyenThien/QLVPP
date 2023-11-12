
using DataAccessLayer;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanDoChoi;

namespace BusinessLogicLayer
{
    public class NCCBL
    {
        private static NCCBL Instance;
        public static NCCBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new NCCBL();
                }
                return Instance;
            }
        }
        private NCCBL() { }
        public DataTable GetDanhSachNCC()
        {
            return NCCDL.GetInstance.GetDanhSachNCC();
        }
        public bool ThemNCC(NhaSanXuat nsx)
        {
            return NCCDL.GetInstance.ThemNCC(nsx);
        }
        public bool ThemNCCFull(NhaCungCapDTO nccDTO)
        {
            return NCCDL.GetInstance.ThemNCCFull(nccDTO);
        }
        public string GetTenNCC(int MANCC)
        {
            return NCCDL.GetInstance.GetTenNCC(MANCC);
        }
        public bool XoaNCC(string MANCC)
        {
            return NCCDL.GetInstance.NgungHopTacNCC(MANCC);
        }
        public bool CapNhatNCC(NhaSanXuat nsx)
        {
            return NCCDL.GetInstance.CapNhatNCC(nsx);
        }
        public bool CheckMaNCC(string MANCC)
        {
            return NCCDL.GetInstance.CheckMaNCC(MANCC);
        }
    }
}
