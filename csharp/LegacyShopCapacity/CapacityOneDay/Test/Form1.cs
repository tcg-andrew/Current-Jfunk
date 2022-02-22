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
            capacityOneDay1.GreenToYellowDoors = 6;
            capacityOneDay1.GreenToYellowFrames = 2;
            capacityOneDay1.MaxDoors = 38;
            capacityOneDay1.MaxFrames = 10;
            //capacityOneDay1.DisplayDate = new DateTime(1, 1, 1);
            capacityOneDay1.Doors = 38;
            capacityOneDay1.Frames = 11;
            capacityOneDay1.FrameDoorOpens = 51;
            capacityOneDay1.DisplayDate = new DateTime(2010, 5, 5);
        }
    }
}
