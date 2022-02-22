namespace PackagingMESEdit
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
            this.lbAvailableMES = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butAdd = new System.Windows.Forms.Button();
            this.lbSelectedMES = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.butDone = new System.Windows.Forms.Button();
            this.butRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbAvailableMES
            // 
            this.lbAvailableMES.FormattingEnabled = true;
            this.lbAvailableMES.Location = new System.Drawing.Point(12, 35);
            this.lbAvailableMES.Name = "lbAvailableMES";
            this.lbAvailableMES.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAvailableMES.Size = new System.Drawing.Size(588, 121);
            this.lbAvailableMES.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available Packaging MES\'s";
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(197, 163);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(219, 23);
            this.butAdd.TabIndex = 2;
            this.butAdd.Text = "Add Selected MES\'s to all lines";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // lbSelectedMES
            // 
            this.lbSelectedMES.FormattingEnabled = true;
            this.lbSelectedMES.Location = new System.Drawing.Point(12, 213);
            this.lbSelectedMES.Name = "lbSelectedMES";
            this.lbSelectedMES.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSelectedMES.Size = new System.Drawing.Size(588, 121);
            this.lbSelectedMES.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Selected Packaging MES\'s";
            // 
            // butDone
            // 
            this.butDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butDone.Location = new System.Drawing.Point(269, 380);
            this.butDone.Name = "butDone";
            this.butDone.Size = new System.Drawing.Size(75, 23);
            this.butDone.TabIndex = 5;
            this.butDone.Text = "Done";
            this.butDone.UseVisualStyleBackColor = true;
            this.butDone.Click += new System.EventHandler(this.butDone_Click);
            // 
            // butRemove
            // 
            this.butRemove.Location = new System.Drawing.Point(197, 341);
            this.butRemove.Name = "butRemove";
            this.butRemove.Size = new System.Drawing.Size(219, 23);
            this.butRemove.TabIndex = 6;
            this.butRemove.Text = "Remove Selected MES\'s from all lines";
            this.butRemove.UseVisualStyleBackColor = true;
            this.butRemove.Click += new System.EventHandler(this.butRemove_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butDone;
            this.ClientSize = new System.Drawing.Size(612, 426);
            this.Controls.Add(this.butRemove);
            this.Controls.Add(this.butDone);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbSelectedMES);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbAvailableMES);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packaging MES Edit";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAvailableMES;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.ListBox lbSelectedMES;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button butDone;
        private System.Windows.Forms.Button butRemove;
    }
}

