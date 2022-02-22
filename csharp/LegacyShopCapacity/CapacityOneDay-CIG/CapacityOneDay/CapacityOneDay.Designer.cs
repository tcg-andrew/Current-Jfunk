namespace CapacityOneDay
{
    partial class CapacityOneDay
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
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.textBoxCut = new System.Windows.Forms.TextBox();
            this.textBoxTemper = new System.Windows.Forms.TextBox();
            this.textBoxUnit = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxDate
            // 
            this.textBoxDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDate.Location = new System.Drawing.Point(2, 2);
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.ReadOnly = true;
            this.textBoxDate.Size = new System.Drawing.Size(60, 20);
            this.textBoxDate.TabIndex = 0;
            this.textBoxDate.TabStop = false;
            this.textBoxDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxCut
            // 
            this.textBoxCut.Location = new System.Drawing.Point(10, 26);
            this.textBoxCut.Name = "textBoxCut";
            this.textBoxCut.ReadOnly = true;
            this.textBoxCut.Size = new System.Drawing.Size(45, 20);
            this.textBoxCut.TabIndex = 1;
            this.textBoxCut.TabStop = false;
            this.textBoxCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTemper
            // 
            this.textBoxTemper.Location = new System.Drawing.Point(10, 50);
            this.textBoxTemper.Name = "textBoxTemper";
            this.textBoxTemper.ReadOnly = true;
            this.textBoxTemper.Size = new System.Drawing.Size(45, 20);
            this.textBoxTemper.TabIndex = 2;
            this.textBoxTemper.TabStop = false;
            this.textBoxTemper.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxUnit
            // 
            this.textBoxUnit.Location = new System.Drawing.Point(10, 74);
            this.textBoxUnit.Name = "textBoxUnit";
            this.textBoxUnit.ReadOnly = true;
            this.textBoxUnit.Size = new System.Drawing.Size(45, 20);
            this.textBoxUnit.TabIndex = 3;
            this.textBoxUnit.TabStop = false;
            this.textBoxUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // CapacityOneDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxUnit);
            this.Controls.Add(this.textBoxDate);
            this.Controls.Add(this.textBoxTemper);
            this.Controls.Add(this.textBoxCut);
            this.Name = "CapacityOneDay";
            this.Size = new System.Drawing.Size(65, 95);
            this.Load += new System.EventHandler(this.CapacityOneDay_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CapacityOneDay_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.TextBox textBoxCut;
        private System.Windows.Forms.TextBox textBoxTemper;
        private System.Windows.Forms.TextBox textBoxUnit;

    }
}
