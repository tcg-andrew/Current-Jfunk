namespace AgilentDataProcessor
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
            this.lbl_Summary = new System.Windows.Forms.Label();
            this.btn_Process = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_Summary
            // 
            this.lbl_Summary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Summary.Location = new System.Drawing.Point(0, 0);
            this.lbl_Summary.Name = "lbl_Summary";
            this.lbl_Summary.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.lbl_Summary.Size = new System.Drawing.Size(292, 109);
            this.lbl_Summary.TabIndex = 0;
            this.lbl_Summary.Text = "label1";
            this.lbl_Summary.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_Process
            // 
            this.btn_Process.Location = new System.Drawing.Point(69, 56);
            this.btn_Process.Name = "btn_Process";
            this.btn_Process.Size = new System.Drawing.Size(155, 23);
            this.btn_Process.TabIndex = 1;
            this.btn_Process.Text = "Process Data Files";
            this.btn_Process.UseVisualStyleBackColor = true;
            this.btn_Process.Click += new System.EventHandler(this.btn_Process_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 109);
            this.Controls.Add(this.btn_Process);
            this.Controls.Add(this.lbl_Summary);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agilent Scan Processor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Summary;
        private System.Windows.Forms.Button btn_Process;
    }
}

