using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaplePlanner
{
    class SQLManager
    {
        public SQLManager()
        {

        }
        public void Insert(int id, string name, string register_date, int playtime, int grade, int donation)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("INSERT INTO user_info VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')", id, name, register_date, playtime, grade, donation);
                    Console.WriteLine(strSQL);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex) { Console.WriteLine(ex.ToString()); }
                
            }
        }
        public string formatDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
