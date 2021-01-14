namespace MaplePlanner
{
    partial class KakaoLogInPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KakaoLogInPage));
            this.textBox_Code = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_Code
            // 
            this.textBox_Code.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Code.Location = new System.Drawing.Point(199, 381);
            this.textBox_Code.MaxLength = 6;
            this.textBox_Code.Name = "textBox_Code";
            this.textBox_Code.Size = new System.Drawing.Size(127, 39);
            this.textBox_Code.TabIndex = 1;
            this.textBox_Code.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Code_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(15, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(480, 92);
            this.label1.TabIndex = 2;
            this.label1.Text = "※ 나와의 채팅 특성상 알림이 가지 않으니 직접 확인바랍니다\r\n\r\n※ \"나와의 채팅\" 이란?\r\n  본인 프로필 > 나와의 채팅";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(184, 440);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 39);
            this.button1.TabIndex = 4;
            this.button1.Text = "인증";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(333, 405);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "03:00";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(266, 440);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 39);
            this.button2.TabIndex = 6;
            this.button2.Text = "재전송";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(150, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(224, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "인증번호를 발송하였습니다.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(0, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(515, 23);
            this.label4.TabIndex = 8;
            this.label4.Text = "카카오톡 나와의 채팅 메시지를 확인 후 인증번호를 입력해주세요.";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(507, 636);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(33, 248);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "나와의 채팅";
            // 
            // KakaoLogInPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 636);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Code);
            this.Controls.Add(this.webBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "KakaoLogInPage";
            this.Text = "카카오 로그인";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KakaoLogInPage_FormClosing);
            this.Load += new System.EventHandler(this.KakaoLogInPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_Code;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label label5;
    }
}