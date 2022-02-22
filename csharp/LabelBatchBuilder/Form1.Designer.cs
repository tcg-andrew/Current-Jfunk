namespace LabelBatchBuilder
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
            this.tb_PONum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_PartNumStart = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_PartNumEnd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Description = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_PartClass = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.b_TakeSelected = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.b_RemoveSelected = new System.Windows.Forms.Button();
            this.b_TakeAll = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PO #";
            // 
            // tb_PONum
            // 
            this.tb_PONum.Location = new System.Drawing.Point(47, 10);
            this.tb_PONum.Name = "tb_PONum";
            this.tb_PONum.Size = new System.Drawing.Size(91, 20);
            this.tb_PONum.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Part #";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Begins With";
            // 
            // tb_PartNumStart
            // 
            this.tb_PartNumStart.Location = new System.Drawing.Point(121, 43);
            this.tb_PartNumStart.Name = "tb_PartNumStart";
            this.tb_PartNumStart.Size = new System.Drawing.Size(100, 20);
            this.tb_PartNumStart.TabIndex = 4;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(227, 44);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButton1.Size = new System.Drawing.Size(44, 17);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "And";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(277, 44);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButton2.Size = new System.Drawing.Size(36, 17);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Or";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(319, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Ends With";
            // 
            // tb_PartNumEnd
            // 
            this.tb_PartNumEnd.Location = new System.Drawing.Point(381, 43);
            this.tb_PartNumEnd.Name = "tb_PartNumEnd";
            this.tb_PartNumEnd.Size = new System.Drawing.Size(100, 20);
            this.tb_PartNumEnd.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Description contains";
            // 
            // tb_Description
            // 
            this.tb_Description.Location = new System.Drawing.Point(121, 72);
            this.tb_Description.Name = "tb_Description";
            this.tb_Description.Size = new System.Drawing.Size(363, 20);
            this.tb_Description.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Part Class";
            // 
            // cb_PartClass
            // 
            this.cb_PartClass.FormattingEnabled = true;
            this.cb_PartClass.Items.AddRange(new object[] {
            "All"});
            this.cb_PartClass.Location = new System.Drawing.Point(70, 99);
            this.cb_PartClass.Name = "cb_PartClass";
            this.cb_PartClass.Size = new System.Drawing.Size(201, 21);
            this.cb_PartClass.TabIndex = 12;
            this.cb_PartClass.Text = "All";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(217, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Location = new System.Drawing.Point(18, 200);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(179, 419);
            this.dataGridView1.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 181);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Search Results";
            // 
            // b_TakeSelected
            // 
            this.b_TakeSelected.Location = new System.Drawing.Point(237, 332);
            this.b_TakeSelected.Name = "b_TakeSelected";
            this.b_TakeSelected.Size = new System.Drawing.Size(34, 23);
            this.b_TakeSelected.TabIndex = 16;
            this.b_TakeSelected.Text = ">";
            this.b_TakeSelected.UseVisualStyleBackColor = true;
            this.b_TakeSelected.Click += new System.EventHandler(this.b_TakeSelected_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.ColumnHeadersVisible = false;
            this.dataGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView2.Location = new System.Drawing.Point(311, 200);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(179, 419);
            this.dataGridView2.TabIndex = 17;
            // 
            // b_RemoveSelected
            // 
            this.b_RemoveSelected.Location = new System.Drawing.Point(237, 377);
            this.b_RemoveSelected.Name = "b_RemoveSelected";
            this.b_RemoveSelected.Size = new System.Drawing.Size(34, 23);
            this.b_RemoveSelected.TabIndex = 18;
            this.b_RemoveSelected.Text = "<";
            this.b_RemoveSelected.UseVisualStyleBackColor = true;
            this.b_RemoveSelected.Click += new System.EventHandler(this.b_RemoveSelected_Click);
            // 
            // b_TakeAll
            // 
            this.b_TakeAll.Location = new System.Drawing.Point(237, 292);
            this.b_TakeAll.Name = "b_TakeAll";
            this.b_TakeAll.Size = new System.Drawing.Size(34, 23);
            this.b_TakeAll.TabIndex = 19;
            this.b_TakeAll.Text = "> >";
            this.b_TakeAll.UseVisualStyleBackColor = true;
            this.b_TakeAll.Click += new System.EventHandler(this.b_TakeAll_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(237, 417);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(34, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "< <";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(114, 651);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(281, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "Print Labels For Selected Parts";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 686);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.b_TakeAll);
            this.Controls.Add(this.b_RemoveSelected);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.b_TakeSelected);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cb_PartClass);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_Description);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_PartNumEnd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.tb_PartNumStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_PONum);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Label Batch Builder";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_PONum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_PartNumStart;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_PartNumEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Description;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_PartClass;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button b_TakeSelected;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button b_RemoveSelected;
        private System.Windows.Forms.Button b_TakeAll;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

