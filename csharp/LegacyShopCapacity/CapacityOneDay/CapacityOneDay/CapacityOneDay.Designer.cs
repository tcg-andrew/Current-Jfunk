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
            this.components = new System.ComponentModel.Container();
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.frameDoorOpeningsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dayDetailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxDoors = new System.Windows.Forms.TextBox();
            this.textBoxFrames = new System.Windows.Forms.TextBox();
            this.dayDetailAndAllPriorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxDate
            // 
            this.textBoxDate.ContextMenuStrip = this.contextMenuStrip1;
            this.textBoxDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDate.Location = new System.Drawing.Point(2, 2);
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.ReadOnly = true;
            this.textBoxDate.Size = new System.Drawing.Size(60, 20);
            this.textBoxDate.TabIndex = 0;
            this.textBoxDate.TabStop = false;
            this.textBoxDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frameDoorOpeningsToolStripMenuItem,
            this.dayDetailToolStripMenuItem,
            this.dayDetailAndAllPriorToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(195, 92);
            // 
            // frameDoorOpeningsToolStripMenuItem
            // 
            this.frameDoorOpeningsToolStripMenuItem.Name = "frameDoorOpeningsToolStripMenuItem";
            this.frameDoorOpeningsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.frameDoorOpeningsToolStripMenuItem.Text = "Frame Door Openings";
            this.frameDoorOpeningsToolStripMenuItem.Click += new System.EventHandler(this.frameDoorOpeningsToolStripMenuItem_Click);
            // 
            // dayDetailToolStripMenuItem
            // 
            this.dayDetailToolStripMenuItem.Name = "dayDetailToolStripMenuItem";
            this.dayDetailToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.dayDetailToolStripMenuItem.Text = "Day Detail";
            this.dayDetailToolStripMenuItem.Click += new System.EventHandler(this.dayDetailToolStripMenuItem_Click);
            // 
            // textBoxDoors
            // 
            this.textBoxDoors.Location = new System.Drawing.Point(10, 26);
            this.textBoxDoors.Name = "textBoxDoors";
            this.textBoxDoors.ReadOnly = true;
            this.textBoxDoors.Size = new System.Drawing.Size(45, 20);
            this.textBoxDoors.TabIndex = 1;
            this.textBoxDoors.TabStop = false;
            this.textBoxDoors.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxFrames
            // 
            this.textBoxFrames.Location = new System.Drawing.Point(10, 50);
            this.textBoxFrames.Name = "textBoxFrames";
            this.textBoxFrames.ReadOnly = true;
            this.textBoxFrames.Size = new System.Drawing.Size(45, 20);
            this.textBoxFrames.TabIndex = 2;
            this.textBoxFrames.TabStop = false;
            this.textBoxFrames.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dayDetailAndAllPriorToolStripMenuItem
            // 
            this.dayDetailAndAllPriorToolStripMenuItem.Name = "dayDetailAndAllPriorToolStripMenuItem";
            this.dayDetailAndAllPriorToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.dayDetailAndAllPriorToolStripMenuItem.Text = "Day Detail and All Prior";
            this.dayDetailAndAllPriorToolStripMenuItem.Click += new System.EventHandler(this.dayDetailAndAllPriorToolStripMenuItem_Click);
            // 
            // CapacityOneDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxDate);
            this.Controls.Add(this.textBoxFrames);
            this.Controls.Add(this.textBoxDoors);
            this.Name = "CapacityOneDay";
            this.Size = new System.Drawing.Size(65, 70);
            this.Load += new System.EventHandler(this.CapacityOneDay_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CapacityOneDay_Paint);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.TextBox textBoxDoors;
        private System.Windows.Forms.TextBox textBoxFrames;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem frameDoorOpeningsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dayDetailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dayDetailAndAllPriorToolStripMenuItem;

    }
}
