namespace MaplePlanner
{
    partial class Info
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Info));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxNickname = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSubject = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxContext = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(332, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "개선사항 및 불편사항은 아래 메일로 문의주세요";
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Location = new System.Drawing.Point(351, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "복사";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Location = new System.Drawing.Point(141, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(200, 18);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "mapleplanner2021@gmail.com";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBoxNickname
            // 
            this.textBoxNickname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNickname.Location = new System.Drawing.Point(71, 26);
            this.textBoxNickname.Name = "textBoxNickname";
            this.textBoxNickname.Size = new System.Drawing.Size(100, 25);
            this.textBoxNickname.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSubject);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxContext);
            this.groupBox1.Controls.Add(this.textBoxNickname);
            this.groupBox1.Location = new System.Drawing.Point(16, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(526, 341);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "바로 메일보내기";
            // 
            // textBoxSubject
            // 
            this.textBoxSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSubject.Location = new System.Drawing.Point(231, 26);
            this.textBoxSubject.Name = "textBoxSubject";
            this.textBoxSubject.Size = new System.Drawing.Size(285, 25);
            this.textBoxSubject.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(189, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "제목";
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button2.Location = new System.Drawing.Point(0, 410);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(558, 39);
            this.button2.TabIndex = 4;
            this.button2.Text = "전송하기";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "닉네임";
            // 
            // textBoxContext
            // 
            this.textBoxContext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxContext.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxContext.Location = new System.Drawing.Point(3, 66);
            this.textBoxContext.Multiline = true;
            this.textBoxContext.Name = "textBoxContext";
            this.textBoxContext.Size = new System.Drawing.Size(520, 272);
            this.textBoxContext.TabIndex = 3;
            this.textBoxContext.Text = "내용을 입력해주세요";
            // 
            // Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 449);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Info";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "메일문의";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Info_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxNickname;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxContext;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxSubject;
        private System.Windows.Forms.Label label3;
    }
}