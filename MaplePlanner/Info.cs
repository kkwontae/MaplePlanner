using System;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

namespace MaplePlanner
{
    public partial class Info : Form
    {
        public Info()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxNickname.Text == "" || textBoxSubject.Text == "" || textBoxContext.Text == "")
            {
                MessageBox.Show("닉네임, 제목, 내용을 모두 입력해주세요");
            }
            else
            {
                MailAddress ma_from = new MailAddress("mapleplanner2021@gmail.com", textBoxNickname.Text);
                MailAddress ma_to = new MailAddress("mapleplanner2021@gmail.com", "mapleplanner");
                string s_password = "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";
                string s_subject = textBoxSubject.Text;
                string s_body = textBoxContext.Text;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.sendgrid.net",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("apikey", s_password)
                };

                using (MailMessage mail = new MailMessage(ma_from, ma_to)
                {
                    Subject = s_subject,
                    Body = s_body,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                    Priority = MailPriority.High 
                })

                    try
                    {
                        button2.Text = "전송 중";
                        button2.Enabled = false;
                        smtp.Send(mail);
                        MessageBox.Show("성공적으로 메일을 전송하였습니다");
                    }
                    catch
                    {
                        MessageBox.Show("메일 전송에 오류가 발생했습니다.\n반복적으로 오류가 발생할 경우 관리자에게 문의하세요","오류");
                    }
                    finally
                    {
                        button2.Text = "전송하기";
                        button2.Enabled = true;
                    }

            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (textBoxContext.TextLength > 0 && textBoxContext.Text!= "내용을 입력해주세요")
                {
                    if (MessageBox.Show("작성 중인 메일은 저장되지 않습니다!\n그래도 나가시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.Close();
                        return true;
                    }
                }
                else
                {
                    this.Close();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Info_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBoxContext.TextLength > 0 && textBoxContext.Text != "내용을 입력해주세요")
            {
                if (MessageBox.Show("작성 중인 메일은 저장되지 않습니다!\n그래도 나가시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
            }
        }
    }
}
