namespace TCGEpicor
{
    partial class SolidWorksViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolidWorksViewer));
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.axEModelViewControl1 = new AxEModelView.AxEModelViewControl();
            ((System.ComponentModel.ISupportInitialize)(this.axEModelViewControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // axEModelViewControl1
            // 
            this.axEModelViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axEModelViewControl1.Enabled = true;
            this.axEModelViewControl1.Location = new System.Drawing.Point(0, 0);
            this.axEModelViewControl1.Name = "axEModelViewControl1";
            this.axEModelViewControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEModelViewControl1.OcxState")));
            this.axEModelViewControl1.Size = new System.Drawing.Size(292, 273);
            this.axEModelViewControl1.TabIndex = 0;
            // 
            // SolidWorksViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.axEModelViewControl1);
            this.Name = "SolidWorksViewer";
            this.Text = "SolidWorksViewer";
            this.Shown += new System.EventHandler(this.SolidWorksViewer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.axEModelViewControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PrintDialog printDialog1;
        private AxEModelView.AxEModelViewControl axEModelViewControl1;
    }
}