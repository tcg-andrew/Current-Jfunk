﻿namespace CIGSliderLabels
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtPieces = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPart = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(148, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Print";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPieces
            // 
            this.txtPieces.Location = new System.Drawing.Point(60, 66);
            this.txtPieces.Multiline = true;
            this.txtPieces.Name = "txtPieces";
            this.txtPieces.Size = new System.Drawing.Size(286, 20);
            this.txtPieces.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Pieces";
            // 
            // txtPart
            // 
            this.txtPart.Location = new System.Drawing.Point(60, 21);
            this.txtPart.Name = "txtPart";
            this.txtPart.Size = new System.Drawing.Size(286, 20);
            this.txtPart.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Part #";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Job # / Customer Name";
            // 
            // txtDetails
            // 
            this.txtDetails.Location = new System.Drawing.Point(21, 122);
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.Size = new System.Drawing.Size(325, 20);
            this.txtDetails.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 220);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtPieces);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPart);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "CIG Slider Labels";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPieces;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDetails;
    }
}

