using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Win32;

namespace MaplePlanner
{
    public partial class MainForm : Form
    {
        private KakaoLogInPage kakaoLoginPage;
        private KakaoManager kakaoManager;
        public MainForm()
        {
            InitializeComponent();
            WebBrowserVersionSetting();
            kakaoManager = new KakaoManager();
            //CheckVersion();

            //menuStrip1.Renderer = new RedTextRenderer();

            //label6.Text = "00시 까지 : " + DateTime.Today.AddDays(1).Subtract(DateTime.Now).ToString(@"HH\:mm\:ss");
            //label6.Text = "00시 까지 : " + DateTime.Today.AddDays(1).Subtract(DateTime.Now).ToString(@"hh\:mm\:ss");
            label7.Text = "현재시간 : " + DateTime.Now.ToString(@"HH\:mm\:ss");

            timer1.Interval = 1000;
            timer1.Start();

            try
            {
                //version = version.Remove(0, 2);
                //게시(Clickonce 등 일 경우) 응용프로그램 버전
                버전ToolStripMenuItem.Text = string.Format("ver. {0} (배포)",
                    System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString().Remove(0, 2));
            }
            catch
            {
                //로컬 빌드 버전일 경우 (현재 어셈블리 버전)
                버전ToolStripMenuItem.Text = string.Format("ver. {0} (빌드)",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(0,2));
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

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

        private readonly int limitedCharacter = 2;
        private void button_addCharacter_Click(object sender, EventArgs e)
        {
            string nickname = textBox_Nickname.Text;
            if (listBox_Characters.Items.Contains(nickname))
            {
                MessageBox.Show("캐릭터를 중복 등록할 수 없습니다","알림");
                textBox_Nickname.Focus();
                return;
            }
            if (listBox_Characters.Items.Count >= limitedCharacter)
            {
                MessageBox.Show("Lite버전 캐릭터 등록 갯수 제한 : " + limitedCharacter.ToString());
                textBox_Nickname.Text = string.Empty;
                textBox_Nickname.Focus();
                return;
            }
                
            if (!string.IsNullOrEmpty(nickname))
            {
                string level;
                string job;
                string imgurl;
                var url = "https://maple.gg/u/" + nickname;
                HtmlWeb web = new HtmlWeb();
                var doc1 = web.Load("https://maple.gg/search?q=" + nickname);
                
                try
                {
                    var doc = web.Load(url);

                    level = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[2]/div[1]/ul/li[1]").InnerText.Split('.')[1];
                    job = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[2]/div[1]/ul/li[2]").InnerText;
                    imgurl = doc.DocumentNode.SelectSingleNode("//*[@id='user-profile']/section/div/div[1]/div/div[2]/img").Attributes["src"].Value;
                }
                catch
                {
                    MessageBox.Show("존재하지 않는 닉네임입니다");
                    textBox_Nickname.Focus();
                    return;
                }

                int last;
                try{ last = Convert.ToInt32(sections[sections.Count - 1]) + 1; }
                catch { last = 1; }

                ini[last.ToString()]["Nickname"] = nickname;
                ini[last.ToString()]["img"] = imgurl;
                ini[last.ToString()]["Level"] = level;
                ini[last.ToString()]["job"] = job;
                ini[last.ToString()]["Plan0"] = string.Empty;
                ini[last.ToString()]["Plan1"] = string.Empty;
                ini[last.ToString()]["Plan2"] = string.Empty;
                ini[last.ToString()]["Plan3"] = string.Empty;
                ini[last.ToString()]["Plan4"] = string.Empty;
                ini[last.ToString()]["PlanCheck"] = ".....";
                ini[last.ToString()]["Symbol"] = 0;
                ini[last.ToString()]["DailyBoss"] = 0;
                ini[last.ToString()]["WeeklyBoss"] = 0;
                ini[last.ToString()]["MonthlyBoss"] = 0;

                sections.Add(last.ToString());

                ini.Save(FilePath);
                ini.Load(FilePath);

                listBox_Characters.Items.Add(nickname);
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
                pictureBox1.Visible = true;
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
            string plancheck = ini[sections[nSelectedCharacter]]["PlanCheck"].Value;
            if (checkedListBox1.Items.Count > 0)
            {
                button_RemovePlan.Enabled = true;
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (plancheck[i] == '!')
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
            var url = "https://maple.gg/u/" + listBox_Characters.Items[nSelectedCharacter].ToString();
            webBrowser1.Url = new Uri(url);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            string imgurl = string.Empty;
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

                ini[sections[nSelectedCharacter]]["img"] = imgurl;
                ini.Save(FilePath);
                ini.Load(FilePath);
                pictureBox1.ImageLocation = imgurl;
                //webBrowser1.Url = new Uri(url);

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

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemCheckState(i, (false ? CheckState.Checked : CheckState.Unchecked));
        }

        private void 로그인ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (로그인ToolStripMenuItem.Text == "로그인")
            {
                MessageBox.Show(webBrowser1.Version.ToString());

                kakaoLoginPage = new KakaoLogInPage();
                if(kakaoLoginPage.ShowDialog()==DialogResult.OK)
                {
                    로그인ToolStripMenuItem.Text = "로그아웃";
                    kakaoManager.KakaoUserData();
                    kakaoManager.KakaoTokenData();
                    SQLManager sql = new SQLManager();
                    sql.Insert(Convert.ToInt32(KakaoData.UserId), KakaoData.UserNickName, sql.formatDateTime(DateTime.Now), 0, 0, 0);
                    //label6.Text = KakaoData.UserId;
                }                
            }
            else if(로그인ToolStripMenuItem.Text == "로그아웃")
            {
                try { kakaoManager.KakaoTalkLogOut(); }
                catch { MessageBox.Show("로그아웃 중 오류가 발생하였습니다."); }
                
                로그인ToolStripMenuItem.Text = "로그인";
            }
            
        }
        private void WebBrowserVersionSetting()
        {
            RegistryKey registryKey = null; // 레지스트리 변경에 사용 될 변수

            int browserver = 0;
            int ie_emulation = 0;
            var targetApplication = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe"; // 현재 프로그램 이름

            // 사용자 IE 버전 확인
            using (WebBrowser wb = new WebBrowser())
            {
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

            try
            {
                registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

                // IE가 없으면 실행 불가능
                if (registryKey == null)
                {
                    MessageBox.Show("웹 브라우저 버전 초기화에 실패했습니다..!");
                    Application.Exit();
                    return;
                }

                string FindAppkey = Convert.ToString(registryKey.GetValue(targetApplication));

                // 이미 키가 있다면 종료
                if (FindAppkey == ie_emulation.ToString())
                {
                    registryKey.Close();
                    return;
                }

                // 키가 없으므로 키 셋팅
                registryKey.SetValue(targetApplication, unchecked((int)ie_emulation), RegistryValueKind.DWord);

                // 다시 키를 받아와서
                FindAppkey = Convert.ToString(registryKey.GetValue(targetApplication));

                // 현재 브라우저 버전이랑 동일 한지 판단
                if (FindAppkey == ie_emulation.ToString())
                {
                    return;
                }
                else
                {
                    MessageBox.Show("웹 브라우저 버전 초기화에 실패했습니다..!");
                    Application.Exit();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("웹 브라우저 버전 초기화에 실패했습니다..!");
                Application.Exit();
                return;
            }
            finally
            {
                // 키 메모리 해제
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
        }
        private void 계정ToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
