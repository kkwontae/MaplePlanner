using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaplePlanner
{
    public partial class ShowUserInfo : Form
    {
        public ShowUserInfo()
        {
            InitializeComponent();
        }

        private void ShowUserInfo_Load(object sender, EventArgs e)
        {
            string info;
            if (MainForm.userDB.Grade == UserGrade.GUEST)
            {
                info = string.Format(@"계정번호 : 계정을 연동해주세요
멤버십 등급 : GUEST
등록일시 : 계정을 연동해주세요");
            }
            else
            {
                info = string.Format(@"계정번호 : {0}
멤버십 등급 : {1}
등록일시 : {2}
", MainForm.userDB.ID, MainForm.userDB.Grade, MainForm.userDB.RegisterDate);
            }
            label1.Text = info;
        }

        private void ShowUserInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }
    }
}
