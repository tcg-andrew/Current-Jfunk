namespace PowerAnalyzer
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOptVolt = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lblOhmLow = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblOhmHigh = new System.Windows.Forms.Label();
            this.pnlVoltage = new System.Windows.Forms.Panel();
            this.lblVoltage = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlOhms = new System.Windows.Forms.Panel();
            this.lblOhms = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlAmps = new System.Windows.Forms.Panel();
            this.lblAmps = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblWatts = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblPF = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.ddl_Plant = new System.Windows.Forms.ComboBox();
            this.pnlVoltage.SuspendLayout();
            this.pnlOhms.SuspendLayout();
            this.pnlAmps.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(100, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(319, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Unit Num";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(437, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Retrieve";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Opt Voltage:";
            // 
            // lblOptVolt
            // 
            this.lblOptVolt.AutoSize = true;
            this.lblOptVolt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptVolt.Location = new System.Drawing.Point(117, 53);
            this.lblOptVolt.Name = "lblOptVolt";
            this.lblOptVolt.Size = new System.Drawing.Size(17, 17);
            this.lblOptVolt.TabIndex = 4;
            this.lblOptVolt.Text = "0";
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3.Location = new System.Drawing.Point(12, 80);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(79, 17);
            this.lbl3.TabIndex = 5;
            this.lbl3.Text = "Ohm Low:";
            // 
            // lblOhmLow
            // 
            this.lblOhmLow.AutoSize = true;
            this.lblOhmLow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOhmLow.Location = new System.Drawing.Point(117, 80);
            this.lblOhmLow.Name = "lblOhmLow";
            this.lblOhmLow.Size = new System.Drawing.Size(17, 17);
            this.lblOhmLow.TabIndex = 6;
            this.lblOhmLow.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Ohm High:";
            // 
            // lblOhmHigh
            // 
            this.lblOhmHigh.AutoSize = true;
            this.lblOhmHigh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOhmHigh.Location = new System.Drawing.Point(117, 106);
            this.lblOhmHigh.Name = "lblOhmHigh";
            this.lblOhmHigh.Size = new System.Drawing.Size(17, 17);
            this.lblOhmHigh.TabIndex = 8;
            this.lblOhmHigh.Text = "0";
            // 
            // pnlVoltage
            // 
            this.pnlVoltage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVoltage.Controls.Add(this.lblVoltage);
            this.pnlVoltage.Controls.Add(this.label4);
            this.pnlVoltage.Location = new System.Drawing.Point(178, 53);
            this.pnlVoltage.Name = "pnlVoltage";
            this.pnlVoltage.Size = new System.Drawing.Size(167, 40);
            this.pnlVoltage.TabIndex = 9;
            // 
            // lblVoltage
            // 
            this.lblVoltage.AutoSize = true;
            this.lblVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVoltage.Location = new System.Drawing.Point(86, 10);
            this.lblVoltage.Name = "lblVoltage";
            this.lblVoltage.Size = new System.Drawing.Size(17, 17);
            this.lblVoltage.TabIndex = 10;
            this.lblVoltage.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Voltage:";
            // 
            // pnlOhms
            // 
            this.pnlOhms.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOhms.Controls.Add(this.lblOhms);
            this.pnlOhms.Controls.Add(this.label5);
            this.pnlOhms.Location = new System.Drawing.Point(351, 53);
            this.pnlOhms.Name = "pnlOhms";
            this.pnlOhms.Size = new System.Drawing.Size(167, 40);
            this.pnlOhms.TabIndex = 10;
            // 
            // lblOhms
            // 
            this.lblOhms.AutoSize = true;
            this.lblOhms.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOhms.Location = new System.Drawing.Point(72, 9);
            this.lblOhms.Name = "lblOhms";
            this.lblOhms.Size = new System.Drawing.Size(17, 17);
            this.lblOhms.TabIndex = 1;
            this.lblOhms.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Ohms:";
            // 
            // pnlAmps
            // 
            this.pnlAmps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAmps.Controls.Add(this.lblAmps);
            this.pnlAmps.Controls.Add(this.label6);
            this.pnlAmps.Location = new System.Drawing.Point(178, 99);
            this.pnlAmps.Name = "pnlAmps";
            this.pnlAmps.Size = new System.Drawing.Size(167, 39);
            this.pnlAmps.TabIndex = 11;
            // 
            // lblAmps
            // 
            this.lblAmps.AutoSize = true;
            this.lblAmps.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmps.Location = new System.Drawing.Point(86, 9);
            this.lblAmps.Name = "lblAmps";
            this.lblAmps.Size = new System.Drawing.Size(17, 17);
            this.lblAmps.TabIndex = 1;
            this.lblAmps.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Amps:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblWatts);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(351, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 39);
            this.panel1.TabIndex = 12;
            // 
            // lblWatts
            // 
            this.lblWatts.AutoSize = true;
            this.lblWatts.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWatts.Location = new System.Drawing.Point(72, 9);
            this.lblWatts.Name = "lblWatts";
            this.lblWatts.Size = new System.Drawing.Size(17, 17);
            this.lblWatts.TabIndex = 1;
            this.lblWatts.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Watts:";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblPF);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Location = new System.Drawing.Point(178, 144);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(278, 41);
            this.panel2.TabIndex = 13;
            // 
            // lblPF
            // 
            this.lblPF.AutoSize = true;
            this.lblPF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPF.Location = new System.Drawing.Point(174, 9);
            this.lblPF.Name = "lblPF";
            this.lblPF.Size = new System.Drawing.Size(17, 17);
            this.lblPF.TabIndex = 1;
            this.lblPF.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(60, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Power Factor:";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(188, 230);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(161, 36);
            this.button2.TabIndex = 14;
            this.button2.Text = "Record And Print";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ddl_Plant
            // 
            this.ddl_Plant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Plant.FormattingEnabled = true;
            this.ddl_Plant.IntegralHeight = false;
            this.ddl_Plant.Items.AddRange(new object[] {
            "FL",
            "TN"});
            this.ddl_Plant.Location = new System.Drawing.Point(15, 230);
            this.ddl_Plant.Name = "ddl_Plant";
            this.ddl_Plant.Size = new System.Drawing.Size(55, 21);
            this.ddl_Plant.TabIndex = 15;
            this.ddl_Plant.SelectedIndexChanged += new System.EventHandler(this.ddl_Plant_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 301);
            this.Controls.Add(this.ddl_Plant);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlAmps);
            this.Controls.Add(this.pnlOhms);
            this.Controls.Add(this.pnlVoltage);
            this.Controls.Add(this.lblOhmHigh);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblOhmLow);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.lblOptVolt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "CIG PA Labels";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.pnlVoltage.ResumeLayout(false);
            this.pnlVoltage.PerformLayout();
            this.pnlOhms.ResumeLayout(false);
            this.pnlOhms.PerformLayout();
            this.pnlAmps.ResumeLayout(false);
            this.pnlAmps.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOptVolt;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lblOhmLow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOhmHigh;
        private System.Windows.Forms.Panel pnlVoltage;
        private System.Windows.Forms.Label lblVoltage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlOhms;
        private System.Windows.Forms.Label lblOhms;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlAmps;
        private System.Windows.Forms.Label lblAmps;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblWatts;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblPF;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox ddl_Plant;

    }
}

