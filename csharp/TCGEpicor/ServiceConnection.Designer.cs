namespace TCGEpicor
{
    partial class ServiceConnection
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
            this.lbl_ErrorSummary = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_PasswordError = new System.Windows.Forms.Label();
            this.lbl_UsernameError = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_ErrorSummary
            // 
            this.lbl_ErrorSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_ErrorSummary.AutoSize = true;
            this.lbl_ErrorSummary.ForeColor = System.Drawing.Color.Red;
            this.lbl_ErrorSummary.Location = new System.Drawing.Point(96, 142);
            this.lbl_ErrorSummary.Margin = new System.Windows.Forms.Padding(36, 0, 36, 3);
            this.lbl_ErrorSummary.Name = "lbl_ErrorSummary";
            this.lbl_ErrorSummary.Size = new System.Drawing.Size(35, 13);
            this.lbl_ErrorSummary.TabIndex = 29;
            this.lbl_ErrorSummary.Text = "label5";
            this.lbl_ErrorSummary.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_PasswordError);
            this.panel1.Controls.Add(this.lbl_UsernameError);
            this.panel1.Controls.Add(this.butCancel);
            this.panel1.Controls.Add(this.btnAccept);
            this.panel1.Controls.Add(this.txtPass);
            this.panel1.Controls.Add(this.txtUser);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(298, 127);
            this.panel1.TabIndex = 30;
            // 
            // lbl_PasswordError
            // 
            this.lbl_PasswordError.AutoSize = true;
            this.lbl_PasswordError.ForeColor = System.Drawing.Color.Red;
            this.lbl_PasswordError.Location = new System.Drawing.Point(236, 47);
            this.lbl_PasswordError.Name = "lbl_PasswordError";
            this.lbl_PasswordError.Size = new System.Drawing.Size(11, 13);
            this.lbl_PasswordError.TabIndex = 42;
            this.lbl_PasswordError.Text = "*";
            this.lbl_PasswordError.Visible = false;
            // 
            // lbl_UsernameError
            // 
            this.lbl_UsernameError.AutoSize = true;
            this.lbl_UsernameError.ForeColor = System.Drawing.Color.Red;
            this.lbl_UsernameError.Location = new System.Drawing.Point(236, 21);
            this.lbl_UsernameError.Name = "lbl_UsernameError";
            this.lbl_UsernameError.Size = new System.Drawing.Size(11, 13);
            this.lbl_UsernameError.TabIndex = 41;
            this.lbl_UsernameError.Text = "*";
            this.lbl_UsernameError.Visible = false;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(44, 87);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 37;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(180, 87);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 36;
            this.btnAccept.Text = "Connect";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(112, 44);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(118, 20);
            this.txtPass.TabIndex = 32;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(112, 18);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(118, 20);
            this.txtUser.TabIndex = 31;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(53, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "Password";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(51, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "Username";
            // 
            // ServiceConnection
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(324, 165);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbl_ErrorSummary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServiceConnection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log On";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_ErrorSummary;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_PasswordError;
        private System.Windows.Forms.Label lbl_UsernameError;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
    }
}