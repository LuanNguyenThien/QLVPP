using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public class AlterLogin
    {
        private static AlterLogin Instance;
        public static AlterLogin GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new AlterLogin();
                }
                return Instance;
            }
        }
        private AlterLogin() { }

        public bool GanquyenLogin(string username)
        {
            try
            {
                string sql = "GRANT ALTER ANY Login TO [" + username + "]";
                int rows = AlterLoginDB.JustExcuteNoParameter(sql);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi database: " + ex.Message);
                return false;
            }
        }
    }
}
