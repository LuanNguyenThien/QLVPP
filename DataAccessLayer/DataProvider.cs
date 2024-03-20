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
    public class DataProvider
    {
        public static SqlConnection Openconnect()
        {
            SqlConnection sChuoiKetNoi = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyVPP;User Id=" + Global.username + ";Password=" +
            Global.password + ";");
            sChuoiKetNoi.Open();
            return sChuoiKetNoi;
        }
        public static void Disconnect(SqlConnection con)
        {
            con.Close();
        }
        public static int JustExcuteNoParameter(string sql)
        {
            SqlConnection con = Openconnect();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            int rows = cmd.ExecuteNonQuery();
            Disconnect(con);
            if (rows > 0)
            {
                return rows;
            }
            else
            {
                return -1;
            }
        }
        public static DataTable GetTable(string sql)
        {
            SqlConnection con = Openconnect();
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Disconnect(con);
            return dt;
        }
        public static object GetSingleValue(string sql)
        {
            SqlConnection con = Openconnect();
            SqlCommand cmd = new SqlCommand(sql, con);
            object result = cmd.ExecuteScalar();
            Disconnect(con);
            return result;
        }
    }
}
