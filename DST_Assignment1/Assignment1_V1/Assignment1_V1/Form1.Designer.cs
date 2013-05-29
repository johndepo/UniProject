namespace Assignment1_V1
{
    partial class Form1
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
            this.lbl_Header = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_ID = new System.Windows.Forms.TextBox();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.cmd_Login = new System.Windows.Forms.Button();
            this.cmd_Logout = new System.Windows.Forms.Button();
            this.cmd_changeTutorial = new System.Windows.Forms.Button();
            this.cmd_UpdateTutorial = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_Session = new System.Windows.Forms.Label();
            this.txt_TuteSession = new System.Windows.Forms.TextBox();
            this.lst_Tutorials = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.cmd_Search = new System.Windows.Forms.Button();
            this.lst_Students = new System.Windows.Forms.ListBox();
            this.cmd_Remove = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Header
            // 
            this.lbl_Header.AutoSize = true;
            this.lbl_Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Header.Location = new System.Drawing.Point(66, 9);
            this.lbl_Header.Name = "lbl_Header";
            this.lbl_Header.Size = new System.Drawing.Size(111, 33);
            this.lbl_Header.TabIndex = 0;
            this.lbl_Header.Text = "LOGIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password";
            // 
            // txt_ID
            // 
            this.txt_ID.Location = new System.Drawing.Point(72, 86);
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Size = new System.Drawing.Size(100, 20);
            this.txt_ID.TabIndex = 3;
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(72, 125);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Size = new System.Drawing.Size(100, 20);
            this.txt_Password.TabIndex = 4;
            // 
            // cmd_Login
            // 
            this.cmd_Login.Location = new System.Drawing.Point(83, 167);
            this.cmd_Login.Name = "cmd_Login";
            this.cmd_Login.Size = new System.Drawing.Size(75, 23);
            this.cmd_Login.TabIndex = 5;
            this.cmd_Login.Text = "Login";
            this.cmd_Login.UseVisualStyleBackColor = true;
            this.cmd_Login.Click += new System.EventHandler(this.cmd_Login_Click);
            // 
            // cmd_Logout
            // 
            this.cmd_Logout.Location = new System.Drawing.Point(83, 197);
            this.cmd_Logout.Name = "cmd_Logout";
            this.cmd_Logout.Size = new System.Drawing.Size(75, 23);
            this.cmd_Logout.TabIndex = 6;
            this.cmd_Logout.Text = "Logout";
            this.cmd_Logout.UseVisualStyleBackColor = true;
            this.cmd_Logout.Click += new System.EventHandler(this.cmd_Logout_Click);
            // 
            // cmd_changeTutorial
            // 
            this.cmd_changeTutorial.Location = new System.Drawing.Point(38, 200);
            this.cmd_changeTutorial.Name = "cmd_changeTutorial";
            this.cmd_changeTutorial.Size = new System.Drawing.Size(120, 23);
            this.cmd_changeTutorial.TabIndex = 7;
            this.cmd_changeTutorial.Text = "Change Tutorial";
            this.cmd_changeTutorial.UseVisualStyleBackColor = true;
            this.cmd_changeTutorial.Click += new System.EventHandler(this.cmd_changeTutorial_Click_1);
            // 
            // cmd_UpdateTutorial
            // 
            this.cmd_UpdateTutorial.Location = new System.Drawing.Point(38, 32);
            this.cmd_UpdateTutorial.Name = "cmd_UpdateTutorial";
            this.cmd_UpdateTutorial.Size = new System.Drawing.Size(120, 23);
            this.cmd_UpdateTutorial.TabIndex = 9;
            this.cmd_UpdateTutorial.Text = "List Tutorials";
            this.cmd_UpdateTutorial.UseVisualStyleBackColor = true;
            this.cmd_UpdateTutorial.Click += new System.EventHandler(this.cmd_UpdateTutorial_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_Session);
            this.groupBox1.Controls.Add(this.txt_TuteSession);
            this.groupBox1.Controls.Add(this.lst_Tutorials);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmd_UpdateTutorial);
            this.groupBox1.Controls.Add(this.cmd_changeTutorial);
            this.groupBox1.Location = new System.Drawing.Point(23, 226);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 229);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // lbl_Session
            // 
            this.lbl_Session.AutoSize = true;
            this.lbl_Session.Location = new System.Drawing.Point(17, 16);
            this.lbl_Session.Name = "lbl_Session";
            this.lbl_Session.Size = new System.Drawing.Size(82, 13);
            this.lbl_Session.TabIndex = 12;
            this.lbl_Session.Text = "Current Tutorial ";
            this.lbl_Session.Click += new System.EventHandler(this.lbl_Session_Click);
            // 
            // txt_TuteSession
            // 
            this.txt_TuteSession.Location = new System.Drawing.Point(50, 174);
            this.txt_TuteSession.Name = "txt_TuteSession";
            this.txt_TuteSession.Size = new System.Drawing.Size(100, 20);
            this.txt_TuteSession.TabIndex = 10;
            // 
            // lst_Tutorials
            // 
            this.lst_Tutorials.FormattingEnabled = true;
            this.lst_Tutorials.Location = new System.Drawing.Point(38, 73);
            this.lst_Tutorials.Name = "lst_Tutorials";
            this.lst_Tutorials.Size = new System.Drawing.Size(120, 95);
            this.lst_Tutorials.TabIndex = 11;
            this.lst_Tutorials.SelectedIndexChanged += new System.EventHandler(this.lst_Tutorials_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Session ; Tutor ; Time ; Room";
            // 
            // txt_Search
            // 
            this.txt_Search.Location = new System.Drawing.Point(23, 587);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(100, 20);
            this.txt_Search.TabIndex = 13;
            // 
            // cmd_Search
            // 
            this.cmd_Search.Location = new System.Drawing.Point(129, 584);
            this.cmd_Search.Name = "cmd_Search";
            this.cmd_Search.Size = new System.Drawing.Size(94, 23);
            this.cmd_Search.TabIndex = 14;
            this.cmd_Search.Text = "Search";
            this.cmd_Search.UseVisualStyleBackColor = true;
            this.cmd_Search.Click += new System.EventHandler(this.cmd_Search_Click);
            // 
            // lst_Students
            // 
            this.lst_Students.FormattingEnabled = true;
            this.lst_Students.Location = new System.Drawing.Point(25, 483);
            this.lst_Students.Name = "lst_Students";
            this.lst_Students.Size = new System.Drawing.Size(133, 95);
            this.lst_Students.TabIndex = 15;
            this.lst_Students.SelectedIndexChanged += new System.EventHandler(this.lst_Students_SelectedIndexChanged);
            // 
            // cmd_Remove
            // 
            this.cmd_Remove.Location = new System.Drawing.Point(164, 483);
            this.cmd_Remove.Name = "cmd_Remove";
            this.cmd_Remove.Size = new System.Drawing.Size(59, 95);
            this.cmd_Remove.TabIndex = 16;
            this.cmd_Remove.Text = "Remove User";
            this.cmd_Remove.UseVisualStyleBackColor = true;
            this.cmd_Remove.Click += new System.EventHandler(this.cmd_Remove_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 631);
            this.Controls.Add(this.cmd_Remove);
            this.Controls.Add(this.lst_Students);
            this.Controls.Add(this.cmd_Search);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmd_Logout);
            this.Controls.Add(this.cmd_Login);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.txt_ID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_Header);
            this.Name = "Form1";
            this.Text = "John Agbulos 3030429";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Header;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_ID;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.Button cmd_Login;
        private System.Windows.Forms.Button cmd_Logout;
        private System.Windows.Forms.Button cmd_changeTutorial;
        private System.Windows.Forms.Button cmd_UpdateTutorial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_TuteSession;
        private System.Windows.Forms.ListBox lst_Tutorials;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_Session;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Button cmd_Search;
        private System.Windows.Forms.ListBox lst_Students;
        private System.Windows.Forms.Button cmd_Remove;
    }
}

