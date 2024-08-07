﻿using DataAccessLayer;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class PhieuNhapBL
    {
        private static PhieuNhapBL Instance;
        public static PhieuNhapBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new PhieuNhapBL();
                }
                return Instance;
            }
        }
        private PhieuNhapBL() { }
        public bool ThemPhieuNhap(PhieuNhapDTO pnDTO)
        {
            return PhieuNhapDL.GetInstance.ThemPhieuNhap(pnDTO);
        }
        public bool XoaPN(string MAPN)
        {
            return PhieuNhapDL.GetInstance.XoaPN(MAPN);
        }
        public DataTable GetDanhSachPhieuNhap()
        {
            return PhieuNhapDL.GetInstance.GetDanhSachPhieuNhap();
        }
        public bool XacNhan(string MaPhieu)
        {
            return PhieuNhapDL.GetInstance.XacNhan(MaPhieu);
        }
        public bool CapNhatSoLuong(string MaPhieu)
        {
            return PhieuNhapDL.GetInstance.CapNhatSoLuong(MaPhieu);
        }
        public string GetMAPNMax()
        {
            return PhieuNhapDL.GetInstance.GetMAPNMax();
        }
    }
}
