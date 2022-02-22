namespace TCAttributizer
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
            this.flp_Inputs = new System.Windows.Forms.FlowLayoutPanel();
            this.grp_NewInput = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_NewInput = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmb_Attr = new System.Windows.Forms.ComboBox();
            this.txt_Attr = new System.Windows.Forms.TextBox();
            this.btn_AddAttr = new System.Windows.Forms.Button();
            this.grp_Values = new System.Windows.Forms.GroupBox();
            this.cmb_Value = new System.Windows.Forms.ComboBox();
            this.txt_Value = new System.Windows.Forms.TextBox();
            this.btn_AddValue = new System.Windows.Forms.Button();
            this.grp_Cond = new System.Windows.Forms.GroupBox();
            this.flp_Cond = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.flp_Inputs.SuspendLayout();
            this.grp_NewInput.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grp_Values.SuspendLayout();
            this.grp_Cond.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.flp_Inputs);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 691);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inputs";
            // 
            // flp_Inputs
            // 
            this.flp_Inputs.AutoScroll = true;
            this.flp_Inputs.AutoSize = true;
            this.flp_Inputs.Controls.Add(this.grp_NewInput);
            this.flp_Inputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp_Inputs.Location = new System.Drawing.Point(3, 16);
            this.flp_Inputs.Name = "flp_Inputs";
            this.flp_Inputs.Size = new System.Drawing.Size(269, 672);
            this.flp_Inputs.TabIndex = 0;
            // 
            // grp_NewInput
            // 
            this.grp_NewInput.Controls.Add(this.button1);
            this.grp_NewInput.Controls.Add(this.txt_NewInput);
            this.grp_NewInput.Controls.Add(this.label1);
            this.grp_NewInput.Location = new System.Drawing.Point(3, 3);
            this.grp_NewInput.Name = "grp_NewInput";
            this.grp_NewInput.Size = new System.Drawing.Size(246, 69);
            this.grp_NewInput.TabIndex = 0;
            this.grp_NewInput.TabStop = false;
            this.grp_NewInput.Text = "New Input";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // txt_NewInput
            // 
            this.txt_NewInput.Location = new System.Drawing.Point(47, 13);
            this.txt_NewInput.Name = "txt_NewInput";
            this.txt_NewInput.Size = new System.Drawing.Size(193, 20);
            this.txt_NewInput.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(234, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.grp_Values);
            this.groupBox3.Controls.Add(this.btn_AddAttr);
            this.groupBox3.Controls.Add(this.txt_Attr);
            this.groupBox3.Controls.Add(this.cmb_Attr);
            this.groupBox3.Location = new System.Drawing.Point(293, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(252, 166);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Attributes";
            // 
            // cmb_Attr
            // 
            this.cmb_Attr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Attr.FormattingEnabled = true;
            this.cmb_Attr.Location = new System.Drawing.Point(6, 19);
            this.cmb_Attr.Name = "cmb_Attr";
            this.cmb_Attr.Size = new System.Drawing.Size(240, 21);
            this.cmb_Attr.TabIndex = 0;
            this.cmb_Attr.SelectedIndexChanged += new System.EventHandler(this.cmb_Attr_SelectedIndexChanged);
            // 
            // txt_Attr
            // 
            this.txt_Attr.Location = new System.Drawing.Point(6, 46);
            this.txt_Attr.Name = "txt_Attr";
            this.txt_Attr.Size = new System.Drawing.Size(209, 20);
            this.txt_Attr.TabIndex = 1;
            // 
            // btn_AddAttr
            // 
            this.btn_AddAttr.Location = new System.Drawing.Point(221, 44);
            this.btn_AddAttr.Name = "btn_AddAttr";
            this.btn_AddAttr.Size = new System.Drawing.Size(25, 23);
            this.btn_AddAttr.TabIndex = 2;
            this.btn_AddAttr.Text = "+";
            this.btn_AddAttr.UseVisualStyleBackColor = true;
            this.btn_AddAttr.Click += new System.EventHandler(this.btn_AddAttr_Click);
            // 
            // grp_Values
            // 
            this.grp_Values.Controls.Add(this.btn_AddValue);
            this.grp_Values.Controls.Add(this.txt_Value);
            this.grp_Values.Controls.Add(this.cmb_Value);
            this.grp_Values.Location = new System.Drawing.Point(6, 73);
            this.grp_Values.Name = "grp_Values";
            this.grp_Values.Size = new System.Drawing.Size(240, 77);
            this.grp_Values.TabIndex = 3;
            this.grp_Values.TabStop = false;
            this.grp_Values.Text = "Values";
            this.grp_Values.Visible = false;
            // 
            // cmb_Value
            // 
            this.cmb_Value.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Value.FormattingEnabled = true;
            this.cmb_Value.Location = new System.Drawing.Point(6, 19);
            this.cmb_Value.Name = "cmb_Value";
            this.cmb_Value.Size = new System.Drawing.Size(228, 21);
            this.cmb_Value.TabIndex = 0;
            // 
            // txt_Value
            // 
            this.txt_Value.Location = new System.Drawing.Point(6, 46);
            this.txt_Value.Name = "txt_Value";
            this.txt_Value.Size = new System.Drawing.Size(197, 20);
            this.txt_Value.TabIndex = 1;
            // 
            // btn_AddValue
            // 
            this.btn_AddValue.Location = new System.Drawing.Point(209, 44);
            this.btn_AddValue.Name = "btn_AddValue";
            this.btn_AddValue.Size = new System.Drawing.Size(25, 23);
            this.btn_AddValue.TabIndex = 4;
            this.btn_AddValue.Text = "+";
            this.btn_AddValue.UseVisualStyleBackColor = true;
            this.btn_AddValue.Click += new System.EventHandler(this.btn_AddValue_Click);
            // 
            // grp_Cond
            // 
            this.grp_Cond.Controls.Add(this.flp_Cond);
            this.grp_Cond.Location = new System.Drawing.Point(293, 184);
            this.grp_Cond.Name = "grp_Cond";
            this.grp_Cond.Size = new System.Drawing.Size(255, 519);
            this.grp_Cond.TabIndex = 2;
            this.grp_Cond.TabStop = false;
            this.grp_Cond.Text = "Conditions";
            // 
            // flp_Cond
            // 
            this.flp_Cond.AutoScroll = true;
            this.flp_Cond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp_Cond.Location = new System.Drawing.Point(3, 16);
            this.flp_Cond.Name = "flp_Cond";
            this.flp_Cond.Size = new System.Drawing.Size(249, 500);
            this.flp_Cond.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 715);
            this.Controls.Add(this.grp_Cond);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flp_Inputs.ResumeLayout(false);
            this.grp_NewInput.ResumeLayout(false);
            this.grp_NewInput.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grp_Values.ResumeLayout(false);
            this.grp_Values.PerformLayout();
            this.grp_Cond.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flp_Inputs;
        private System.Windows.Forms.GroupBox grp_NewInput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_NewInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_AddAttr;
        private System.Windows.Forms.TextBox txt_Attr;
        private System.Windows.Forms.ComboBox cmb_Attr;
        private System.Windows.Forms.GroupBox grp_Values;
        private System.Windows.Forms.Button btn_AddValue;
        private System.Windows.Forms.TextBox txt_Value;
        private System.Windows.Forms.ComboBox cmb_Value;
        private System.Windows.Forms.GroupBox grp_Cond;
        private System.Windows.Forms.FlowLayoutPanel flp_Cond;
    }
}

