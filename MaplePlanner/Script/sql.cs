using MySql.Data.MySqlClient;
using System;

namespace MaplePlanner
{
    class SQLManager
    {
        public SQLManager()
        {

        }
        public static void Insert(string id, string name, string register_date, int playtime, UserGrade grade, int donation, string hddserial)
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
                    while (rdr.Read())
                    {
                        if (rdr["id"].ToString() == id)
                        {
                            if (rdr["hddserial"].ToString() != hddserial) //접속한 곳이 다르면
                            {
                                rdr.Close();
                                strSQL = string.Format("UPDATE user_info SET hddserial='{0}' WHERE id='{1}'", hddserial, id);
                                cmd = new MySqlCommand(strSQL, conn);
                                cmd.ExecuteReader();
                                return;
                            }
                            else
                            {
                                rdr.Close();
                                return;
                            }
                        }
                    }
                    rdr.Close();

                    strSQL = string.Format("INSERT INTO user_info VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", id, name, register_date, playtime, (int)grade, donation, hddserial);
                    cmd = new MySqlCommand(strSQL, conn);
                    Console.WriteLine(strSQL);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }
        public static bool ExistsHDD(string hddserial)
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
                                rdr["id"].ToString(),
                                rdr["name"].ToString(),
                                DateTime.Parse(rdr["register_date"].ToString()),
                                Convert.ToInt32(rdr["playtime"]),
                                (UserGrade)rdr["grade"],
                                Convert.ToInt32(rdr["donation"]),
                                rdr["hddserial"].ToString()
                                );
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                return userinfo;
            }
        }
        public static void Delete(string id)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("DELETE FROM user_info WHERE id='{0}'", id);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteReader();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }
        public static Permissions GetPermissions(UserGrade grade)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            Permissions perms = new Permissions();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("SELECT * FROM permission WHERE grade='{0}'", (int)grade);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if ((int)rdr["grade"] == (int)grade)
                        {
                            perms.MaxCharacters = (int)rdr["max_characters"];
                            perms.MaxPlans = (int)rdr["max_plans"];
                            perms.ShowCharImg = (bool)rdr["show_char_img"];
                            perms.ShowBgImg = (bool)rdr["show_bg_img"];
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex){ Console.WriteLine(ex.ToString()); }
                Console.WriteLine(grade);
                Console.WriteLine((int)grade);
                Console.WriteLine(perms.MaxCharacters);
                Console.WriteLine(perms.MaxPlans);
                Console.WriteLine(perms.ShowCharImg);
                Console.WriteLine(perms.ShowBgImg);
                return perms;
            }
        }
        public static void SetGrade(int id, UserGrade grade)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("UPDATE user_info SET grade='{0}' WHERE id={1}", (int)grade, id);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteReader();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }


        public static void InsertPermissions(UserGrade grade, int maxCharacter, int maxPlans, bool showCharImg, bool showBgImg)
        {
            string connStr = "Server=mapleplanner.synology.me;port=3307;Database=users;Uid=usermapleplanner;Pwd=SvX0gCXVSZRLJx06!!;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string strSQL = string.Format("INSERT INTO permission VALUES ({0},'{1}',{2},{3},{4},{5})", (int)grade, grade, maxCharacter, maxPlans, showCharImg, showBgImg);
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    Console.WriteLine(strSQL);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }
        public static string formatDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
