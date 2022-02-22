using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DayDetail
{
    public partial class DayDetail : Form
    {
        public DayDetail(string strDate)
        {
            this.Text = "Day Detail for " + strDate;
            InitializeComponent();
        }
    }
}
