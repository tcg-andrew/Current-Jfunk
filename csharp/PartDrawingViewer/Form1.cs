using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PartDrawingViewer
{
    public partial class Form1 : Form
    {
        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";

        DataSet ds;
        string Company;
        string Partnum;
        int cursor;

        public Form1(string[] args)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Class not registered"))
                {
                    MessageBox.Show("SolidWorks eDrawings Viewer is required to view drawings.  Please notify IT", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            try
            {
                Company = args[0];
                Partnum = args[1];

                LoadImages();

                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblNoDrawings.Visible = true;
                    lblNoDrawings.Text = "No drawings found for Part #" + Partnum + ". Please contact Engineering";
                    axEModelViewControl1.Visible = false;
                }
                else
                {
                    lblNoDrawings.Visible = false;
                    axEModelViewControl1.Visible = true;
                    this.WindowState = FormWindowState.Maximized;
                    cursor = 0;
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        tsNavigation.Visible = true;
                        toolStripButton1.Enabled = false;
                    }
                    DisplayImage(ds.Tables[0].Rows[cursor]["opr"].ToString(), ds.Tables[0].Rows[cursor]["xfilename"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadImages()
        {
            ds = new DataSet();

            SqlConnection connection = new SqlConnection("Data Source="+server+";Database="+database+";Integrated Security=SSPI");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from [dbo].fn_AttachmentsForPart(@Company, @Partnum)";
            command.Parameters.AddWithValue("Company", Company);
            command.Parameters.AddWithValue("Partnum", Partnum);

            connection.Open();

            SqlDataAdapter sda = new SqlDataAdapter(command);
            sda.Fill(ds);

            connection.Close();
        }

        private void DisplayImage(string operation, string filename)
        {
            MessageBox.Show(filename);
            string title = "Part # " + Partnum;
            if (!String.IsNullOrEmpty(operation))
                title += " Operation: " + operation;
            title += ". Display Image \"" + filename + "\"";

            this.Text = title;

            axEModelViewControl1.OpenDoc(filename, false, false, true, "");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            cursor--;
            if (cursor == 0)
                toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = true;
            DisplayImage(ds.Tables[0].Rows[cursor]["opr"].ToString(), ds.Tables[0].Rows[cursor]["xfilename"].ToString());
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            cursor++;
            if (cursor == ds.Tables[0].Rows.Count - 1)
                toolStripButton2.Enabled = false;
            toolStripButton1.Enabled = true;
            DisplayImage(ds.Tables[0].Rows[cursor]["opr"].ToString(), ds.Tables[0].Rows[cursor]["xfilename"].ToString());
        }
    }
}
