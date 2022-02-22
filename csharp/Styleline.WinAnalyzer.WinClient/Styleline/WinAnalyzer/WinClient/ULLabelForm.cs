namespace Styleline.WinAnalyzer.WinClient
{
    using DevExpress.XtraEditors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ULLabelForm : Form
    {
        private SimpleButton btnCancel;
        private SimpleButton btnPrint;
        private IContainer components;
        private TextEdit descriptionTextEdit;
        private TextEdit heaterAmpsTextEdit;
        private TextEdit jobNumberTextEdit;
        private LabelControl labelControl1;
        private LabelControl labelControl2;
        private TextEdit lightAmpsTextEdit;
        private BindingSource ulLabelTOBindingSource;

        public ULLabelForm()
        {
            this.InitializeComponent();
            this.ulLabelTOBindingSource.DataSource = new UlLabelTO();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
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
            this.labelControl1 = new LabelControl();
            this.labelControl2 = new LabelControl();
            this.descriptionTextEdit = new TextEdit();
            this.heaterAmpsTextEdit = new TextEdit();
            this.lightAmpsTextEdit = new TextEdit();
            this.jobNumberTextEdit = new TextEdit();
            this.btnCancel = new SimpleButton();
            this.btnPrint = new SimpleButton();
            this.ulLabelTOBindingSource = new BindingSource(this.components);
            Label label = new Label();
            Label label2 = new Label();
            Label label3 = new Label();
            Label label4 = new Label();
            this.descriptionTextEdit.Properties.BeginInit();
            this.heaterAmpsTextEdit.Properties.BeginInit();
            this.lightAmpsTextEdit.Properties.BeginInit();
            this.jobNumberTextEdit.Properties.BeginInit();
            ((ISupportInitialize) this.ulLabelTOBindingSource).BeginInit();
            base.SuspendLayout();
            label.AutoSize = true;
            label.ForeColor = Color.White;
            label.Location = new Point(0x53, 0x24);
            label.Name = "descriptionLabel";
            label.Size = new Size(0x3f, 13);
            label.TabIndex = 3;
            label.Text = "Description:";
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(0x152, 0x24);
            label2.Name = "heaterAmpsLabel";
            label2.Size = new Size(0x47, 13);
            label2.TabIndex = 4;
            label2.Text = "Heater Amps:";
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(0x15b, 0x47);
            label3.Name = "lightAmpsLabel";
            label3.Size = new Size(0x3e, 13);
            label3.TabIndex = 6;
            label3.Text = "Light Amps:";
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(0x4f, 0x47);
            label4.Name = "jobNumberLabel";
            label4.Size = new Size(0x43, 13);
            label4.TabIndex = 8;
            label4.Text = "Job Number:";
            this.labelControl1.Appearance.Font = new Font("Tahoma", 36f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelControl1.Appearance.ForeColor = Color.White;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new Size(0x1f, 0x3a);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "U";
            this.labelControl2.Appearance.Font = new Font("Tahoma", 36f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelControl2.Appearance.ForeColor = Color.White;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new Point(0x31, 0x2f);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new Size(0x18, 0x3a);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "L";
            this.descriptionTextEdit.DataBindings.Add(new Binding("EditValue", this.ulLabelTOBindingSource, "Description", true));
            this.descriptionTextEdit.Location = new Point(0x98, 0x21);
            this.descriptionTextEdit.Name = "descriptionTextEdit";
            this.descriptionTextEdit.Size = new Size(0xa6, 20);
            this.descriptionTextEdit.TabIndex = 4;
            this.heaterAmpsTextEdit.DataBindings.Add(new Binding("EditValue", this.ulLabelTOBindingSource, "HeaterAmps", true));
            this.heaterAmpsTextEdit.Location = new Point(0x19f, 0x21);
            this.heaterAmpsTextEdit.Name = "heaterAmpsTextEdit";
            this.heaterAmpsTextEdit.Properties.Appearance.ForeColor = Color.Black;
            this.heaterAmpsTextEdit.Properties.Appearance.Options.UseForeColor = true;
            this.heaterAmpsTextEdit.Size = new Size(100, 20);
            this.heaterAmpsTextEdit.TabIndex = 5;
            this.lightAmpsTextEdit.DataBindings.Add(new Binding("EditValue", this.ulLabelTOBindingSource, "LightAmps", true));
            this.lightAmpsTextEdit.Location = new Point(0x19f, 0x44);
            this.lightAmpsTextEdit.Name = "lightAmpsTextEdit";
            this.lightAmpsTextEdit.Size = new Size(100, 20);
            this.lightAmpsTextEdit.TabIndex = 7;
            this.jobNumberTextEdit.DataBindings.Add(new Binding("EditValue", this.ulLabelTOBindingSource, "JobNumber", true));
            this.jobNumberTextEdit.Location = new Point(0x98, 0x44);
            this.jobNumberTextEdit.Name = "jobNumberTextEdit";
            this.jobNumberTextEdit.Size = new Size(0xa6, 20);
            this.jobNumberTextEdit.TabIndex = 9;
            this.btnCancel.Location = new Point(0x192, 0x5e);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x71, 0x24);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnPrint.Location = new Point(0x11b, 0x5e);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x71, 0x24);
            this.btnPrint.TabIndex = 11;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.ulLabelTOBindingSource.DataSource = typeof(UlLabelTO);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.ClientSize = new Size(570, 0x8e);
            base.Controls.Add(this.btnPrint);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(label4);
            base.Controls.Add(this.jobNumberTextEdit);
            base.Controls.Add(label3);
            base.Controls.Add(this.lightAmpsTextEdit);
            base.Controls.Add(label2);
            base.Controls.Add(this.heaterAmpsTextEdit);
            base.Controls.Add(label);
            base.Controls.Add(this.descriptionTextEdit);
            base.Controls.Add(this.labelControl2);
            base.Controls.Add(this.labelControl1);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ULLabelForm";
            base.ShowIcon = false;
            this.Text = "Print UL Labels";
            this.descriptionTextEdit.Properties.EndInit();
            this.heaterAmpsTextEdit.Properties.EndInit();
            this.lightAmpsTextEdit.Properties.EndInit();
            this.jobNumberTextEdit.Properties.EndInit();
            ((ISupportInitialize) this.ulLabelTOBindingSource).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public UlLabelTO LabelTO
        {
            get
            {
                return (this.ulLabelTOBindingSource.Current as UlLabelTO);
            }
            set
            {
                this.ulLabelTOBindingSource.DataSource = value;
            }
        }
    }
}

