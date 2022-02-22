namespace HardMOMRevision
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lb_MOMs = new System.Windows.Forms.ListView();
            this.Part = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Revision = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_ClearMom = new System.Windows.Forms.Button();
            this.btn_RemoveMOM = new System.Windows.Forms.Button();
            this.btn_AddMOM = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gb_MergeOpr = new System.Windows.Forms.GroupBox();
            this.ddl_MergeToOpr = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ddl_MergeFromOpr = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_Status = new System.Windows.Forms.Label();
            this.gb_Duplicate = new System.Windows.Forms.GroupBox();
            this.but_Duplicate = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_DuplicateName = new System.Windows.Forms.TextBox();
            this.gb_Routine = new System.Windows.Forms.GroupBox();
            this.but_Routine = new System.Windows.Forms.Button();
            this.cb_Routines = new System.Windows.Forms.ComboBox();
            this.gb_RemoveOpr = new System.Windows.Forms.GroupBox();
            this.ddl_Remove_Operations = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_RemoveOpr = new System.Windows.Forms.Button();
            this.gb_Revise = new System.Windows.Forms.GroupBox();
            this.btn_Revise = new System.Windows.Forms.Button();
            this.txt_Revise_Qty = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ddl_Revise_Part = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gb_Replace = new System.Windows.Forms.GroupBox();
            this.ddl_Replace_Part = new System.Windows.Forms.ComboBox();
            this.btn_Replace = new System.Windows.Forms.Button();
            this.txt_Replace_Part = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.gb_Remove = new System.Windows.Forms.GroupBox();
            this.btn_Remove = new System.Windows.Forms.Button();
            this.ddl_Remove_Part = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gb_Add = new System.Windows.Forms.GroupBox();
            this.btn_AddMaterial = new System.Windows.Forms.Button();
            this.txt_Add_Qty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ddl_Add_Operations = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Add_Partnum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_AddOpr = new System.Windows.Forms.GroupBox();
            this.ddl_AddOperation = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_AddOpr = new System.Windows.Forms.Button();
            this.ddl_ReviseOpr = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gb_MergeOpr.SuspendLayout();
            this.gb_Duplicate.SuspendLayout();
            this.gb_Routine.SuspendLayout();
            this.gb_RemoveOpr.SuspendLayout();
            this.gb_Revise.SuspendLayout();
            this.gb_Replace.SuspendLayout();
            this.gb_Remove.SuspendLayout();
            this.gb_Add.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.gb_AddOpr.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lb_MOMs);
            this.splitContainer1.Panel1.Controls.Add(this.btn_ClearMom);
            this.splitContainer1.Panel1.Controls.Add(this.btn_RemoveMOM);
            this.splitContainer1.Panel1.Controls.Add(this.btn_AddMOM);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gb_AddOpr);
            this.splitContainer1.Panel2.Controls.Add(this.gb_MergeOpr);
            this.splitContainer1.Panel2.Controls.Add(this.lbl_Status);
            this.splitContainer1.Panel2.Controls.Add(this.gb_Duplicate);
            this.splitContainer1.Panel2.Controls.Add(this.gb_Routine);
            this.splitContainer1.Panel2.Controls.Add(this.gb_RemoveOpr);
            this.splitContainer1.Panel2.Controls.Add(this.gb_Revise);
            this.splitContainer1.Panel2.Controls.Add(this.gb_Replace);
            this.splitContainer1.Panel2.Controls.Add(this.gb_Remove);
            this.splitContainer1.Panel2.Controls.Add(this.gb_Add);
            this.splitContainer1.Size = new System.Drawing.Size(738, 728);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 0;
            // 
            // lb_MOMs
            // 
            this.lb_MOMs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Part,
            this.Revision});
            this.lb_MOMs.FullRowSelect = true;
            this.lb_MOMs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lb_MOMs.Location = new System.Drawing.Point(3, 49);
            this.lb_MOMs.Name = "lb_MOMs";
            this.lb_MOMs.Size = new System.Drawing.Size(210, 676);
            this.lb_MOMs.TabIndex = 5;
            this.lb_MOMs.UseCompatibleStateImageBehavior = false;
            this.lb_MOMs.View = System.Windows.Forms.View.Details;
            // 
            // Part
            // 
            this.Part.Width = 150;
            // 
            // Revision
            // 
            this.Revision.Width = 50;
            // 
            // btn_ClearMom
            // 
            this.btn_ClearMom.Location = new System.Drawing.Point(71, 25);
            this.btn_ClearMom.Name = "btn_ClearMom";
            this.btn_ClearMom.Size = new System.Drawing.Size(75, 23);
            this.btn_ClearMom.TabIndex = 4;
            this.btn_ClearMom.Text = "Clear List";
            this.btn_ClearMom.UseVisualStyleBackColor = true;
            this.btn_ClearMom.Click += new System.EventHandler(this.btn_ClearMom_Click);
            // 
            // btn_RemoveMOM
            // 
            this.btn_RemoveMOM.Location = new System.Drawing.Point(190, 25);
            this.btn_RemoveMOM.Name = "btn_RemoveMOM";
            this.btn_RemoveMOM.Size = new System.Drawing.Size(23, 23);
            this.btn_RemoveMOM.TabIndex = 3;
            this.btn_RemoveMOM.Text = "-";
            this.btn_RemoveMOM.UseVisualStyleBackColor = true;
            this.btn_RemoveMOM.Click += new System.EventHandler(this.btn_RemoveMOM_Click);
            // 
            // btn_AddMOM
            // 
            this.btn_AddMOM.Location = new System.Drawing.Point(3, 25);
            this.btn_AddMOM.Name = "btn_AddMOM";
            this.btn_AddMOM.Size = new System.Drawing.Size(23, 23);
            this.btn_AddMOM.TabIndex = 2;
            this.btn_AddMOM.Text = "+";
            this.btn_AddMOM.UseVisualStyleBackColor = true;
            this.btn_AddMOM.Click += new System.EventHandler(this.btn_AddMOM_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "MOMs To Revise";
            // 
            // gb_MergeOpr
            // 
            this.gb_MergeOpr.Controls.Add(this.ddl_MergeToOpr);
            this.gb_MergeOpr.Controls.Add(this.label13);
            this.gb_MergeOpr.Controls.Add(this.ddl_MergeFromOpr);
            this.gb_MergeOpr.Controls.Add(this.label12);
            this.gb_MergeOpr.Controls.Add(this.button1);
            this.gb_MergeOpr.Enabled = false;
            this.gb_MergeOpr.Location = new System.Drawing.Point(3, 409);
            this.gb_MergeOpr.Name = "gb_MergeOpr";
            this.gb_MergeOpr.Size = new System.Drawing.Size(503, 64);
            this.gb_MergeOpr.TabIndex = 6;
            this.gb_MergeOpr.TabStop = false;
            this.gb_MergeOpr.Text = "Merge Operation";
            // 
            // ddl_MergeToOpr
            // 
            this.ddl_MergeToOpr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_MergeToOpr.FormattingEnabled = true;
            this.ddl_MergeToOpr.Location = new System.Drawing.Point(277, 21);
            this.ddl_MergeToOpr.Name = "ddl_MergeToOpr";
            this.ddl_MergeToOpr.Size = new System.Drawing.Size(121, 21);
            this.ddl_MergeToOpr.TabIndex = 7;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(218, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Opr To";
            // 
            // ddl_MergeFromOpr
            // 
            this.ddl_MergeFromOpr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_MergeFromOpr.FormattingEnabled = true;
            this.ddl_MergeFromOpr.Location = new System.Drawing.Point(66, 21);
            this.ddl_MergeFromOpr.Name = "ddl_MergeFromOpr";
            this.ddl_MergeFromOpr.Size = new System.Drawing.Size(121, 21);
            this.ddl_MergeFromOpr.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Opr From";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(422, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Merge";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_Status
            // 
            this.lbl_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_Status.AutoSize = true;
            this.lbl_Status.Location = new System.Drawing.Point(9, 703);
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(41, 13);
            this.lbl_Status.TabIndex = 6;
            this.lbl_Status.Text = "label12";
            // 
            // gb_Duplicate
            // 
            this.gb_Duplicate.Controls.Add(this.but_Duplicate);
            this.gb_Duplicate.Controls.Add(this.label11);
            this.gb_Duplicate.Controls.Add(this.txt_DuplicateName);
            this.gb_Duplicate.Enabled = false;
            this.gb_Duplicate.Location = new System.Drawing.Point(3, 549);
            this.gb_Duplicate.Name = "gb_Duplicate";
            this.gb_Duplicate.Size = new System.Drawing.Size(503, 46);
            this.gb_Duplicate.TabIndex = 5;
            this.gb_Duplicate.TabStop = false;
            this.gb_Duplicate.Text = "Duplicate";
            // 
            // but_Duplicate
            // 
            this.but_Duplicate.Location = new System.Drawing.Point(422, 15);
            this.but_Duplicate.Name = "but_Duplicate";
            this.but_Duplicate.Size = new System.Drawing.Size(75, 23);
            this.but_Duplicate.TabIndex = 2;
            this.but_Duplicate.Text = "Duplicate";
            this.but_Duplicate.UseVisualStyleBackColor = true;
            this.but_Duplicate.Click += new System.EventHandler(this.but_Duplicate_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "New Name";
            // 
            // txt_DuplicateName
            // 
            this.txt_DuplicateName.Location = new System.Drawing.Point(73, 17);
            this.txt_DuplicateName.Name = "txt_DuplicateName";
            this.txt_DuplicateName.Size = new System.Drawing.Size(207, 20);
            this.txt_DuplicateName.TabIndex = 0;
            // 
            // gb_Routine
            // 
            this.gb_Routine.Controls.Add(this.but_Routine);
            this.gb_Routine.Controls.Add(this.cb_Routines);
            this.gb_Routine.Enabled = false;
            this.gb_Routine.Location = new System.Drawing.Point(3, 597);
            this.gb_Routine.Name = "gb_Routine";
            this.gb_Routine.Size = new System.Drawing.Size(503, 59);
            this.gb_Routine.TabIndex = 4;
            this.gb_Routine.TabStop = false;
            this.gb_Routine.Text = "Custom Routines";
            this.gb_Routine.Visible = false;
            // 
            // but_Routine
            // 
            this.but_Routine.Location = new System.Drawing.Point(422, 17);
            this.but_Routine.Name = "but_Routine";
            this.but_Routine.Size = new System.Drawing.Size(75, 23);
            this.but_Routine.TabIndex = 6;
            this.but_Routine.Text = "Run";
            this.but_Routine.UseVisualStyleBackColor = true;
            this.but_Routine.Click += new System.EventHandler(this.but_Routine_Click);
            // 
            // cb_Routines
            // 
            this.cb_Routines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Routines.FormattingEnabled = true;
            this.cb_Routines.Items.AddRange(new object[] {
            "Modline Frame Rename Add S Bracket Identifier",
            "Modline Door New Handles"});
            this.cb_Routines.Location = new System.Drawing.Point(6, 19);
            this.cb_Routines.Name = "cb_Routines";
            this.cb_Routines.Size = new System.Drawing.Size(356, 21);
            this.cb_Routines.TabIndex = 5;
            // 
            // gb_RemoveOpr
            // 
            this.gb_RemoveOpr.Controls.Add(this.ddl_Remove_Operations);
            this.gb_RemoveOpr.Controls.Add(this.label10);
            this.gb_RemoveOpr.Controls.Add(this.btn_RemoveOpr);
            this.gb_RemoveOpr.Enabled = false;
            this.gb_RemoveOpr.Location = new System.Drawing.Point(3, 480);
            this.gb_RemoveOpr.Name = "gb_RemoveOpr";
            this.gb_RemoveOpr.Size = new System.Drawing.Size(503, 64);
            this.gb_RemoveOpr.TabIndex = 3;
            this.gb_RemoveOpr.TabStop = false;
            this.gb_RemoveOpr.Text = "Remove Operation";
            // 
            // ddl_Remove_Operations
            // 
            this.ddl_Remove_Operations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Remove_Operations.FormattingEnabled = true;
            this.ddl_Remove_Operations.Location = new System.Drawing.Point(66, 21);
            this.ddl_Remove_Operations.Name = "ddl_Remove_Operations";
            this.ddl_Remove_Operations.Size = new System.Drawing.Size(121, 21);
            this.ddl_Remove_Operations.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Operation";
            // 
            // btn_RemoveOpr
            // 
            this.btn_RemoveOpr.Location = new System.Drawing.Point(422, 19);
            this.btn_RemoveOpr.Name = "btn_RemoveOpr";
            this.btn_RemoveOpr.Size = new System.Drawing.Size(75, 23);
            this.btn_RemoveOpr.TabIndex = 2;
            this.btn_RemoveOpr.Text = "Remove";
            this.btn_RemoveOpr.UseVisualStyleBackColor = true;
            this.btn_RemoveOpr.Click += new System.EventHandler(this.btn_RemoveOpr_Click);
            // 
            // gb_Revise
            // 
            this.gb_Revise.Controls.Add(this.label16);
            this.gb_Revise.Controls.Add(this.ddl_ReviseOpr);
            this.gb_Revise.Controls.Add(this.label15);
            this.gb_Revise.Controls.Add(this.btn_Revise);
            this.gb_Revise.Controls.Add(this.txt_Revise_Qty);
            this.gb_Revise.Controls.Add(this.label9);
            this.gb_Revise.Controls.Add(this.ddl_Revise_Part);
            this.gb_Revise.Controls.Add(this.label8);
            this.gb_Revise.Enabled = false;
            this.gb_Revise.Location = new System.Drawing.Point(3, 245);
            this.gb_Revise.Name = "gb_Revise";
            this.gb_Revise.Size = new System.Drawing.Size(503, 78);
            this.gb_Revise.TabIndex = 3;
            this.gb_Revise.TabStop = false;
            this.gb_Revise.Text = "Revise Material";
            // 
            // btn_Revise
            // 
            this.btn_Revise.Location = new System.Drawing.Point(422, 19);
            this.btn_Revise.Name = "btn_Revise";
            this.btn_Revise.Size = new System.Drawing.Size(75, 23);
            this.btn_Revise.TabIndex = 9;
            this.btn_Revise.Text = "Revise";
            this.btn_Revise.UseVisualStyleBackColor = true;
            this.btn_Revise.Click += new System.EventHandler(this.btn_Revise_Click);
            // 
            // txt_Revise_Qty
            // 
            this.txt_Revise_Qty.Location = new System.Drawing.Point(347, 22);
            this.txt_Revise_Qty.Name = "txt_Revise_Qty";
            this.txt_Revise_Qty.Size = new System.Drawing.Size(65, 20);
            this.txt_Revise_Qty.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(293, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "New Qty";
            // 
            // ddl_Revise_Part
            // 
            this.ddl_Revise_Part.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Revise_Part.FormattingEnabled = true;
            this.ddl_Revise_Part.Location = new System.Drawing.Point(48, 22);
            this.ddl_Revise_Part.Name = "ddl_Revise_Part";
            this.ddl_Revise_Part.Size = new System.Drawing.Size(230, 21);
            this.ddl_Revise_Part.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Part #";
            // 
            // gb_Replace
            // 
            this.gb_Replace.Controls.Add(this.ddl_Replace_Part);
            this.gb_Replace.Controls.Add(this.btn_Replace);
            this.gb_Replace.Controls.Add(this.txt_Replace_Part);
            this.gb_Replace.Controls.Add(this.label7);
            this.gb_Replace.Controls.Add(this.label6);
            this.gb_Replace.Enabled = false;
            this.gb_Replace.Location = new System.Drawing.Point(3, 160);
            this.gb_Replace.Name = "gb_Replace";
            this.gb_Replace.Size = new System.Drawing.Size(503, 79);
            this.gb_Replace.TabIndex = 2;
            this.gb_Replace.TabStop = false;
            this.gb_Replace.Text = "Replace Material";
            // 
            // ddl_Replace_Part
            // 
            this.ddl_Replace_Part.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Replace_Part.FormattingEnabled = true;
            this.ddl_Replace_Part.Location = new System.Drawing.Point(49, 22);
            this.ddl_Replace_Part.Name = "ddl_Replace_Part";
            this.ddl_Replace_Part.Size = new System.Drawing.Size(230, 21);
            this.ddl_Replace_Part.TabIndex = 8;
            // 
            // btn_Replace
            // 
            this.btn_Replace.Location = new System.Drawing.Point(422, 47);
            this.btn_Replace.Name = "btn_Replace";
            this.btn_Replace.Size = new System.Drawing.Size(75, 23);
            this.btn_Replace.TabIndex = 3;
            this.btn_Replace.Text = "Replace";
            this.btn_Replace.UseVisualStyleBackColor = true;
            this.btn_Replace.Click += new System.EventHandler(this.btn_Replace_Click);
            // 
            // txt_Replace_Part
            // 
            this.txt_Replace_Part.Location = new System.Drawing.Point(84, 50);
            this.txt_Replace_Part.Name = "txt_Replace_Part";
            this.txt_Replace_Part.Size = new System.Drawing.Size(196, 20);
            this.txt_Replace_Part.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Replace With";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Part #";
            // 
            // gb_Remove
            // 
            this.gb_Remove.Controls.Add(this.btn_Remove);
            this.gb_Remove.Controls.Add(this.ddl_Remove_Part);
            this.gb_Remove.Controls.Add(this.label5);
            this.gb_Remove.Enabled = false;
            this.gb_Remove.Location = new System.Drawing.Point(3, 90);
            this.gb_Remove.Name = "gb_Remove";
            this.gb_Remove.Size = new System.Drawing.Size(503, 64);
            this.gb_Remove.TabIndex = 1;
            this.gb_Remove.TabStop = false;
            this.gb_Remove.Text = "Remove Material";
            // 
            // btn_Remove
            // 
            this.btn_Remove.Location = new System.Drawing.Point(422, 19);
            this.btn_Remove.Name = "btn_Remove";
            this.btn_Remove.Size = new System.Drawing.Size(75, 23);
            this.btn_Remove.TabIndex = 2;
            this.btn_Remove.Text = "Remove";
            this.btn_Remove.UseVisualStyleBackColor = true;
            this.btn_Remove.Click += new System.EventHandler(this.btn_Remove_Click);
            // 
            // ddl_Remove_Part
            // 
            this.ddl_Remove_Part.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Remove_Part.FormattingEnabled = true;
            this.ddl_Remove_Part.Location = new System.Drawing.Point(48, 23);
            this.ddl_Remove_Part.Name = "ddl_Remove_Part";
            this.ddl_Remove_Part.Size = new System.Drawing.Size(231, 21);
            this.ddl_Remove_Part.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Part #";
            // 
            // gb_Add
            // 
            this.gb_Add.Controls.Add(this.btn_AddMaterial);
            this.gb_Add.Controls.Add(this.txt_Add_Qty);
            this.gb_Add.Controls.Add(this.label4);
            this.gb_Add.Controls.Add(this.ddl_Add_Operations);
            this.gb_Add.Controls.Add(this.label3);
            this.gb_Add.Controls.Add(this.txt_Add_Partnum);
            this.gb_Add.Controls.Add(this.label2);
            this.gb_Add.Enabled = false;
            this.gb_Add.Location = new System.Drawing.Point(3, 9);
            this.gb_Add.Name = "gb_Add";
            this.gb_Add.Size = new System.Drawing.Size(503, 75);
            this.gb_Add.TabIndex = 0;
            this.gb_Add.TabStop = false;
            this.gb_Add.Text = "Add Material";
            // 
            // btn_AddMaterial
            // 
            this.btn_AddMaterial.Location = new System.Drawing.Point(422, 40);
            this.btn_AddMaterial.Name = "btn_AddMaterial";
            this.btn_AddMaterial.Size = new System.Drawing.Size(75, 23);
            this.btn_AddMaterial.TabIndex = 6;
            this.btn_AddMaterial.Text = "Add";
            this.btn_AddMaterial.UseVisualStyleBackColor = true;
            this.btn_AddMaterial.Click += new System.EventHandler(this.btn_AddMaterial_Click);
            // 
            // txt_Add_Qty
            // 
            this.txt_Add_Qty.Location = new System.Drawing.Point(433, 13);
            this.txt_Add_Qty.Name = "txt_Add_Qty";
            this.txt_Add_Qty.Size = new System.Drawing.Size(64, 20);
            this.txt_Add_Qty.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(404, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Qty";
            // 
            // ddl_Add_Operations
            // 
            this.ddl_Add_Operations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Add_Operations.FormattingEnabled = true;
            this.ddl_Add_Operations.Location = new System.Drawing.Point(277, 13);
            this.ddl_Add_Operations.Name = "ddl_Add_Operations";
            this.ddl_Add_Operations.Size = new System.Drawing.Size(121, 21);
            this.ddl_Add_Operations.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(218, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Operation";
            // 
            // txt_Add_Partnum
            // 
            this.txt_Add_Partnum.Location = new System.Drawing.Point(48, 13);
            this.txt_Add_Partnum.Name = "txt_Add_Partnum";
            this.txt_Add_Partnum.Size = new System.Drawing.Size(164, 20);
            this.txt_Add_Partnum.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Part #";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pasteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(78, 26);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(77, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // gb_AddOpr
            // 
            this.gb_AddOpr.Controls.Add(this.ddl_AddOperation);
            this.gb_AddOpr.Controls.Add(this.label14);
            this.gb_AddOpr.Controls.Add(this.btn_AddOpr);
            this.gb_AddOpr.Enabled = false;
            this.gb_AddOpr.Location = new System.Drawing.Point(3, 340);
            this.gb_AddOpr.Name = "gb_AddOpr";
            this.gb_AddOpr.Size = new System.Drawing.Size(503, 64);
            this.gb_AddOpr.TabIndex = 6;
            this.gb_AddOpr.TabStop = false;
            this.gb_AddOpr.Text = "Add Operation";
            // 
            // ddl_AddOperation
            // 
            this.ddl_AddOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_AddOperation.FormattingEnabled = true;
            this.ddl_AddOperation.Location = new System.Drawing.Point(66, 21);
            this.ddl_AddOperation.Name = "ddl_AddOperation";
            this.ddl_AddOperation.Size = new System.Drawing.Size(121, 21);
            this.ddl_AddOperation.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Operation";
            // 
            // btn_AddOpr
            // 
            this.btn_AddOpr.Location = new System.Drawing.Point(422, 19);
            this.btn_AddOpr.Name = "btn_AddOpr";
            this.btn_AddOpr.Size = new System.Drawing.Size(75, 23);
            this.btn_AddOpr.TabIndex = 2;
            this.btn_AddOpr.Text = "Add";
            this.btn_AddOpr.UseVisualStyleBackColor = true;
            this.btn_AddOpr.Click += new System.EventHandler(this.btn_AddOpr_Click);
            // 
            // ddl_ReviseOpr
            // 
            this.ddl_ReviseOpr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_ReviseOpr.FormattingEnabled = true;
            this.ddl_ReviseOpr.Location = new System.Drawing.Point(66, 49);
            this.ddl_ReviseOpr.Name = "ddl_ReviseOpr";
            this.ddl_ReviseOpr.Size = new System.Drawing.Size(121, 21);
            this.ddl_ReviseOpr.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 52);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 13);
            this.label15.TabIndex = 12;
            this.label15.Text = "Operation";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(344, 52);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(148, 13);
            this.label16.TabIndex = 14;
            this.label16.Text = "Blank field will not be modified";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 728);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Batch Part Maintenance";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gb_MergeOpr.ResumeLayout(false);
            this.gb_MergeOpr.PerformLayout();
            this.gb_Duplicate.ResumeLayout(false);
            this.gb_Duplicate.PerformLayout();
            this.gb_Routine.ResumeLayout(false);
            this.gb_RemoveOpr.ResumeLayout(false);
            this.gb_RemoveOpr.PerformLayout();
            this.gb_Revise.ResumeLayout(false);
            this.gb_Revise.PerformLayout();
            this.gb_Replace.ResumeLayout(false);
            this.gb_Replace.PerformLayout();
            this.gb_Remove.ResumeLayout(false);
            this.gb_Remove.PerformLayout();
            this.gb_Add.ResumeLayout(false);
            this.gb_Add.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.gb_AddOpr.ResumeLayout(false);
            this.gb_AddOpr.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btn_ClearMom;
        private System.Windows.Forms.Button btn_RemoveMOM;
        private System.Windows.Forms.Button btn_AddMOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gb_Add;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddl_Add_Operations;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Add_Partnum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_AddMaterial;
        private System.Windows.Forms.TextBox txt_Add_Qty;
        private System.Windows.Forms.GroupBox gb_Remove;
        private System.Windows.Forms.Button btn_Remove;
        private System.Windows.Forms.ComboBox ddl_Remove_Part;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gb_Replace;
        private System.Windows.Forms.Button btn_Replace;
        private System.Windows.Forms.TextBox txt_Replace_Part;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ddl_Replace_Part;
        private System.Windows.Forms.GroupBox gb_Revise;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_Revise;
        private System.Windows.Forms.TextBox txt_Revise_Qty;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ddl_Revise_Part;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.GroupBox gb_RemoveOpr;
        private System.Windows.Forms.ComboBox ddl_Remove_Operations;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_RemoveOpr;
        private System.Windows.Forms.GroupBox gb_Routine;
        private System.Windows.Forms.Button but_Routine;
        private System.Windows.Forms.ComboBox cb_Routines;
        private System.Windows.Forms.GroupBox gb_Duplicate;
        private System.Windows.Forms.Button but_Duplicate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_DuplicateName;
        private System.Windows.Forms.Label lbl_Status;
        private System.Windows.Forms.GroupBox gb_MergeOpr;
        private System.Windows.Forms.ComboBox ddl_MergeToOpr;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox ddl_MergeFromOpr;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView lb_MOMs;
        private System.Windows.Forms.ColumnHeader Part;
        private System.Windows.Forms.ColumnHeader Revision;
        private System.Windows.Forms.GroupBox gb_AddOpr;
        private System.Windows.Forms.ComboBox ddl_AddOperation;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_AddOpr;
        private System.Windows.Forms.ComboBox ddl_ReviseOpr;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
    }
}

