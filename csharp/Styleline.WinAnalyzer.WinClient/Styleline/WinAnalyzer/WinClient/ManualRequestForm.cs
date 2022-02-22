namespace Styleline.WinAnalyzer.WinClient
{
    using DevExpress.XtraEditors;
    using Styleline.WinAnalyzer.CommPipe;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using ObjectLibrary;
    using System.Configuration;

    public class ManualRequestForm : XtraForm
    {
        private BindingSource analyzerRequestBindingSource;
        private TextEdit assemblyNumberTextBox;
        private SimpleButton btnCancel;
        private SimpleButton btnOk;
        private IContainer components;
        private TextEdit descriptionTextBox;
        private CheckEdit isFrameCheckEdit;
        private TextEdit itemNumberTextEdit;
        private TextEdit jobNumberTextBox;
        private TextEdit jobQtyTextBox;
        public string LineDescription;


        public ManualRequestForm()
        {
            this.InitializeComponent();
            this.analyzerRequestBindingSource.DataSource = new AnalyzerRequest();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.assemblyNumberTextBox = new TextEdit();
            this.assemblyNumberTextBox.EnterMoveNextControl = true;
            this.assemblyNumberTextBox.LostFocus += new EventHandler(assemblyNumberTextBox_LostFocus);
            this.analyzerRequestBindingSource = new BindingSource(this.components);
            this.descriptionTextBox = new TextEdit();
            this.isFrameCheckEdit = new CheckEdit();
            this.itemNumberTextEdit = new TextEdit();
            this.jobNumberTextBox = new TextEdit();
            this.jobNumberTextBox.EnterMoveNextControl = true;
            this.jobQtyTextBox = new TextEdit();
            this.btnOk = new SimpleButton();
            this.btnCancel = new SimpleButton();
            LabelControl control = new LabelControl();
            LabelControl control2 = new LabelControl();
            LabelControl control3 = new LabelControl();
            LabelControl control4 = new LabelControl();
            LabelControl control5 = new LabelControl();
            LabelControl control6 = new LabelControl();
            this.assemblyNumberTextBox.Properties.BeginInit();
            ((ISupportInitialize) this.analyzerRequestBindingSource).BeginInit();
            this.descriptionTextBox.Properties.BeginInit();
            this.isFrameCheckEdit.Properties.BeginInit();
            this.itemNumberTextEdit.Properties.BeginInit();
            this.jobNumberTextBox.Properties.BeginInit();
            this.jobQtyTextBox.Properties.BeginInit();
            base.SuspendLayout();
            control.Location = new Point(0x25, 0x24);
            control.Name = "assemblyNumberLabel";
            control.Size = new Size(0x59, 13);
            control.TabIndex = 1;
            control.Text = "Assembly Number:";
            control2.Location = new Point(0x45, 0x58);
            control2.Name = "descriptionLabel";
            control2.Size = new Size(0x39, 13);
            control2.TabIndex = 3;
            control2.Text = "Description:";
            control3.Location = new Point(80, 0x8b);
            control3.Name = "isFrameLabel";
            control3.Size = new Size(0x2e, 13);
            control3.TabIndex = 9;
            control3.Text = "Is Frame:";
            control4.Location = new Point(60, 0x3e);
            control4.Name = "itemNumberLabel";
            control4.Size = new Size(0x42, 13);
            control4.TabIndex = 11;
            control4.Text = "Item Number:";
            control5.Location = new Point(0x41, 10);
            control5.Name = "jobNumberLabel";
            control5.Size = new Size(0x3d, 13);
            control5.TabIndex = 13;
            control5.Text = "Job Number:";
            control6.Location = new Point(0x54, 0x72);
            control6.Name = "jobQtyLabel";
            control6.Size = new Size(0x2a, 13);
            control6.TabIndex = 15;
            control6.Text = "Job Qty:";
            this.assemblyNumberTextBox.DataBindings.Add(new Binding("Text", this.analyzerRequestBindingSource, "AssemblyNumber", true));
            this.assemblyNumberTextBox.Location = new Point(140, 0x21);
            this.assemblyNumberTextBox.Name = "assemblyNumberTextBox";
            this.assemblyNumberTextBox.Size = new Size(0xa8, 20);
            this.assemblyNumberTextBox.TabIndex = 1;
            this.analyzerRequestBindingSource.DataSource = typeof(AnalyzerRequest);
            this.descriptionTextBox.DataBindings.Add(new Binding("Text", this.analyzerRequestBindingSource, "Description", true));
            this.descriptionTextBox.Location = new Point(140, 0x55);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new Size(0xa8, 20);
            this.descriptionTextBox.TabIndex = 3;
            this.isFrameCheckEdit.DataBindings.Add(new Binding("EditValue", this.analyzerRequestBindingSource, "IsFrame", true));
            this.isFrameCheckEdit.Location = new Point(140, 0x88);
            this.isFrameCheckEdit.Name = "isFrameCheckEdit";
            this.isFrameCheckEdit.Properties.Caption = "";
            this.isFrameCheckEdit.Size = new Size(100, 0x13);
            this.isFrameCheckEdit.TabIndex = 5;
            this.itemNumberTextEdit.DataBindings.Add(new Binding("EditValue", this.analyzerRequestBindingSource, "ItemNumber", true));
            this.itemNumberTextEdit.Location = new Point(140, 0x3b);
            this.itemNumberTextEdit.Name = "itemNumberTextEdit";
            this.itemNumberTextEdit.Size = new Size(0xa8, 20);
            this.itemNumberTextEdit.TabIndex = 2;
            this.jobNumberTextBox.DataBindings.Add(new Binding("Text", this.analyzerRequestBindingSource, "JobNumber", true));
            this.jobNumberTextBox.Location = new Point(140, 7);
            this.jobNumberTextBox.Name = "jobNumberTextBox";
            this.jobNumberTextBox.Size = new Size(0xa8, 20);
            this.jobNumberTextBox.TabIndex = 0;
            this.jobQtyTextBox.DataBindings.Add(new Binding("Text", this.analyzerRequestBindingSource, "JobQty", true));
            this.jobQtyTextBox.Location = new Point(140, 0x6f);
            this.jobQtyTextBox.Name = "jobQtyTextBox";
            this.jobQtyTextBox.Size = new Size(0xa8, 20);
            this.jobQtyTextBox.TabIndex = 4;
            this.btnOk.Location = new Point(0x98, 0xac);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x4b, 0x17);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnCancel.Location = new Point(0xe9, 0xac);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x151, 0xcf);
            base.ControlBox = false;
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(control);
            base.Controls.Add(this.assemblyNumberTextBox);
            base.Controls.Add(control2);
            base.Controls.Add(this.descriptionTextBox);
            base.Controls.Add(control3);
            base.Controls.Add(this.isFrameCheckEdit);
            base.Controls.Add(control4);
            base.Controls.Add(this.itemNumberTextEdit);
            base.Controls.Add(control5);
            base.Controls.Add(this.jobNumberTextBox);
            base.Controls.Add(control6);
            base.Controls.Add(this.jobQtyTextBox);
            base.FormBorderStyle = FormBorderStyle.Fixed3D;
            base.Name = "ManualRequestForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Manual Request";
            this.assemblyNumberTextBox.Properties.EndInit();
            ((ISupportInitialize) this.analyzerRequestBindingSource).EndInit();
            this.descriptionTextBox.Properties.EndInit();
            this.isFrameCheckEdit.Properties.EndInit();
            this.itemNumberTextEdit.Properties.EndInit();
            this.jobNumberTextBox.Properties.EndInit();
            this.jobQtyTextBox.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        void assemblyNumberTextBox_LostFocus(object sender, EventArgs e)
        {
            try
            {
                if (jobNumberTextBox.Text.Length > 0 && assemblyNumberTextBox.Text.Length > 0)
                {
                    PowerAnalyzerInterface pai = new PowerAnalyzerInterface();
                    string[] results = pai.GetAssemblyDetails(ConfigurationManager.AppSettings["server"].ToString(), ConfigurationManager.AppSettings["vantage_database"].ToString(), ConfigurationManager.AppSettings["user"].ToString(), ConfigurationManager.AppSettings["password"].ToString(), ConfigurationManager.AppSettings["company"].ToString(), jobNumberTextBox.Text, Int32.Parse(assemblyNumberTextBox.Text));
                    if (results[0].Length > 0)
                    {
                        itemNumberTextEdit.Text = results[0];
                        descriptionTextBox.Text = results[1];
                        LineDescription = results[1];
                        jobQtyTextBox.Text = results[2];
                        isFrameCheckEdit.Checked = results[3] == "1";
//                        btnOk_Click(sender, e);
                    }
                }
            }
            catch (Exception eX)
            {
                MessageBox.Show(eX.Message);
            }
        }

        public AnalyzerRequest CurrentRequest
        {
            get
            {
                return (this.analyzerRequestBindingSource.Current as AnalyzerRequest);
            }
        }
    }
}

