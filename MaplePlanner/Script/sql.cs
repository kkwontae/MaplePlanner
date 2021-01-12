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
        public void Insert(int id, string email, string name, string register_date, int playtime, UserGrade grade, int donation, string hddserial)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("SELECT * FROM user_info WHERE id={0}", id);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    //cmd.ExecuteNonQuery();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        if (Convert.ToInt32(rdr["id"]) == id)
                        {
                            if(rdr["hddserial"].ToString() == hddserial)
                            {
                                rdr.Close();
                                return;
                            }
                        }
                    }
                    rdr.Close();

                    strSQL = string.Format("INSERT INTO user_info VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", id, email, name, register_date, playtime, (int)grade, donation, hddserial);
                    cmd = new MySqlCommand(strSQL, conn);
                    Console.WriteLine(strSQL);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }
        public static bool IsSerailinDB(string hddserial)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("SELECT * FROM user_info WHERE hddserial='{0}'", hddserial);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (rdr["hddserial"].ToString() == hddserial)
                        {
                            rdr.Close();
                            return true;
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                return false;
            }
        }
        public static UserInfo GetUserDB(string hddserial)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            UserInfo userinfo = new UserInfo();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("SELECT * FROM user_info WHERE hddserial='{0}'", hddserial);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (rdr["hddserial"].ToString() == hddserial)
                        {
                            userinfo = new UserInfo(
                                Convert.ToInt32(rdr["id"]),
                                rdr["email"].ToString(),
                                rdr["name"].ToString(),
                                DateTime.Parse(rdr["register_date"].ToString()),
                                Convert.ToInt32(rdr["playtime"]),
                                (UserGrade)rdr["grade"],
                                Convert.ToInt32(rdr["donation"]),
                                hddserial
                                );
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                return userinfo;
            }
        }
        public static void Delete(string hddserial)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("DELETE FROM user_info WHERE hddserial='{0}'", hddserial);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteReader();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }



        public string formatDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
