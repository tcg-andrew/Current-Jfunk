namespace PartDrawingViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblNoDrawings = new System.Windows.Forms.Label();
            this.tsNavigation = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.axEModelViewControl1 = new AxEModelView.AxEModelViewControl();
            this.tsNavigation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axEModelViewControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNoDrawings
            // 
            this.lblNoDrawings.AutoSize = true;
            this.lblNoDrawings.Location = new System.Drawing.Point(60, 157);
            this.lblNoDrawings.Name = "lblNoDrawings";
            this.lblNoDrawings.Size = new System.Drawing.Size(306, 13);
            this.lblNoDrawings.TabIndex = 1;
            this.lblNoDrawings.Text = "No drawings found for part #xxxxx.  Please contact Engineering";
            this.lblNoDrawings.Visible = false;
            // 
            // tsNavigation
            // 
            this.tsNavigation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.tsNavigation.Location = new System.Drawing.Point(0, 0);
            this.tsNavigation.Name = "tsNavigation";
            this.tsNavigation.Size = new System.Drawing.Size(403, 25);
            this.tsNavigation.TabIndex = 2;
            this.tsNavigation.Text = "toolStrip1";
            this.tsNavigation.Visible = false;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Prev";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Next";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // axEModelViewControl1
            // 
            this.axEModelViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axEModelViewControl1.Enabled = true;
            this.axEModelViewControl1.Location = new System.Drawing.Point(0, 0);
            this.axEModelViewControl1.Name = "axEModelViewControl1";
            this.axEModelViewControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEModelViewControl1.OcxState")));
            this.axEModelViewControl1.Size = new System.Drawing.Size(403, 334);
            this.axEModelViewControl1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 334);
            this.Controls.Add(this.axEModelViewControl1);
            this.Controls.Add(this.tsNavigation);
            this.Controls.Add(this.lblNoDrawings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.tsNavigation.ResumeLayout(false);
            this.tsNavigation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axEModelViewControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNoDrawings;
        private System.Windows.Forms.ToolStrip tsNavigation;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private AxEModelView.AxEModelViewControl axEModelViewControl1;
    }
}

