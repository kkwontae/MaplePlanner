using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaplePlanner
{
    public enum UserGrade
    {
        GUEST = 0,
        BRONZE = 10,
        SILVER = 20,
        GOLD = 30,
        DIAMOND = 40,
        RED = 100, //후원자
        BLACK = 1000 //정기구독
    }
    class UserInfo
    {
        public int id;
        public string email;
        public string name;
        public DateTime register_date;
        public int playtime;
        public UserGrade grade;
        public int donation;
        public string hddserial;

        public UserInfo()
        {

        }
        public UserInfo(int id, string email, string name, DateTime register_date, int playtime, UserGrade grade, int donation, string hddserial)
        {
            this.id = id;
            this.email = email;
            this.name = name;
            this.register_date = register_date;
            this.playtime = playtime;
            this.grade = grade;
            this.donation = donation;
            this.hddserial = hddserial;
        }
    }
}
