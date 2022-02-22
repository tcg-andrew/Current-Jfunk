namespace OperationBatch
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
            this.tb_JobNum = new System.Windows.Forms.TextBox();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Process = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_EmployeeID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dd_Operation = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_AsmSeq = new System.Windows.Forms.TextBox();
            this.dgv_JobBatch = new System.Windows.Forms.DataGridView();
            this.clm_JobNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clm_AsmSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clm_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_JobBatch)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Job #";
            // 
            // tb_JobNum
            // 
            this.tb_JobNum.Location = new System.Drawing.Point(54, 65);
            this.tb_JobNum.Name = "tb_JobNum";
            this.tb_JobNum.Size = new System.Drawing.Size(96, 20);
            this.tb_JobNum.TabIndex = 1;
            this.tb_JobNum.TextChanged += new System.EventHandler(this.tb_JobNum_TextChanged);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(275, 63);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(81, 23);
            this.btn_Add.TabIndex = 2;
            this.btn_Add.Text = "Add";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Process
            // 
            this.btn_Process.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Process.Location = new System.Drawing.Point(146, 509);
            this.btn_Process.Name = "btn_Process";
            this.btn_Process.Size = new System.Drawing.Size(75, 23);
            this.btn_Process.TabIndex = 4;
            this.btn_Process.Text = "Process";
            this.btn_Process.UseVisualStyleBackColor = true;
            this.btn_Process.Click += new System.EventHandler(this.btn_Process_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Employee";
            // 
            // tb_EmployeeID
            // 
            this.tb_EmployeeID.Location = new System.Drawing.Point(72, 10);
            this.tb_EmployeeID.Name = "tb_EmployeeID";
            this.tb_EmployeeID.Size = new System.Drawing.Size(63, 20);
            this.tb_EmployeeID.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(188, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Operation";
            // 
            // dd_Operation
            // 
            this.dd_Operation.FormattingEnabled = true;
            this.dd_Operation.Items.AddRange(new object[] {
            "Door Punch",
            "Frame Punch",
            "Packing"});
            this.dd_Operation.Location = new System.Drawing.Point(247, 9);
            this.dd_Operation.Name = "dd_Operation";
            this.dd_Operation.Size = new System.Drawing.Size(108, 21);
            this.dd_Operation.TabIndex = 8;
            this.dd_Operation.SelectedIndexChanged += new System.EventHandler(this.dd_Operation_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "AsmSeq";
            // 
            // tb_AsmSeq
            // 
            this.tb_AsmSeq.Location = new System.Drawing.Point(208, 65);
            this.tb_AsmSeq.Name = "tb_AsmSeq";
            this.tb_AsmSeq.Size = new System.Drawing.Size(43, 20);
            this.tb_AsmSeq.TabIndex = 10;
            // 
            // dgv_JobBatch
            // 
            this.dgv_JobBatch.AllowUserToAddRows = false;
            this.dgv_JobBatch.AllowUserToResizeColumns = false;
            this.dgv_JobBatch.AllowUserToResizeRows = false;
            this.dgv_JobBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_JobBatch.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgv_JobBatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_JobBatch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clm_JobNum,
            this.clm_AsmSeq,
            this.clm_Qty});
            this.dgv_JobBatch.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv_JobBatch.Location = new System.Drawing.Point(12, 92);
            this.dgv_JobBatch.Name = "dgv_JobBatch";
            this.dgv_JobBatch.RowHeadersVisible = false;
            this.dgv_JobBatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_JobBatch.Size = new System.Drawing.Size(344, 411);
            this.dgv_JobBatch.TabIndex = 11;
            // 
            // clm_JobNum
            // 
            this.clm_JobNum.HeaderText = "Job #";
            this.clm_JobNum.Name = "clm_JobNum";
            this.clm_JobNum.ReadOnly = true;
            this.clm_JobNum.Width = 200;
            // 
            // clm_AsmSeq
            // 
            this.clm_AsmSeq.HeaderText = "AsmSeq";
            this.clm_AsmSeq.Name = "clm_AsmSeq";
            this.clm_AsmSeq.ReadOnly = true;
            this.clm_AsmSeq.Width = 60;
            // 
            // clm_Qty
            // 
            this.clm_Qty.HeaderText = "Qty";
            this.clm_Qty.Name = "clm_Qty";
            this.clm_Qty.Width = 50;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(367, 544);
            this.Controls.Add(this.dgv_JobBatch);
            this.Controls.Add(this.tb_AsmSeq);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dd_Operation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_EmployeeID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Process);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.tb_JobNum);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(375, 750);
            this.Name = "Form1";
            this.Text = "Batch Operation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_JobBatch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_JobNum;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Process;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_EmployeeID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox dd_Operation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_AsmSeq;
        private System.Windows.Forms.DataGridView dgv_JobBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_JobNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_AsmSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_Qty;
    }
}

