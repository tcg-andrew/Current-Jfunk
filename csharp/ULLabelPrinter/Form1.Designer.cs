namespace ULLabelPrinter
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Part = new System.Windows.Forms.TextBox();
            this.tb_Job = new System.Windows.Forms.TextBox();
            this.tb_Heater = new System.Windows.Forms.TextBox();
            this.tb_Light = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Part #";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Job #";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Heater Amps";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Light Amps";
            // 
            // tb_Part
            // 
            this.tb_Part.Location = new System.Drawing.Point(53, 21);
            this.tb_Part.Name = "tb_Part";
            this.tb_Part.Size = new System.Drawing.Size(100, 20);
            this.tb_Part.TabIndex = 4;
            // 
            // tb_Job
            // 
            this.tb_Job.Location = new System.Drawing.Point(53, 63);
            this.tb_Job.Name = "tb_Job";
            this.tb_Job.Size = new System.Drawing.Size(100, 20);
            this.tb_Job.TabIndex = 5;
            // 
            // tb_Heater
            // 
            this.tb_Heater.Location = new System.Drawing.Point(250, 21);
            this.tb_Heater.Name = "tb_Heater";
            this.tb_Heater.Size = new System.Drawing.Size(100, 20);
            this.tb_Heater.TabIndex = 6;
            // 
            // tb_Light
            // 
            this.tb_Light.Location = new System.Drawing.Point(250, 63);
            this.tb_Light.Name = "tb_Light";
            this.tb_Light.Size = new System.Drawing.Size(100, 20);
            this.tb_Light.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(144, 121);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Print Label";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 162);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_Light);
            this.Controls.Add(this.tb_Heater);
            this.Controls.Add(this.tb_Job);
            this.Controls.Add(this.tb_Part);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "UL Label Printer";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Part;
        private System.Windows.Forms.TextBox tb_Job;
        private System.Windows.Forms.TextBox tb_Heater;
        private System.Windows.Forms.TextBox tb_Light;
        private System.Windows.Forms.Button button1;
    }
}

