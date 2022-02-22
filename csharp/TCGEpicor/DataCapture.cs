using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TCGEpicor
{
    public partial class DataCapture : Form
    {
        private Dictionary<string, Dictionary<string, string>> _dataCaptures;

        private Dictionary<string, Control> inputs;

        public Dictionary<string, Dictionary<string, string>> DataCaptures 
        {
            set
            {
                _dataCaptures = value;
                flowLayoutPanel1.Controls.Clear();
                foreach (string key in _dataCaptures.Keys)
                {
                    FlowLayoutPanel input = new FlowLayoutPanel();
                    input.FlowDirection = FlowDirection.LeftToRight;
                    input.AutoSize = true;
                    Label lbl = new Label();
                    lbl.Text = key;
                    lbl.AutoSize = true;
                    input.Controls.Add(lbl);
                    if (_dataCaptures[key].Keys.Count > 0 && !_dataCaptures[key].Keys.Contains("FILE"))
                    {
                        ComboBox cb = new ComboBox();
                        int maxlength = 0;
                        foreach (string v in _dataCaptures[key].Keys)
                        {
                            cb.Items.Add(v);
                            if (v.Length > maxlength)
                                maxlength = v.Length;
                        }
                        cb.DropDownStyle = ComboBoxStyle.DropDownList;
                        cb.Width = (maxlength + 3) * 6 + SystemInformation.VerticalScrollBarWidth;
                        if (SelectedValues.ContainsKey(key))
                            cb.SelectedItem = _dataCaptures[key][SelectedValues[key]];
                        else
                            cb.SelectedIndex = 0;
                        inputs.Add(key, cb);
                        input.Controls.Add(cb);
                    }
                    else
                    {
                            TextBox tb = new TextBox();
                            if (_dataCaptures[key].Keys.Count > 0 && _dataCaptures[key].Keys.Contains("FILE"))
                            {
                                tb.Click += new EventHandler(tb_Click);

                            }

                            inputs.Add(key, tb);
                            input.Controls.Add(tb);

                    }
                    flowLayoutPanel1.Controls.Add(input);
                }
            }
        }

        void tb_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.AddExtension = true;
            ofd.DefaultExt = "csv";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ((TextBox)sender).Text = ofd.FileName;
            }
        }

        public Dictionary<string, string> SelectedValues { get; set; }

        public DataCapture()
        {
            inputs = new Dictionary<string, Control>();
            SelectedValues = new Dictionary<string, string>();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedValues = new Dictionary<string, string>();
            foreach (string key in _dataCaptures.Keys)
            {
                if (_dataCaptures[key].Keys.Count > 0 && !_dataCaptures[key].Keys.Contains("FILE"))
                {
                    SelectedValues.Add(key, _dataCaptures[key][((ComboBox)inputs[key]).SelectedItem.ToString()]);
                }
                else
                {
                    SelectedValues.Add(key, ((TextBox)inputs[key]).Text);
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
