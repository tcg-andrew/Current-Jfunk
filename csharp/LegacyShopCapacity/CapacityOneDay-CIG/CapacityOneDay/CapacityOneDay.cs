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
        private int intCut;
        private int intTemper;
        private int intUnit;
        private byte isInSuper7;
        private int intGreenToYellowCut;
        private int intGreenToYellowTemper;
        private int intGreenToYellowUnit;
        private int intMaxCut;
        private int intMaxTemper;
        private int intMaxUnit;
        private byte isHidden;
        private int intCutCap;
        private int intTemperCap;
        private int intUnitCap;


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

        public int Cut
        {
            get
            {
                return intCut;
            }
            set
            {
                intCut = value;
                intCutCap = intMaxCut - intCut;
                this.textBoxCut.Text = intCutCap.ToString("N0");
                Invalidate();
            }
        }

        public int Temper
        {
            get
            {
                return intTemper;
            }
            set
            {
                intTemper = value;
                intTemperCap = intMaxTemper - intTemper;
                this.textBoxTemper.Text = intTemperCap.ToString("N0");
                Invalidate();
            }
        }

        public int Unit
        {
            get
            {
                return intUnit;
            }
            set
            {
                intUnit = value;
                intUnitCap = intMaxUnit - intUnit;
                this.textBoxUnit.Text = intUnitCap.ToString("N0");
                Invalidate();
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

        public int GreenToYellowCut
        {
            get
            {
                return intGreenToYellowCut;
            }
            set
            {
                intGreenToYellowCut = value;
                Invalidate();
            }
        }

        public int GreenToYellowTemper
        {
            get
            {
                return intGreenToYellowTemper;
            }
            set
            {
                intGreenToYellowTemper = value;
                Invalidate();
            }
        }

        public int GreenToYellowUnit
        {
            get
            {
                return intGreenToYellowUnit;
            }
            set
            {
                intGreenToYellowUnit = value;
                Invalidate();
            }
        }

        public int MaxCut
        {
            get
            {
                return intMaxCut;
            }
            set
            {
                intMaxCut = value;
                Invalidate();
            }
        }

        public int MaxTemper
        {
            get
            {
                return intMaxTemper;
            }
            set
            {
                intMaxTemper = value;
                Invalidate();
            }
        }

        public int MaxUnit
        {
            get
            {
                return intMaxUnit;
            }
            set
            {
                intMaxUnit = value;
                Invalidate();
            }
        }

        private void CapacityOneDay_Paint(object sender, PaintEventArgs e)
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
                this.textBoxDate.BackColor = System.Drawing.Color.HotPink ;
            }
            else
            {
                this.textBoxDate.BackColor = System.Drawing.Color.White ;
            }

            if (this.GreenToYellowCut > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxCut.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxCut.BackColor = System.Drawing.Color.SpringGreen;
                }
            }
            else
            {
                this.textBoxCut.BackColor = System.Drawing.Color.Crimson;
            }

            if (this.GreenToYellowTemper>0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxTemper.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxTemper.BackColor = System.Drawing.Color.SpringGreen;
                }
            }
            else
            {
                this.textBoxTemper.BackColor = System.Drawing.Color.Crimson;
            }

            if (this.GreenToYellowUnit > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxUnit.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxUnit.BackColor = System.Drawing.Color.SpringGreen;
                }
            }
            else
            {
                this.textBoxUnit.BackColor = System.Drawing.Color.Crimson;
            }

            if (intCutCap <= this.GreenToYellowCut && this.GreenToYellowCut > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxCut.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxCut.BackColor = System.Drawing.Color.Yellow;
                }
            }

            if (intCutCap <= 0)
                {
                    this.textBoxCut.BackColor = System.Drawing.Color.Crimson;
                }

            if (intTemperCap <= this.GreenToYellowTemper && this.GreenToYellowTemper > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxTemper.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxTemper.BackColor = System.Drawing.Color.Yellow;
                }
            }

            if (intTemperCap <= 0)
            {
                this.textBoxTemper.BackColor = System.Drawing.Color.Crimson;
            }

            if (intUnitCap <= this.GreenToYellowUnit && this.GreenToYellowUnit > 0)
            {
                if (this.isInSuper7 == 1)
                {
                    this.textBoxUnit.BackColor = System.Drawing.Color.HotPink;
                }
                else
                {
                    this.textBoxUnit.BackColor = System.Drawing.Color.Yellow;
                }
            }

            if (intUnitCap <= 0)
            {
                this.textBoxUnit.BackColor = System.Drawing.Color.Crimson;
            }
        }

        private void CapacityOneDay_Load(object sender, EventArgs e)
        {
        }

 //        private void dayDetailToolStripMenuItem_Click(object sender, EventArgs e)
   //     {
     //       DayDetail frmDayDetail = new DayDetail(this.DisplayDate.ToString("MM/dd/yyyy"),"No");
       //     frmDayDetail.Show();
      //  }

//        private void dayDetailAndAllPriorToolStripMenuItem_Click(object sender, EventArgs e)
  //      {
    //        DayDetail frmDayDetail = new DayDetail(this.DisplayDate.ToString("MM/dd/yyyy"), "Yes");
      //      frmDayDetail.Show();
    //    }


    }
}
