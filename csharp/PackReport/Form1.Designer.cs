namespace PackReport
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.btn_Report = new System.Windows.Forms.Button();
            this.btn_Done = new System.Windows.Forms.Button();
            this.rb_ShopCap = new System.Windows.Forms.RadioButton();
            this.rb_Ship = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End Date";
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(74, 26);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(200, 20);
            this.startDate.TabIndex = 2;
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(74, 56);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(200, 20);
            this.endDate.TabIndex = 3;
            // 
            // btn_Report
            // 
            this.btn_Report.Location = new System.Drawing.Point(76, 131);
            this.btn_Report.Name = "btn_Report";
            this.btn_Report.Size = new System.Drawing.Size(75, 23);
            this.btn_Report.TabIndex = 4;
            this.btn_Report.Text = "Run Report";
            this.btn_Report.UseVisualStyleBackColor = true;
            this.btn_Report.Click += new System.EventHandler(this.btn_Report_Click);
            // 
            // btn_Done
            // 
            this.btn_Done.Location = new System.Drawing.Point(157, 131);
            this.btn_Done.Name = "btn_Done";
            this.btn_Done.Size = new System.Drawing.Size(75, 23);
            this.btn_Done.TabIndex = 5;
            this.btn_Done.Text = "Done";
            this.btn_Done.UseVisualStyleBackColor = true;
            this.btn_Done.Click += new System.EventHandler(this.btn_Done_Click);
            // 
            // rb_ShopCap
            // 
            this.rb_ShopCap.AutoSize = true;
            this.rb_ShopCap.Checked = true;
            this.rb_ShopCap.Location = new System.Drawing.Point(64, 96);
            this.rb_ShopCap.Name = "rb_ShopCap";
            this.rb_ShopCap.Size = new System.Drawing.Size(72, 17);
            this.rb_ShopCap.TabIndex = 6;
            this.rb_ShopCap.TabStop = true;
            this.rb_ShopCap.Text = "Shop Cap";
            this.rb_ShopCap.UseVisualStyleBackColor = true;
            // 
            // rb_Ship
            // 
            this.rb_Ship.AutoSize = true;
            this.rb_Ship.Location = new System.Drawing.Point(172, 96);
            this.rb_Ship.Name = "rb_Ship";
            this.rb_Ship.Size = new System.Drawing.Size(72, 17);
            this.rb_Ship.TabIndex = 7;
            this.rb_Ship.TabStop = true;
            this.rb_Ship.Text = "Ship Date";
            this.rb_Ship.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 192);
            this.Controls.Add(this.rb_Ship);
            this.Controls.Add(this.rb_ShopCap);
            this.Controls.Add(this.btn_Done);
            this.Controls.Add(this.btn_Report);
            this.Controls.Add(this.endDate);
            this.Controls.Add(this.startDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.Button btn_Report;
        private System.Windows.Forms.Button btn_Done;
        private System.Windows.Forms.RadioButton rb_ShopCap;
        private System.Windows.Forms.RadioButton rb_Ship;
    }
}

