using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HoaDonNew
    {
        public string MaHD { get; set; }
        public string MaNV { get; set; }
        public string MaKH { get; set; }
        public Nullable<System.DateTime> NgayBanHang { get; set; }
        public string TrangThai { get; set; }
        public decimal TriGiaHoaDon { get; set; }
        public Nullable<decimal> TienKhachTra { get; set; }
        public Nullable<decimal> TienThua { get; set; }

    }
}