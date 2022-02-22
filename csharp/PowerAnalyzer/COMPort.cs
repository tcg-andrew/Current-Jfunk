using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerAnalyzer
{
    public partial class COMPort : Form
    {
        public string COM { get; set; }

        public COMPort()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            COM = comboBox1.SelectedItem.ToString();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void COMPort_Shown(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
