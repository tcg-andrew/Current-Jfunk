namespace Test
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
            this.capacityOneDay1 = new CapacityOneDay.CapacityOneDay();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // capacityOneDay1
            // 
            this.capacityOneDay1.DisplayDate = new System.DateTime(((long)(0)));
            this.capacityOneDay1.Doors = 0;
            this.capacityOneDay1.Frames = 0;
            this.capacityOneDay1.GreenToYellowDoors = 0;
            this.capacityOneDay1.GreenToYellowFrames = 0;
            this.capacityOneDay1.InSuper7 = ((byte)(0));
            this.capacityOneDay1.Location = new System.Drawing.Point(56, 31);
            this.capacityOneDay1.MaxDoors = 0;
            this.capacityOneDay1.MaxFrames = 0;
            this.capacityOneDay1.Name = "capacityOneDay1";
            this.capacityOneDay1.Size = new System.Drawing.Size(65, 70);
            this.capacityOneDay1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 161);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.capacityOneDay1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private CapacityOneDay.CapacityOneDay capacityOneDay1;
        private System.Windows.Forms.Button button1;
    }
}

