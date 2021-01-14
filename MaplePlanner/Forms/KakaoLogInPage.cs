using Microsoft.Win32;
using Newtonsoft.Json.Linq;
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
    public partial class KakaoLogInPage : Form
    {
        KakaoManager kakaoManager;
        string accessCode;
        TimeSpan maxDuration;

        public KakaoLogInPage()
        {
            InitializeComponent();


            //webBrowser1.Navigate("javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())");

            webBrowser1.Visible = true;
            textBox_Code.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            button1.Visible = false;
            button2.Visible = false;

            kakaoManager = new KakaoManager();

            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;

            webBrowser1.Navigate(KakaoApiEndPoint.KakaoLogInUrl);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (kakaoManager.GetUserToKen(webBrowser1))
            {
                //MessageBox.Show("토큰 얻기 종로");
                kakaoManager.GetAccessToKen();

                textBox_Code.Visible = true;
                webBrowser1.Visible = false;
                textBox_Code.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                button1.Visible = true;
                button2.Visible = true;

                InsertAccessCode();
                //DialogResult = DialogResult.OK;
            }
        }

        public string GetAccessNum()
        {
            Int64 tmp1 = Convert.ToInt64(DateTime.Now.ToString("ddHHmmfff")); // 밀리초단위로 생성
            Int64 tmp2 = Convert.ToInt64(161217); // 인증랜덤키값
            Int64 tmp3 = Convert.ToInt64(tmp1 * tmp2);
            string str = tmp3.ToString();
            string retVal = str.Substring(str.Length-6);

            return retVal;
        }

        private void KakaoLogInPage_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            maxDuration = maxDuration.Subtract(TimeSpan.FromSeconds(1));
            label2.Text = maxDuration.ToString(@"mm\:ss");

            if (maxDuration == TimeSpan.Zero)
            {
                MessageBox.Show("인증시간이 만료되어 창을 종료합니다.\n다시 로그인해주세요");
                timer1.Stop();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_Code.Text == accessCode)
            {
                MessageBox.Show("성공적으로 인증되었습니다.");
                timer1.Stop();
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("인증번호가 일치하지 않습니다.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            MessageBox.Show("인증번호를 재전송합니다.");
            InsertAccessCode();
        }
        private void InsertAccessCode()
        {
            accessCode = GetAccessNum();
            if (SendMessage(accessCode))
            {
                maxDuration = TimeSpan.FromMinutes(3);
                label2.ForeColor = Color.Red;
                timer1.Start();
            }
        }
        private bool SendMessage(string accessCode)
        {
            try
            {
                JObject SendJson = new JObject();
                JObject LinkJson = new JObject();

                LinkJson.Add("web_url", "https://mapleplanner.synology.me");
                LinkJson.Add("mobile_web_url", "https://mapleplanner.synology.me");

                SendJson.Add("object_type", "text");
                SendJson.Add("text", "[메이플플래너]\n메시지 확인 후, 아래의 인증번호를 입력해주세요.\n\n인증번호 : " + accessCode);
                SendJson.Add("link", LinkJson);
                SendJson.Add("button_title", "홈페이지로 이동(PC 버전)");

                kakaoManager.KakaoDefaultSendMessage(SendJson);

                return true;
            }
            catch
            {
                MessageBox.Show("인증번호 전송 도중 오류가 발생했습니다.\n 관리자에게 문의하세요");
                return false;
            }
        }
        


        private void KakaoLogInPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void textBox_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
