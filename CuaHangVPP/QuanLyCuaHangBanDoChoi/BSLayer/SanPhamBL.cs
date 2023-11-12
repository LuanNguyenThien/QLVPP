﻿using DataAccessLayer;
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
    public class SanPhamBL
    {
        private static SanPhamBL Instance;
        public static SanPhamBL GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new SanPhamBL();
                }
                return Instance;
            }
        }
        private SanPhamBL() { }
        public int GetTongSanPhamDaBan()
        {
            return SanPhamDL.GetInstance.GetTongSanPhamDaBan();
        }
        public int GetMaSPMoi()
        {
            return SanPhamDL.GetInstance.GetMaSPMax()+1;
        }
        public string GetTenSP(int MASP)
        {
            return SanPhamDL.GetInstance.GetTenSP(MASP);
        }
        public DataTable GetDanhSachSanPhamTheoNCC(int MANCC)
        {
            return SanPhamDL.GetInstance.GetDanhSachSanPhamTheoNCC(MANCC);
        }
        public bool CheckMaSP(string MASP)
        {
            return SanPhamDL.GetInstance.CheckMaSP(MASP);
        }
        public bool NgungKinhDoanhSanPham(string MASP)
        {
            return SanPhamDL.GetInstance.NgungKinhDoanhSanPham(MASP);
        }
        public bool ThemSanPham(SanPham sp, NhaSanXuat nsx, LoaiSanPham lsp)
        {
            return SanPhamDL.GetInstance.ThemSanPham(sp, nsx, lsp);
        }
        public bool SuaSanPham(SanPham sp, NhaSanXuat nsx, LoaiSanPham lsp)
        {
            return SanPhamDL.GetInstance.SuaSanPham(sp, nsx, lsp);
        }
        public DataTable GetDanhSachSanPhamTheoBoLoc(string TENSP, string TENLOAISP, string TENNCC)
        {
            return SanPhamDL.GetInstance.GetDanhSachSanPhamTheoBoLoc(TENSP,TENLOAISP, TENNCC);
        }
        public bool CapNhatSoLuong(int MaSP, int SoLuong)
        {
            return SanPhamDL.GetInstance.CapNhatSoLuong(MaSP, SoLuong);
        }
        public bool CapNhatSoLuongKhiBanHang(int MaSP, int SoLuong)
        {
            return SanPhamDL.GetInstance.CapNhatSoLuongKhiBanHang(MaSP, SoLuong);
        }
        public DataTable GetDanhSachSanPham()
        {
            return SanPhamDL.GetInstance.GetDanhSachSanPham();
        }
        public double GetTongDoanhThu()
        {
            return SanPhamDL.GetInstance.GetTongDoanhThu();
        }
        public int GetTongKhachHang()
        {
            return SanPhamDL.GetInstance.GetTongKhachHang();
        }
        public List<SanPhamDTO> GetTop10SP(int top)
        {
            return SanPhamDL.GetInstance.GetTop10SP(top);
        }
        public double GetDoanhThuHomNay()
        {
            return SanPhamDL.GetInstance.GetDoanhThuHomNay();
        }
    }
}
