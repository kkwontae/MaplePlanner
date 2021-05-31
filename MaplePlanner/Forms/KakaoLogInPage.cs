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

        public KakaoLogInPage()
        {
            InitializeComponent();

            webBrowser1.Visible = true;

            kakaoManager = new KakaoManager();

            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;

            webBrowser1.Navigate(KakaoApiEndPoint.KakaoLogInUrl);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (kakaoManager.GetUserToKen(webBrowser1))
            {
                kakaoManager.GetAccessToKen();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
