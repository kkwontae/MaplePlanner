﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Management;
using System.Collections;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Security.Cryptography;

namespace MaplePlanner
{
    public partial class MainForm : Form
    {
        private KakaoLogInPage kakaoLoginPage;
        private KakaoManager kakaoManager;
        public static UserInfo userDB;
        public static Permissions userPermissions;
        public static string hddserial;
        ArrayList hardDriveDetails = new ArrayList();


        private void WelcomeUser()
        {
            hddserial = Script.HardDrive.GetHDDSerial();
            userDB = new UserInfo(); // GUEST
            userPermissions = new Permissions(); // GUEST
            pictureBox1.Visible = false;

            //string id = "1591937298";

            if (SQLManager.ExistsHDD(hddserial)) // 등록된 HDD이면,
            {
                userDB = SQLManager.GetUserDB(hddserial);
                userPermissions = SQLManager.GetPermissions(userDB.Grade);
                로그인ToolStripMenuItem.Text = "계정연동완료(" + userDB.ID + ")";
            }
            else
            {
                로그인ToolStripMenuItem.Text = "계정연동";
                MessageBox.Show("GUEST 모드로 접속하였습니다.\n상단의 계정>계정연동을 통해 카카오톡 계정을 연동하시면 더 많은 기능을 사용할 수 있습니다.");
            }
        }
        public MainForm()
        {
            InitializeComponent();
            WebBrowserVersionSetting();

            kakaoManager = new KakaoManager();
            
            //menuStrip1.Renderer = new RedTextRenderer();

            label7.Text = "현재시간 : " + DateTime.Now.ToString(@"HH\:mm\:ss");

            timer1.Start();

            try
            {
                //게시(Clickonce 등 일 경우) 응용프로그램 버전
                버전ToolStripMenuItem.Text = string.Format("ver. {0} (배포)",
                    System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString().Remove(0, 2));
                textDebug.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
            catch
            {
                //로컬 빌드 버전일 경우 (현재 어셈블리 버전)
                버전ToolStripMenuItem.Text = string.Format("ver. {0} (빌드)",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(0,2));
                textDebug.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            WelcomeUser();
            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                    File.Create(FilePath).Close();
            }
            catch { }

            if (new FileInfo(FilePath).Length > 0)
            {
                try
                {
                    ini.Load(FilePath);
                    string num = ini.First().ToString().Split(new[] { '[', ',' })[1];

                    labelNick.Text = ini[num]["Nickname"].Value;
                    labelLevel.Text = ini[num]["Level"].Value;
                    labelJob.Text = ini[num]["Job"].Value;

                    sections.AddRange(GetSectionNames());
                    listBox_Characters.Items.AddRange(sections.Select(str => ini[str]["Nickname"].Value).ToArray());
                }
                catch { }
            }
            if (listBox_Characters.Items.Count > 0)
            {
                EnableAllCheckBox(true);
                listBox_Characters.SelectedIndex = 0;
            }
            else
            {
                EnableAllCheckBox(false);
                button_AddPlan.Enabled = false;
                button_RemovePlan.Enabled = false;
                button_RemoveCharacter.Enabled = false;
                textBox_Plan.Enabled = false;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //label6.Text = "12시까지 : " + DateTime.Today.AddDays(1).Subtract(DateTime.Now).ToString(@"HH\:mm\:ss");

            //label6.Text = "00시까지 : " + DateTime.Today.AddDays(1).Subtract(DateTime.Now).ToString(@"hh\:mm\:ss");
            label7.Text = "현재시간 : " + DateTime.Now.ToString(@"HH\:mm\:ss");
            //if (DateTime.Today.AddDays(1).Subtract(DateTime.Now).CompareTo(new TimeSpan(1, 0, 0)) < 0)
            //    label6.ForeColor = Color.Red;
            //else
            //    label6.ForeColor = Color.Black;
            if (DateTime.Today.AddDays(1).Subtract(DateTime.Now).CompareTo(new TimeSpan(1, 0, 0)) < 0)
                label7.ForeColor = Color.Red;
            else
                label7.ForeColor = Color.Black;
        }

        #region Buttons
        private void button_AddPlan_Click(object sender, EventArgs e)
        {
            if (Encoding.Default.GetBytes(textBox_Plan.Text).Length > 40)
            {
                MessageBox.Show("영문 40자 혹은 한글 20자 이하까지 작성가능합니다.");
                return;
            }

            if (checkedListBox1.Items.Count==5)
            {
                MessageBox.Show("플랜은 캐릭터 별 최대 5개까지만 등록 가능합니다.");
                textBox_Plan.Text = string.Empty;
                textBox_Plan.Focus();
                return;
            }
            string plan = textBox_Plan.Text.Trim();
            if (plan.Length < 1)
            {
                textBox_Plan.Text = string.Empty;
                textBox_Plan.Focus();
                return;
            }

            ini[sections[nSelectedCharacter]]["Plan" + checkedListBox1.Items.Count.ToString()] = plan;
            
            ini.Save(FilePath);
            ini.Load(FilePath);

            checkedListBox1.Items.Add(plan);
            textBox_Plan.Text = string.Empty;
            textBox_Plan.Focus();
            checkedListBox1.SelectedIndex = checkedListBox1.Items.Count-1;
        }
        private void button_RemovePlan_Click(object sender, EventArgs e)
        {
            int sel = checkedListBox1.SelectedIndex;
            if (sel >= 0)
            {
                ini[sections[nSelectedCharacter]]["Plan" + sel.ToString()] = string.Empty;

                for (int i = sel; i < 4; i++)
                {
                    ini[sections[nSelectedCharacter]]["Plan" + i.ToString()]
                    = ini[sections[nSelectedCharacter]]["Plan" + (i + 1).ToString()];
                }
                ini[sections[nSelectedCharacter]]["Plan4"] = string.Empty;

                ini.Save(FilePath);
                ini.Load(FilePath);

                checkedListBox1.Items.RemoveAt(sel);
                try { checkedListBox1.SelectedIndex = sel; }
                catch
                {
                    try { checkedListBox1.SelectedIndex = sel - 1; }
                    catch { }
                }
            }
        }
        private void button_RemoveCharacter_Click(object sender, EventArgs e)
        {
            ini[sections[nSelectedCharacter]].Clear();
            sections.RemoveAt(listBox_Characters.SelectedIndex);
            ini.Save(FilePath);
            ini.Load(FilePath);
            listBox_Characters.Items.RemoveAt(listBox_Characters.SelectedIndex);
        }

        private void button_addCharacter_Click(object sender, EventArgs e)
        {
            string nickname = Regex.Replace(textBox_Nickname.Text,@"\r\n","");
            if (listBox_Characters.Items.Contains(nickname))
            {
                MessageBox.Show("캐릭터를 중복 등록할 수 없습니다","알림");
                textBox_Nickname.Focus();
                return;
            }
            if (listBox_Characters.Items.Count >= userPermissions.MaxCharacters)
            {
                MessageBox.Show(userDB.Grade.ToString() + " 등급 캐릭터 등록 갯수 제한 : " + userPermissions.MaxCharacters.ToString());
                textBox_Nickname.Text = string.Empty;
                textBox_Nickname.Focus();
                return;
            }
                
            if (!string.IsNullOrEmpty(nickname))
            {
                string strLevel;
                string strJob;
                string strImgUrl;
                string strUrl = "https://maple.gg/u/" + nickname;
                HtmlWeb web = new HtmlWeb();
                //var doc1 = web.Load("https://maple.gg/search?q=" + nickname);
                
                try
                {
                    var doc = web.Load(strUrl);

                    strLevel = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[2]/div[1]/ul/li[1]").InnerText.Split('.')[1];
                    strJob = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[2]/div[1]/ul/li[2]").InnerText;
                    strImgUrl = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[1]/div/div[2]/img").Attributes["src"].Value;
                }
                catch
                {
                    MessageBox.Show("존재하지 않는 닉네임입니다");
                    textBox_Nickname.Focus();
                    return;
                }

                int intLast;
                try{ intLast = Convert.ToInt32(sections[sections.Count - 1]) + 1; }
                catch { intLast = 1; }

                ini[intLast.ToString()]["Nickname"] = nickname;
                ini[intLast.ToString()]["img"] = strImgUrl;
                ini[intLast.ToString()]["Level"] = strLevel;
                ini[intLast.ToString()]["job"] = strJob;
                ini[intLast.ToString()]["Plan0"] = string.Empty;
                ini[intLast.ToString()]["Plan1"] = string.Empty;
                ini[intLast.ToString()]["Plan2"] = string.Empty;
                ini[intLast.ToString()]["Plan3"] = string.Empty;
                ini[intLast.ToString()]["Plan4"] = string.Empty;
                ini[intLast.ToString()]["PlanCheck"] = ".....";
                ini[intLast.ToString()]["Symbol"] = 0;
                ini[intLast.ToString()]["DailyBoss"] = 0;
                ini[intLast.ToString()]["WeeklyBoss"] = 0;
                ini[intLast.ToString()]["MonthlyBoss"] = 0;

                sections.Add(intLast.ToString());

                ini.Save(FilePath);
                ini.Load(FilePath);

                listBox_Characters.Items.Add(nickname);
                if (userPermissions.ShowCharImg)
                    pictureBox1.Visible = true;
                else
                    pictureBox1.Visible = false;
                textBox_Nickname.Text = string.Empty;

                listBox_Characters.SelectedIndex = listBox_Characters.Items.Count - 1;
                textBox_Nickname.Focus();
            }
        }
        #endregion

        private void textBox_Nickname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x0D) //VK_RETURN
                button_addCharacter.PerformClick();
        }
        
        int nSelectedCharacter;
        private void listBox_Characters_SelectedIndexChanged(object sender, EventArgs e)
        {
            nSelectedCharacter = listBox_Characters.SelectedIndex;
            if (nSelectedCharacter >= 0)
            {
                UpdateInfo(nSelectedCharacter);
                EnableAllCheckBox(true);
                checkedListBox1.Enabled = true;
                if (userPermissions.ShowCharImg)
                    pictureBox1.Visible = true;
                else
                    pictureBox1.Visible = false;
                    
                button_AddPlan.Enabled = true;
                button_RemoveCharacter.Enabled = true;
                textBox_Plan.Enabled = true;
            }
            else
            {
                if (listBox_Characters.Items.Count> 0)
                    listBox_Characters.SelectedIndex = listBox_Characters.Items.Count - 1;
                else
                {
                    labelNick.Text = "-";
                    labelLevel.Text = "-";
                    labelJob.Text = "-";
                    if (userPermissions.ShowBgImg)
                        pictureBox1.Visible = true;
                    else
                        pictureBox1.Visible = false;
                    checkedListBox1.Items.Clear();
                    checkedListBox1.Enabled = false;
                    button_AddPlan.Enabled = false;
                    button_RemovePlan.Enabled = false;
                    button_RemoveCharacter.Enabled = false;
                    textBox_Plan.Enabled = false;
                    CheckAllCheckBox(false);
                    EnableAllCheckBox(false);
                }
            }
        }
        private void UpdateInfo(int selectedIndex)
        {
            string section = sections[selectedIndex];
            
            labelNick.Text = ini[section]["Nickname"].Value;
            labelLevel.Text = ini[section]["Level"].Value;
            labelJob.Text = ini[section]["Job"].Value;
            List<string> plans = new List<string>();
            for(int i=0; i<5; i++)
            {
                string plan = ini[sections[nSelectedCharacter]]["Plan" + i.ToString()].Value;
                if(!string.IsNullOrEmpty(plan))
                    plans.Add(plan);
            }
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(plans.ToArray());
            string strPlanChecked = ini[sections[nSelectedCharacter]]["PlanCheck"].Value;
            if (checkedListBox1.Items.Count > 0)
            {
                button_RemovePlan.Enabled = true;
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (strPlanChecked[i] == '!')
                        checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                    else
                    {
                        try { checkedListBox1.SetItemCheckState(i, CheckState.Unchecked); }
                        catch { }
                    }
                }
            }
            else
                button_RemovePlan.Enabled = false;
                
            try { pictureBox1.ImageLocation = ini[sections[nSelectedCharacter]]["img"].Value; }
            catch { }

            LoadCheckState(tabPageDailyBoss);
            LoadCheckState(tabPageWeeklyBoss);
            LoadCheckState(tabPageMonthlyBoss);
            LoadCheckState(tabPageSymbol);
            LoadCheckState(tabPageContents, 6, new string[] { "Daily","Weekly" });
        }

        #region ini Setting
        List<string> sections = new List<string>();
        IniFile ini = new IniFile();

        static readonly string projectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        static readonly string DirPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), projectName);
        readonly string FilePath = System.IO.Path.Combine(DirPath, "Characters.dat");
        //[DllImport("kernel32")]
        //static extern int GetPrivateProfileString(string Section, string Key,
        //      string Value, StringBuilder Result, int Size, string FileName);


        //[DllImport("kernel32")]
        //static extern int GetPrivateProfileString(string Section, int Key,
        //       string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
        //       int Size, string FileName);


        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int Section, string Key,
              string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
              int Size, string FileName);

        public string[] GetSectionNames()  // ini 파일 안의 모든 section 이름 가져오기
        {
            for (int maxsize = 500; true; maxsize *= 2)
            {
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(0, "", "", bytes, maxsize, FilePath);

                if (size < maxsize - 2)
                {
                    string Selected = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                    return Selected.Split(new char[] { '\0' });
                }
            }
        }
        #endregion

        private void LoadCheckState(TabPage tp)
        {
            string type = Regex.Replace(tp.Name, @"tabPage", "");

            var tp_cbs = tp.Controls.OfType<CheckBox>().ToArray();
            int value = Convert.ToInt32(ini[sections[nSelectedCharacter]][type].Value);
            int temp = value;
            for (int i = 0; i < tp_cbs.Length; i++)
            {
                if ((value & 0b1) == 0b1)
                    tp_cbs[i].Checked = true;
                else
                    tp_cbs[i].Checked = false;
                value >>= 1;
            }
            ini[sections[nSelectedCharacter]][type] = temp.ToString();
            ini.Save(FilePath);
            ini.Load(FilePath);
        }

        private void LoadCheckState(TabPage tp, int seperateIndex, string[] strList)
        {
            var tp_cbs = tp.Controls.OfType<CheckBox>().ToArray();
            int[] size = { seperateIndex, tp_cbs.Length };
            for (int sep=0; sep < 2; sep++)
            {
                int value = Convert.ToInt32(ini[sections[nSelectedCharacter]][strList[sep]].Value);
                int temp = value;
                for (int i = sep*seperateIndex; i < size[sep]; i++)
                {
                    if ((value & 0b1) == 0b1)
                        tp_cbs[i].Checked = true;
                    else
                        tp_cbs[i].Checked = false;
                    value >>= 1;
                }
                ini[sections[nSelectedCharacter]][strList[sep]] = temp.ToString();
                ini.Save(FilePath);
                ini.Load(FilePath);
            }
        }
        private void SaveCheckState(object sender, EventArgs e)
        {
            if (nSelectedCharacter < 0)
                return;

            string cbName = ((CheckBox)sender).Name;
            int cbNum = Convert.ToInt32(Regex.Replace(cbName, @"\D", ""));
            string type = Regex.Replace(cbName, @"\d|checkBox", "");
            int flag = (int)Math.Pow(2,cbNum);

            int value = Convert.ToInt32(ini[sections[nSelectedCharacter]][type].Value);
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked)
                value += flag;
            else
                value -= flag;
            ini[sections[nSelectedCharacter]][type] = value;
            ini.Save(FilePath);
            ini.Load(FilePath);
        }
        #region 루타비스
        private void checkBoxDailyBoss4_Click(object sender, EventArgs e)
        {
            if (checkBoxDailyBoss4.Checked)
            {
                checkBoxDailyBoss5.Checked = true;
                checkBoxDailyBoss6.Checked = true;
                checkBoxDailyBoss7.Checked = true;
                checkBoxDailyBoss8.Checked = true;
            }
            else
            {
                checkBoxDailyBoss5.Checked = false;
                checkBoxDailyBoss6.Checked = false;
                checkBoxDailyBoss7.Checked = false;
                checkBoxDailyBoss8.Checked = false;
            }
        }
        private void checkBoxWeeklyBoss4_Click(object sender, EventArgs e)
        {
            if (checkBoxWeeklyBoss4.Checked)
            {
                checkBoxWeeklyBoss5.Checked = true;
                checkBoxWeeklyBoss6.Checked = true;
                checkBoxWeeklyBoss7.Checked = true;
                checkBoxWeeklyBoss8.Checked = true;
            }
            else
            {
                checkBoxWeeklyBoss5.Checked = false;
                checkBoxWeeklyBoss6.Checked = false;
                checkBoxWeeklyBoss7.Checked = false;
                checkBoxWeeklyBoss8.Checked = false;
            }
        }
        #endregion

        

        #region ToolStripMenuItem
        private void 모두체크해제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말로 모두 체크해제 하시겠습니까?", "경고!", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                CheckAllCheckBox(false);
            }
        }
        private void EnableAllCheckBox(bool enable)
        {
            var tctrls = this.Controls.OfType<TabControl>().ToArray();
            //var tbpgs = tctrls[i].Controls.OfType<TabPage>().ToArray();
            //MessageBox.Show(tbpgs.Length.ToString());
            foreach (var tc in tctrls) //Form -> TabControls
            {
                foreach (var tb in tc.Controls.OfType<TabPage>().ToArray()) // TabControls -> TabPages
                {
                    foreach (var cb in tb.Controls.OfType<CheckBox>().ToArray()) // TabPages -> Checkboxes
                    {
                        cb.Enabled = enable;
                    }
                }
            }
        }
        private void CheckAllCheckBox(bool check)
        {
            var tctrls = this.Controls.OfType<TabControl>().ToArray();

            foreach (var tc in tctrls) //Form -> TabControls
            {
                foreach (var tb in tc.Controls.OfType<TabPage>().ToArray()) // TabControls -> TabPages
                {
                    foreach (var cb in tb.Controls.OfType<CheckBox>().ToArray()) // TabPages -> Checkboxes
                    {
                        cb.Checked = check;
                    }
                }
            }
        }
        private void ControlTabPageCheckBox(TabPage tb, bool check)
        {
            var tctrls = this.Controls.OfType<TabControl>().ToArray();
            //var tbpgs = tctrls[i].Controls.OfType<TabPage>().ToArray();
            //MessageBox.Show(tbpgs.Length.ToString());
            foreach (var cb in tb.Controls.OfType<CheckBox>().ToArray()) // TabPages -> Checkboxes
            {
                cb.Checked = check;
            }
        }
        
        private void ControlTabPageCheckBox(TabPage tb, bool check, int startindex, int count)
        {
            var tctrls = this.Controls.OfType<TabControl>().ToArray();
            //var tbpgs = tctrls[i].Controls.OfType<TabPage>().ToArray();
            //MessageBox.Show(tbpgs.Length.ToString());
            int i = 0;
            foreach (var cb in tb.Controls.OfType<CheckBox>().ToArray()) // TabPages -> Checkboxes
            {
                
                if (i++ >= startindex && i <= startindex + count)
                    cb.Checked = check;
                else
                    continue;
                //MessageBox.Show(i.ToString() + ", " + (i >= startindex && i < startindex + count));
                
            }
        }
        private void 정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info frm = new Info();
            
            frm.ShowDialog();
        }
        private void 일일초기화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("체크해제 대상 : 심볼퀘스트, 일일컨텐츠, 일일보스\n정말로 체크해제 하시겠습니까?", "경고!", MessageBoxButtons.YesNo)
                  == DialogResult.Yes)
            {
                ControlTabPageCheckBox(tabPageDailyBoss, false);
                ControlTabPageCheckBox(tabPageSymbol, false);
                ControlTabPageCheckBox(tabPageContents, false, 0, 4);
            }
            
        }
        private void 주간초기화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("체크해제 대상 : 주간보스, 주간컨텐츠\n정말로 체크해제 하시겠습니까?", "경고!", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
            {
                ControlTabPageCheckBox(tabPageWeeklyBoss, false);
                ControlTabPageCheckBox(tabPageContents, false, 6, 3);
            }
        }
        private void 월간초기화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlTabPageCheckBox(tabPageMonthlyBoss, false);
        }
        #endregion

        private void textBox_Plan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x0D) //VK_RETURN
                button_AddPlan.PerformClick();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string plancheck = ini[sections[nSelectedCharacter]]["PlanCheck"].Value;

            if (e.NewValue == CheckState.Checked)
                plancheck = plancheck.Remove(e.Index, 1).Insert(e.Index, "!");
            else
                plancheck = plancheck.Remove(e.Index, 1).Insert(e.Index, ".");

            ini[sections[nSelectedCharacter]]["PlanCheck"] = plancheck;
            
            ini.Save(FilePath);
            ini.Load(FilePath);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.Items.Count > 0)
                button_RemovePlan.Enabled = true;
            else
                button_RemovePlan.Enabled = false;
        }

        private void 패치노트ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatchNotes dlg = new PatchNotes();
            dlg.ShowDialog();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                var url = "https://maple.gg/u/" + listBox_Characters.Items[nSelectedCharacter].ToString();
                webBrowser1.Url = new Uri(url);
            }
            catch
            {

            }
            
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            string imgurl = string.Empty;




            string strLevel;
            string strJob;

            var url = "https://maple.gg/u/" + listBox_Characters.Items[nSelectedCharacter].ToString();
            try
            {
                RefreshMapleGG(url);
            }
            catch { MessageBox.Show("Load Error!"); }
            try
            {
                HtmlWeb web = new HtmlWeb();
                var doc = web.Load(url);

                imgurl = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[1]/div/div[2]/img").Attributes["src"].Value;
                strLevel = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[2]/div[1]/ul/li[1]").InnerText.Split('.')[1];
                strJob = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[2]/div[1]/ul/li[2]").InnerText;

                ini[sections[nSelectedCharacter]]["img"] = imgurl;
                ini[sections[nSelectedCharacter]]["Level"] = strLevel;
                ini[sections[nSelectedCharacter]]["job"] = strJob;

                ini.Save(FilePath);
                ini.Load(FilePath);
                pictureBox1.ImageLocation = imgurl;
                try
                {
                    UpdateInfo(listBox_Characters.SelectedIndex);
                }
                catch { }

                

            }
            catch { }
            
        }
        private void RefreshMapleGG(string url)
        {
            //var url = "https://maple.gg/u/" + listBox_Characters.Items[nSelectedCharacter].ToString();
            try
            {
                webBrowser1.Navigate(url);
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
                webBrowser1.Document.GetElementById("btn-sync").InvokeMember("Click");
            }
            catch { MessageBox.Show("Load Error!"); }
        }
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            string homepageURL = "https://mapleplanner.synology.me";
            System.Diagnostics.Process.Start(homepageURL);
        }

        private void 계정연동ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userDB.Grade == UserGrade.GUEST)
            {
                kakaoLoginPage = new KakaoLogInPage();
                if(kakaoLoginPage.ShowDialog()==DialogResult.OK)
                {
                    kakaoManager.KakaoUserData();
                    kakaoManager.KakaoTokenData();

                    SQLManager.Insert(KakaoData.UserId, KakaoData.UserNickName, SQLManager.formatDateTime(DateTime.Now),0, UserGrade.브론즈IV, 0, hddserial);
                    //label6.Text = KakaoData.UserId;

                    userDB = SQLManager.GetUserDB(hddserial);
                    userPermissions = SQLManager.GetPermissions(userDB.Grade);

                    pictureBox1.Visible = true;

                    로그인ToolStripMenuItem.Text = "계정연동완료(" + KakaoData.UserId + ")";
                }                
            }
        }

        private void 계정연동해제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(userDB.Grade != UserGrade.GUEST)
            {
                if (MessageBox.Show("계정연동해제를 하면, 정보가 모두 초기화됩니다\n정말 초기화 하시겠습니까?\n(프로그램이 자동으로 재실행됩니다)", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    로그인ToolStripMenuItem.Text = "계정연동";
                    SQLManager.Delete(userDB.ID);
                    userDB = new UserInfo();
                    Application.Restart();
                    return;
                }
            }
        }
        private void WebBrowserVersionSetting()
        {
            int browserver = 0;
            using (WebBrowser wb = new WebBrowser())
            {
                int ie_emulation = 0;

                browserver = wb.Version.Major;
                if (browserver >= 11)
                    ie_emulation = 11001;
                else if (browserver == 10)
                    ie_emulation = 10001;
                else if (browserver == 9)
                    ie_emulation = 9999;
                else if (browserver == 8)
                    ie_emulation = 8888;
                else
                    ie_emulation = 7000;
            }
            if(browserver < 8)
            {
                if(MessageBox.Show("프로그램 최초등록을 위해 레지스트리 값 추가가 필요합니다.\n레지스트리 등록을 위해 관리자권한을 요청할 수 있습니다.","알림",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    Register(browserver);
            }
            
        }
        private void Register(int value)
        {
            string key;
            ProcessStartInfo regadd = new ProcessStartInfo();
            if (Environment.Is64BitProcess)
                key = @"""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION""";
            else
                key = @"""HKLM\SOFTWARE\WOW6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION""";
            string arg = string.Format("add {0} /v MaplePlanner.exe /t REG_DWORD /d {1} /f", key, value.ToString());

            regadd.Arguments = arg;
            regadd.WindowStyle = ProcessWindowStyle.Minimized;
            regadd.CreateNoWindow = true;
            regadd.FileName = "reg.exe";
            regadd.Verb = "runas";
            Process.Start(regadd);
        }
        //private void WebBrowserVersionSetting()
        //{
        //    RegistryKey registryKey = null; // 레지스트리 변경에 사용 될 변수

        //    int browserver = 0;
        //    int ie_emulation = 0;
        //    var targetApplication = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe"; // 현재 프로그램 이름

        //    // 사용자 IE 버전 확인
        //    using (WebBrowser wb = new WebBrowser())
        //    {
        //        browserver = wb.Version.Major;
        //        if (browserver >= 11)
        //            ie_emulation = 11001;
        //        else if (browserver == 10)
        //            ie_emulation = 10001;
        //        else if (browserver == 9)
        //            ie_emulation = 9999;
        //        else if (browserver == 8)
        //            ie_emulation = 8888;
        //        else
        //            ie_emulation = 7000;
        //    }
        //    try
        //    {
        //        registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
        //            @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

        //        // IE가 없으면 실행 불가능
        //        if (registryKey == null)
        //        {
        //            MessageBox.Show("IE no detected");
        //            Application.Exit();
        //            return;
        //        }

        //        string FindAppkey = Convert.ToString(registryKey.GetValue(targetApplication));
        //        // 이미 키가 있다면 종료
        //        if (FindAppkey == ie_emulation.ToString())
        //        {
        //            registryKey.Close();
        //            return;
        //        }

        //        // 키가 없으므로 키 셋팅
        //        registryKey.SetValue(targetApplication, unchecked((int)ie_emulation), RegistryValueKind.DWord);

        //        // 다시 키를 받아와서
        //        FindAppkey = Convert.ToString(registryKey.GetValue(targetApplication));

        //        // 현재 브라우저 버전이랑 동일 한지 판단
        //        if (FindAppkey == ie_emulation.ToString())
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            MessageBox.Show("https://mapleplanner.synology.me 에 방문하여 레지스트리 키를 다운받아 실행하세요");
        //            Application.Exit();
        //            return;
        //        }
        //    }
        //    catch
        //    {
        //        Application.Exit();
        //        return;
        //    }
        //    finally
        //    {
        //        // 키 메모리 해제
        //        if (registryKey != null)
        //        {
        //            registryKey.Close();
        //        }
        //    }
        //}

        private void 계정정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userDB = SQLManager.GetUserDB(hddserial);
            userPermissions = SQLManager.GetPermissions(userDB.Grade);
            ShowUserInfo dlg = new ShowUserInfo();
            dlg.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserGrade tmp = (UserGrade)Enum.Parse(typeof(UserGrade), textDebug.Text);
            SQLManager.SetGrade(1591937298, tmp);
            userDB = SQLManager.GetUserDB(hddserial);
            userPermissions = SQLManager.GetPermissions(userDB.Grade);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Script.HardDrive.MD5("test"));
        }

        private void button_CheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemCheckState(i, (false ? CheckState.Checked : CheckState.Unchecked));
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
    }
    public class RedTextRenderer : ToolStripRenderer
    {
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.OrangeRed;
            e.TextFont = new Font("Ariel", 10, FontStyle.Bold);
            base.OnRenderItemText(e);
        }
    }
}
