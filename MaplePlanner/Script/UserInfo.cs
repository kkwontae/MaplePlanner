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
        브론즈IV = 10,
        브론즈III = 11,
        브론즈II = 12,
        브론즈I = 13,
        실버IV = 20,
        실버III = 21,
        실버II = 22,
        실버I = 23,
        골드IV = 30,
        골드III = 31,
        골드II = 32,
        골드I = 33,
        다이아IV = 40,
        다이아III = 41,
        다이아II = 42,
        다이아I = 43,
        레드III = 100, 
        레드II = 101,
        레드I = 102,
        블랙 = 1000, //후원자
        관리자 = 9999
    }
    public class UserInfo
    {
        private string id;
        private string name;
        private DateTime register_date;
        private int playtime;
        private UserGrade grade;
        private int donation;
        private Permissions permissions;
        private string hddserial;

        public UserInfo()
        {
            grade = UserGrade.GUEST;
            permissions = new Permissions(grade);
        }
        public UserInfo(string id, string name, DateTime register_date, int playtime, UserGrade grade, int donation, string hddserial)
        {
            this.id = id;
            this.name = name;
            this.register_date = register_date;
            this.playtime = playtime;
            this.grade = grade;
            this.donation = donation;
            this.permissions = new Permissions(grade);
            this.hddserial = hddserial;
        }
        public string ID => id;
        public string Name => name;
        public DateTime RegisterDate => register_date;
        public int Playtime => playtime;
        public UserGrade Grade => grade;
        public int Donation => donation;
        public Permissions Permissions => permissions;
        public string HDDserial => hddserial;
    }
}
