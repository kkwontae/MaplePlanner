using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaplePlanner
{
    public class Permissions
    {
        private int maxCharacters;
        private int maxPlans;
        private bool showCharImg;
        private bool showBgImg;
        private UserGrade grade;

        public int MaxCharacters
        {
            get { return maxCharacters; }
            set { maxCharacters = value; }
        }
        public int MaxPlans
        {
            get { return maxPlans; }
            set { maxPlans = value; }
        }
        public bool ShowCharImg
        {
            get { return showCharImg; }
            set { showCharImg = value; }
        }
        public bool ShowBgImg
        {
            get { return showBgImg; }
            set { showBgImg = value; }
        }

        public Permissions()
        {
            this.maxCharacters = 1;
            this.maxPlans = 0;
            this.showCharImg = false;
            this.grade = UserGrade.GUEST;
        }
        public Permissions(UserGrade grade)
        {
            this.grade = grade;
        }
    }
}
