using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace TCGEpicor
{
    public partial class SearchForAcrobat : Form
    {
        public string AcrobatLocation { get; set; }

        public SearchForAcrobat()
        {
            InitializeComponent();
            progressBar1.Maximum = 10000;
            progressBar1.Step = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            button1.Enabled = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(SearchDirectory), "C:\\");
        }

        private void SearchDirectory(object state)
        {
            AcrobatLocation = RecursiveSearch("C:\\");
            if (String.IsNullOrEmpty(AcrobatLocation))
            {
                MessageBox.Show("A version of Adobe Acrobat or Acrobat Reader could not be found.  Please install one of those applications to enabled PDF printing", "No Acrobat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private string RecursiveSearch(string directory)
        {
            try
            {
                if (progressBar1.InvokeRequired)
                    progressBar1.Invoke(new MethodInvoker(delegate
                        {
                            progressBar1.PerformStep();
                            if (progressBar1.Value == progressBar1.Maximum)
                                progressBar1.Value = progressBar1.Minimum;
                        }
                    ));
                else
                {
                    progressBar1.PerformStep();
                    if (progressBar1.Value == progressBar1.Maximum)
                        progressBar1.Value = progressBar1.Minimum;
                }

                foreach (string f in Directory.GetFiles(directory, "AcroRd32.exe"))
                {
                    return f;
                }
                foreach (string f in Directory.GetFiles(directory, "Acrobat.exe"))
                {
                    return f;
                }

                foreach (string d in Directory.GetDirectories(directory))
                {
                    foreach (string f in Directory.GetFiles(directory, "AcroRd32.exe"))
                    {
                        return f;
                    }
                    foreach (string f in Directory.GetFiles(d, "Acrobat.exe"))
                    {
                        return f;
                    }
                    string recurse = RecursiveSearch(d);
                    if (!String.IsNullOrEmpty(recurse))
                        return recurse;
                }
            }
            catch
            {
            }

            return "";
        }

    }
}
