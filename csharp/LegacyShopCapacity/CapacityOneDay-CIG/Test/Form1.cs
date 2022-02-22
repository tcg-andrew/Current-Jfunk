using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            capacityOneDay1.IsHidden = 0;
            capacityOneDay1.GreenToYellowCut = 6;
            capacityOneDay1.GreenToYellowTemper = 2;
            capacityOneDay1.MaxCut = 38;
            capacityOneDay1.MaxTemper = 10;
            //capacityOneDay1.DisplayDate = new DateTime(1, 1, 1);
            capacityOneDay1.Cut = 20;
            capacityOneDay1.Temper = 3;
            capacityOneDay1.DisplayDate = new DateTime(2010, 11, 18);
        }
    }
}
