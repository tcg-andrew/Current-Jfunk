namespace Styleline.WinAnalyzer.WinClient
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SettingsForm : XtraForm
    {
        private SimpleButton btnCancel;
        private SimpleButton btnOK;
        private CheckEdit clearPrintSettingsCheckEdit;
        private ComboBoxEdit comboBoxEdit1;
        private IContainer components;
        private CheckEdit enableDoorlineRecordCheckEdit;
        private CheckEdit enableFramelineRecordCheckEdit;
        private CheckEdit enablePrintULLabelCheckEdit;
        private BindingSource settingsTOBindingSource;

        public SettingsForm()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SettingsTO.Default.LoadSettings();
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.clearPrintSettingsCheckEdit.Checked)
            {
                SettingsTO.Default.PrinterName = string.Empty;
            }
            SettingsTO.Default.SaveSettings();
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
            this.comboBoxEdit1 = new ComboBoxEdit();
            this.enableDoorlineRecordCheckEdit = new CheckEdit();
            this.enableFramelineRecordCheckEdit = new CheckEdit();
            this.enablePrintULLabelCheckEdit = new CheckEdit();
            this.btnOK = new SimpleButton();
            this.btnCancel = new SimpleButton();
            this.clearPrintSettingsCheckEdit = new CheckEdit();
            this.settingsTOBindingSource = new BindingSource(this.components);
            LabelControl control = new LabelControl();
            this.comboBoxEdit1.Properties.BeginInit();
            this.enableDoorlineRecordCheckEdit.Properties.BeginInit();
            this.enableFramelineRecordCheckEdit.Properties.BeginInit();
            this.enablePrintULLabelCheckEdit.Properties.BeginInit();
            this.clearPrintSettingsCheckEdit.Properties.BeginInit();
            ((ISupportInitialize) this.settingsTOBindingSource).BeginInit();
            base.SuspendLayout();
            control.Location = new Point(0x27, 0x1c);
            control.Name = "comPortLabel";
            control.Size = new Size(0x30, 13);
            control.TabIndex = 3;
            control.Text = "Com Port:";
            this.comboBoxEdit1.DataBindings.Add(new Binding("EditValue", this.settingsTOBindingSource, "ComPort", true));
            this.comboBoxEdit1.Location = new Point(0x2a, 0x2c);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] { "NONE", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8" });
            this.comboBoxEdit1.Size = new Size(100, 20);
            this.comboBoxEdit1.TabIndex = 4;
            this.enableDoorlineRecordCheckEdit.DataBindings.Add(new Binding("EditValue", this.settingsTOBindingSource, "EnableDoorlineRecord", true));
            this.enableDoorlineRecordCheckEdit.Location = new Point(40, 70);
            this.enableDoorlineRecordCheckEdit.Name = "enableDoorlineRecordCheckEdit";
            this.enableDoorlineRecordCheckEdit.Properties.Caption = "Enable Doorline Record";
            this.enableDoorlineRecordCheckEdit.Size = new Size(0xa4, 0x13);
            this.enableDoorlineRecordCheckEdit.TabIndex = 5;
            this.enableFramelineRecordCheckEdit.DataBindings.Add(new Binding("EditValue", this.settingsTOBindingSource, "EnableFramelineRecord", true));
            this.enableFramelineRecordCheckEdit.Location = new Point(40, 0x5f);
            this.enableFramelineRecordCheckEdit.Name = "enableFramelineRecordCheckEdit";
            this.enableFramelineRecordCheckEdit.Properties.Caption = "Enable Frameline Record";
            this.enableFramelineRecordCheckEdit.Size = new Size(0xa4, 0x13);
            this.enableFramelineRecordCheckEdit.TabIndex = 6;
            this.enablePrintULLabelCheckEdit.DataBindings.Add(new Binding("EditValue", this.settingsTOBindingSource, "EnablePrintULLabel", true));
            this.enablePrintULLabelCheckEdit.Location = new Point(40, 120);
            this.enablePrintULLabelCheckEdit.Name = "enablePrintULLabelCheckEdit";
            this.enablePrintULLabelCheckEdit.Properties.Caption = "Enable Print UL Label";
            this.enablePrintULLabelCheckEdit.Size = new Size(0xa4, 0x13);
            this.enablePrintULLabelCheckEdit.TabIndex = 7;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x81, 0xb7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x81, 0xd4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.clearPrintSettingsCheckEdit.Location = new Point(40, 0x91);
            this.clearPrintSettingsCheckEdit.Name = "clearPrintSettingsCheckEdit";
            this.clearPrintSettingsCheckEdit.Properties.Caption = "Clear UL Print Settings";
            this.clearPrintSettingsCheckEdit.Size = new Size(0x86, 0x13);
            this.clearPrintSettingsCheckEdit.TabIndex = 10;
            this.settingsTOBindingSource.DataSource = typeof(SettingsTO);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0xfd, 0xff);
            base.ControlBox = false;
            base.Controls.Add(this.clearPrintSettingsCheckEdit);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.enablePrintULLabelCheckEdit);
            base.Controls.Add(this.enableFramelineRecordCheckEdit);
            base.Controls.Add(this.enableDoorlineRecordCheckEdit);
            base.Controls.Add(this.comboBoxEdit1);
            base.Controls.Add(control);
            base.Name = "SettingsForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Settings";
            base.Load += new EventHandler(this.SettingsForm_Load);
            this.comboBoxEdit1.Properties.EndInit();
            this.enableDoorlineRecordCheckEdit.Properties.EndInit();
            this.enableFramelineRecordCheckEdit.Properties.EndInit();
            this.enablePrintULLabelCheckEdit.Properties.EndInit();
            this.clearPrintSettingsCheckEdit.Properties.EndInit();
            ((ISupportInitialize) this.settingsTOBindingSource).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.settingsTOBindingSource.DataSource = SettingsTO.Default;
        }
    }
}

