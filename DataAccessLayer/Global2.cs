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
    public class Global2
    {
        // Khai báo các biến đăng nhập làm các thuộc tính tĩnh
        public static string Username { get; set; }
        public static string Password { get; set; }
    }
}
