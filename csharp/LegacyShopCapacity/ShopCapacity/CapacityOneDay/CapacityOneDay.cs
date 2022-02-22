using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CapacityOneDay
{
    public partial class CapacityOneDay : UserControl
    {
        private DateTime dteDisplayDate;
        private int intDoors;
        private int intFrames;
        private int intFrameDoorOpens;
        private byte isInSuper7;
        private int intGreenToYellowDoors;
        private int intGreenToYellowFrames;
        private int intMaxDoors;
        private int intMaxFrames;
        private byte isHidden;
        private int intDoorCap;
        private int intFrameCap;


        public CapacityOneDay()
        {
            InitializeComponent();
        }

        public DateTime DisplayDate
        {
            get
            {
                return dteDisplayDate;
            }
            set
            {
                dteDisplayDate = value;
                this.textBoxDate.Text = dteDisplayDate.ToString("M/d/yy");
                Invalidate();
            }
        }

        public int Doors
        {
            get
            {
                return intDoors;
            }
            set
            {
                intDoors = value;
                intDoorCap = intMaxDoors - intDoors;
                this.textBoxDoors.Text = intDoorCap.ToString("N0");
                Invalidate();
            }
        }

        public int Frames
        {
            get
            {
                return intFrames;
            }
            set
            {
                intFrames = value;
                intFrameCap=intMaxFrames-intFrames;
                this.textBoxFrames.Text = intFrameCap.ToString("N0");
                Invalidate();
            }
        }

        public int FrameDoorOpens
        {
            get
            {
                return intFrameDoorOpens;
            }
            set
            {
                intFrameDoorOpens = value;
            }
        }

        public byte InSuper7
        {
            get
            {
                return isInSuper7;
            }
            set
            {
                isInSuper7 = value;
                Invalidate();
            }
        }

        public byte IsHidden
        {
            get
            {
                return isHidden;
            }
            set
            {
                isHidden = value;
                Invalidate();
            }
        }

        public int GreenToYellowDoors
        {
            get
            {
                return intGreenToYellowDoors;
            }
            set
            {
                intGreenToYellowDoors = value;
                Invalidate();
            }
        }

        public int GreenToYellowFrames
        {
            get
            {
                return intGreenToYellowFrames;
            }
            set
            {
                intGreenToYellowFrames = value;
                Invalidate();
            }
        }

        public int MaxDoors
        {
            get
            {
                return intMaxDoors;
            }
            set
            {
                intMaxDoors = value;
                Invalidate();
            }
        }

        public int MaxFrames
        {
            get
            {
                return intMaxFrames;
            }
            set
            {
                intMaxFrames = value;
                Invalidate();
            }
        }

        private void CapacityOneDay_Load(object sender, EventArgs e)
        {
            if (this.isHidden == 1)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
            }

            if (this.isInSuper7 == 1)
            {
                this.textBoxDate.BackColor = System.Drawing.Color.HotPink;
            }
            else
            {
                this.textBoxDate.BackColor = System.Drawing.Color.White;
            }

            if (this.GreenToYellowDoors > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxDoors.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxDoors.BackColor = System.Drawing.Color.SpringGreen;
                }
            }
            else
            {
                this.textBoxDoors.BackColor = System.Drawing.Color.Crimson;
            }

            if (this.GreenToYellowFrames > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxFrames.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxFrames.BackColor = System.Drawing.Color.SpringGreen;
                }
            }
            else
            {
                this.textBoxFrames.BackColor = System.Drawing.Color.Crimson;
            }

            if (intDoorCap <= this.GreenToYellowDoors && this.GreenToYellowDoors > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxDoors.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxDoors.BackColor = System.Drawing.Color.Yellow;
                }
            }

            if (intDoorCap <= 0)
            {
                this.textBoxDoors.BackColor = System.Drawing.Color.Crimson;
            }

            if (intFrameCap <= this.GreenToYellowFrames && this.GreenToYellowFrames > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxFrames.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxFrames.BackColor = System.Drawing.Color.Yellow;
                }
            }

            if (intFrameCap <= 0)
            {
                this.textBoxFrames.BackColor = System.Drawing.Color.Crimson;
            }
        }

        private void frameDoorOpeningsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Frame Door-Openings: "+FrameDoorOpens.ToString("N0"));
        }

        private void dayDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DayDetail frmDayDetail = new DayDetail(this.DisplayDate.ToString("MM/dd/yyyy"),"No");
            frmDayDetail.Show();
        }

        private void dayDetailAndAllPriorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DayDetail frmDayDetail = new DayDetail(this.DisplayDate.ToString("MM/dd/yyyy"), "Yes");
            frmDayDetail.Show();
        }


    }
}
