namespace TrustSource
{
    partial class TrustSourceOptionsUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTrustSourceApiKey = new System.Windows.Forms.TextBox();
            this.txtTrustSourceUsername = new System.Windows.Forms.TextBox();
            this.lblTrustSourceUserName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAskOptional = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTrustSourceApiKey
            // 
            this.txtTrustSourceApiKey.Location = new System.Drawing.Point(89, 58);
            this.txtTrustSourceApiKey.Name = "txtTrustSourceApiKey";
            this.txtTrustSourceApiKey.Size = new System.Drawing.Size(318, 22);
            this.txtTrustSourceApiKey.TabIndex = 0;
            // 
            // txtTrustSourceUsername
            // 
            this.txtTrustSourceUsername.Location = new System.Drawing.Point(89, 30);
            this.txtTrustSourceUsername.Name = "txtTrustSourceUsername";
            this.txtTrustSourceUsername.Size = new System.Drawing.Size(318, 22);
            this.txtTrustSourceUsername.TabIndex = 1;
            // 
            // lblTrustSourceUserName
            // 
            this.lblTrustSourceUserName.AutoSize = true;
            this.lblTrustSourceUserName.Location = new System.Drawing.Point(6, 33);
            this.lblTrustSourceUserName.Name = "lblTrustSourceUserName";
            this.lblTrustSourceUserName.Size = new System.Drawing.Size(77, 17);
            this.lblTrustSourceUserName.TabIndex = 2;
            this.lblTrustSourceUserName.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Api Key:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(294, 86);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 35);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnSave_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAskOptional);
            this.groupBox1.Controls.Add(this.txtTrustSourceUsername);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.lblTrustSourceUserName);
            this.groupBox1.Controls.Add(this.txtTrustSourceApiKey);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 127);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TrustSource Credentials";
            // 
            // chkAskOptional
            // 
            this.chkAskOptional.AutoSize = true;
            this.chkAskOptional.Location = new System.Drawing.Point(89, 94);
            this.chkAskOptional.Name = "chkAskOptional";
            this.chkAskOptional.Size = new System.Drawing.Size(187, 21);
            this.chkAskOptional.TabIndex = 6;
            this.chkAskOptional.Text = "Ask Optional Paramteres";
            this.chkAskOptional.UseVisualStyleBackColor = true;
            // 
            // TrustSourceOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "TrustSourceOptionsUserControl";
            this.Size = new System.Drawing.Size(441, 518);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTrustSourceApiKey;
        private System.Windows.Forms.TextBox txtTrustSourceUsername;
        private System.Windows.Forms.Label lblTrustSourceUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAskOptional;
    }
}
