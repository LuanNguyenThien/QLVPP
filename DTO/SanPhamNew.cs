using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SanPhamNew
    {
        public string MaSP { get; set; }
        public string MaLoaiSP { get; set; }
        public string TenSP { get; set; }
        public string DonViTinh { get; set; }
        public int GiaTien { get; set; }
        public int GiaNhap { get; set; }
        public int SoLuong { get; set; }
        public int LoiNhuan { get; set; }
        public int KhuyenMai { get; set; }
        public string TinhTrang { get; set; }
        public byte[] HinhAnh { get; set; }
        public double tongdoanhthu { get; set; }
    }
}
