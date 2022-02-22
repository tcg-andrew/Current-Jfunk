namespace CRDProdLabel
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_Asm = new System.Windows.Forms.TextBox();
            this.chk_Asm = new System.Windows.Forms.CheckBox();
            this.txt_Part = new System.Windows.Forms.TextBox();
            this.cbox_Part = new System.Windows.Forms.CheckBox();
            this.txt_SO = new System.Windows.Forms.TextBox();
            this.cbox_SO = new System.Windows.Forms.CheckBox();
            this.date_To = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.date_From = new System.Windows.Forms.DateTimePicker();
            this.cbox_Date = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_Asm);
            this.groupBox1.Controls.Add(this.chk_Asm);
            this.groupBox1.Controls.Add(this.txt_Part);
            this.groupBox1.Controls.Add(this.cbox_Part);
            this.groupBox1.Controls.Add(this.txt_SO);
            this.groupBox1.Controls.Add(this.cbox_SO);
            this.groupBox1.Controls.Add(this.date_To);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.date_From);
            this.groupBox1.Controls.Add(this.cbox_Date);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // txt_Asm
            // 
            this.txt_Asm.Location = new System.Drawing.Point(109, 98);
            this.txt_Asm.Name = "txt_Asm";
            this.txt_Asm.Size = new System.Drawing.Size(121, 20);
            this.txt_Asm.TabIndex = 9;
            // 
            // chk_Asm
            // 
            this.chk_Asm.AutoSize = true;
            this.chk_Asm.Location = new System.Drawing.Point(8, 101);
            this.chk_Asm.Name = "chk_Asm";
            this.chk_Asm.Size = new System.Drawing.Size(80, 17);
            this.chk_Asm.TabIndex = 8;
            this.chk_Asm.Text = "Assembly #";
            this.chk_Asm.UseVisualStyleBackColor = true;
            // 
            // txt_Part
            // 
            this.txt_Part.Location = new System.Drawing.Point(109, 72);
            this.txt_Part.Name = "txt_Part";
            this.txt_Part.Size = new System.Drawing.Size(121, 20);
            this.txt_Part.TabIndex = 7;
            // 
            // cbox_Part
            // 
            this.cbox_Part.AutoSize = true;
            this.cbox_Part.Location = new System.Drawing.Point(8, 74);
            this.cbox_Part.Name = "cbox_Part";
            this.cbox_Part.Size = new System.Drawing.Size(95, 17);
            this.cbox_Part.TabIndex = 6;
            this.cbox_Part.Text = "Part starts with";
            this.cbox_Part.UseVisualStyleBackColor = true;
            // 
            // txt_SO
            // 
            this.txt_SO.Location = new System.Drawing.Point(109, 46);
            this.txt_SO.Name = "txt_SO";
            this.txt_SO.Size = new System.Drawing.Size(132, 20);
            this.txt_SO.TabIndex = 5;
            this.txt_SO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_SO_KeyPress);
            // 
            // cbox_SO
            // 
            this.cbox_SO.AutoSize = true;
            this.cbox_SO.Location = new System.Drawing.Point(8, 48);
            this.cbox_SO.Name = "cbox_SO";
            this.cbox_SO.Size = new System.Drawing.Size(53, 17);
            this.cbox_SO.TabIndex = 4;
            this.cbox_SO.Text = "Job #";
            this.cbox_SO.UseVisualStyleBackColor = true;
            // 
            // date_To
            // 
            this.date_To.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.date_To.Location = new System.Drawing.Point(236, 20);
            this.date_To.Name = "date_To";
            this.date_To.Size = new System.Drawing.Size(98, 20);
            this.date_To.TabIndex = 3;
            this.date_To.ValueChanged += new System.EventHandler(this.date_To_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(214, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "to";
            // 
            // date_From
            // 
            this.date_From.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.date_From.Location = new System.Drawing.Point(109, 20);
            this.date_From.Name = "date_From";
            this.date_From.Size = new System.Drawing.Size(98, 20);
            this.date_From.TabIndex = 1;
            this.date_From.ValueChanged += new System.EventHandler(this.date_From_ValueChanged);
            // 
            // cbox_Date
            // 
            this.cbox_Date.AutoSize = true;
            this.cbox_Date.Location = new System.Drawing.Point(8, 20);
            this.cbox_Date.Name = "cbox_Date";
            this.cbox_Date.Size = new System.Drawing.Size(84, 17);
            this.cbox_Date.TabIndex = 0;
            this.cbox_Date.Text = "Date Range";
            this.cbox_Date.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 180);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Preview";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 215);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CRD Production Labels";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker date_To;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker date_From;
        private System.Windows.Forms.CheckBox cbox_Date;
        private System.Windows.Forms.TextBox txt_SO;
        private System.Windows.Forms.CheckBox cbox_SO;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_Part;
        private System.Windows.Forms.CheckBox cbox_Part;
        private System.Windows.Forms.TextBox txt_Asm;
        private System.Windows.Forms.CheckBox chk_Asm;
    }
}

