namespace Assignment1_V1
{
    partial class StaffForm
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
            this.lst_Students = new System.Windows.Forms.ListBox();
            this.cmd_Remove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lst_Students
            // 
            this.lst_Students.FormattingEnabled = true;
            this.lst_Students.Location = new System.Drawing.Point(12, 12);
            this.lst_Students.Name = "lst_Students";
            this.lst_Students.Size = new System.Drawing.Size(268, 160);
            this.lst_Students.TabIndex = 2;
            this.lst_Students.SelectedIndexChanged += new System.EventHandler(this.lst_Tutorials_SelectedIndexChanged);
            // 
            // cmd_Remove
            // 
            this.cmd_Remove.Location = new System.Drawing.Point(107, 178);
            this.cmd_Remove.Name = "cmd_Remove";
            this.cmd_Remove.Size = new System.Drawing.Size(75, 23);
            this.cmd_Remove.TabIndex = 3;
            this.cmd_Remove.Text = "Remove";
            this.cmd_Remove.UseVisualStyleBackColor = true;
            // 
            // StaffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.cmd_Remove);
            this.Controls.Add(this.lst_Students);
            this.Name = "StaffForm";
            this.Text = "StaffForm";
            this.Load += new System.EventHandler(this.StaffForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lst_Students;
        private System.Windows.Forms.Button cmd_Remove;
    }
}