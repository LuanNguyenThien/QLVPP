using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NhanVienDTO
    {
        public string manv { get; set; }
        public string tennv { get; set; }
        public DateTime ngaysinh { get; set; }
        public string sdt { get; set; }

        public string gioitinh { get; set; }
        public string diachi { get; set; }
        public DateTime ngaytuyendung { get; set; }

        public string trangthai { get; set; }
        public string macv { get; set; }

        public string tentk { get; set; }


        public byte[] hinhanh { get; set; }
    }
}
