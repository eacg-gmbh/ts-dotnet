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
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAskOptional = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTrustSourceApiKey
            // 
            this.txtTrustSourceApiKey.Location = new System.Drawing.Point(132, 51);
            this.txtTrustSourceApiKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTrustSourceApiKey.Name = "txtTrustSourceApiKey";
            this.txtTrustSourceApiKey.Size = new System.Drawing.Size(475, 31);
            this.txtTrustSourceApiKey.TabIndex = 0;
            this.txtTrustSourceApiKey.TextChanged += new System.EventHandler(this.txtTrustSourceApiKey_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 55);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Api Key:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAskOptional);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTrustSourceApiKey);
            this.groupBox1.Location = new System.Drawing.Point(4, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(620, 160);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TrustSource Credentials";
            // 
            // chkAskOptional
            // 
            this.chkAskOptional.AutoSize = true;
            this.chkAskOptional.Location = new System.Drawing.Point(132, 94);
            this.chkAskOptional.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAskOptional.Name = "chkAskOptional";
            this.chkAskOptional.Size = new System.Drawing.Size(282, 29);
            this.chkAskOptional.TabIndex = 6;
            this.chkAskOptional.Text = "Ask Optional Paramteres";
            this.chkAskOptional.UseVisualStyleBackColor = true;
            this.chkAskOptional.CheckedChanged += new System.EventHandler(this.chkAskOptional_CheckedChanged);
            // 
            // TrustSourceOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TrustSourceOptionsUserControl";
            this.Size = new System.Drawing.Size(662, 809);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTrustSourceApiKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAskOptional;
    }
}
