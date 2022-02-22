using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;


namespace CopyCSV
{
    public partial class Form1 : Form
    {
        public IEnumerator nextFile;
        public int HowManyMore;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog() ;
            fBD.Description = "Choose the From Folder";

            if (fBD.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fBD.SelectedPath;
            }

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();
            fBD.Description = "Choose the Copy To Folder";

            if (fBD.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = fBD.SelectedPath;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            foreach (string f in Directory.GetFiles(textBox1.Text))
                this.listBox1.Items.Add(f);
            nextFile = listBox1.Items.GetEnumerator();
            HowManyMore = listBox1.Items.Count;
            if (nextFile.MoveNext())
            {
                HowManyMore--;
                listBox2.Items.Add(DateTime.Now.ToString());
                listBox2.Items.Add(nextFile.Current);
                decimal remainmin = (decimal)HowManyMore * numericUpDown1.Value;
                listBox2.Items.Add(remainmin.ToString());
                listBox2.Items.Add("Finish Time: "+DateTime.Now.AddMinutes((double)remainmin + (double)numericUpDown1.Value).ToLongTimeString());
                String howmorestr = textBox2.Text + "\\" + HowManyMore.ToString() + ".csv";
                File.Copy(nextFile.Current.ToString(),howmorestr, false);
            } 
            timer1.Interval = (int)((double)numericUpDown1.Value * 60.0 * 1000.0);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nextFile.MoveNext())
            {
                HowManyMore--;
                listBox2.Items.Add(DateTime.Now.ToString());
                listBox2.Items.Add(nextFile.Current);
                decimal remainmin = (decimal)HowManyMore * numericUpDown1.Value;
                listBox2.Items.Add(remainmin.ToString());
                listBox2.Items.Add("Finish Time: "+DateTime.Now.AddMinutes((double)remainmin + (double)numericUpDown1.Value).ToLongTimeString());
                String howmorestr = textBox2.Text + "\\" + HowManyMore.ToString() + ".csv";
                File.Copy(nextFile.Current.ToString(), howmorestr, false);
                
            }
            else
            {
                timer1.Enabled = false;
                button1.Enabled = true;
            }
        }
    }
}