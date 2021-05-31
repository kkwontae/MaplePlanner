using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaplePlanner
{
    public partial class PatchNotes : Form
    {
        public PatchNotes()
        {
            InitializeComponent();
        }

        private void PatchNotes_Load(object sender, EventArgs e)
        {
            string version;
            try
            {
                //게시(Clickonce 등 일 경우) 응용프로그램 버전
                version = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString().Remove(0, 2);
            }
            catch
            {
                //로컬 빌드 버전일 경우 (현재 어셈블리 버전)
                version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(0, 2);
            }

            label1.Text = @"███╗   ███╗ █████╗ ██████╗ ██╗     ███████╗                 
████╗ ████║██╔══██╗██╔══██╗██║     ██╔════╝                 
██╔████╔██║███████║██████╔╝██║     █████╗                   
██║╚██╔╝██║██╔══██║██╔═══╝ ██║     ██╔══╝                   
██║ ╚═╝ ██║██║  ██║██║     ███████╗███████╗                 
╚═╝     ╚═╝╚═╝  ╚═╝╚═╝     ╚══════╝╚══════╝                 
                                                            
██████╗ ██╗      █████╗ ███╗   ██╗███╗   ██╗███████╗██████╗ 
██╔══██╗██║     ██╔══██╗████╗  ██║████╗  ██║██╔════╝██╔══██╗
██████╔╝██║     ███████║██╔██╗ ██║██╔██╗ ██║█████╗  ██████╔╝
██╔═══╝ ██║     ██╔══██║██║╚██╗██║██║╚██╗██║██╔══╝  ██╔══██╗
██║     ███████╗██║  ██║██║ ╚████║██║ ╚████║███████╗██║  ██║
╚═╝     ╚══════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝
* 버그 및 건의 사항 제보는 프로그램 내 문의 탭을 사용해주세요";
            
               
            try
            {
                var filename = System.Reflection.Assembly.GetExecutingAssembly().Location;
                
                var req = HttpWebRequest.CreateHttp("http://mapleplanner.synology.me/Mapleplanner/PatchNotes/" + version + ".txt");

                using (var res = req.GetResponse())
                {
                    using (var stream = res.GetResponseStream())
                    {
                        using (var reader = new System.IO.StreamReader(stream))
                        {
                            var notes = reader.ReadToEnd();
                            textBox1.Text = notes;
                        }
                    }
                }
            }
            catch { }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
