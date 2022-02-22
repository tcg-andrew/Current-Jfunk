using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CapacityCal
{
    public partial class CapacityCal : Form
    {
        BindingSource _bindsrc = new BindingSource();

        public CapacityCal()
        {
            InitializeComponent();
            SqlConnection myConn = new SqlConnection("Data Source=sql;Database=devsql;Integrated Security=SSPI");

            try
            {
                myConn.Open();

                SqlCommand query = new SqlCommand("SELECT * from _tcg_CapCal", myConn);
                SqlDataAdapter adrQuery = new SqlDataAdapter(query);
                DataSet dsQuery = new DataSet();
                adrQuery.Fill(dsQuery);

                BindingNavigator _bindnav = new BindingNavigator(true);
                _bindsrc.DataSource = dsQuery;
                _bindnav.BindingSource = _bindsrc;
                //DataGridView dataGridView1 = new DataGridView();
                dataGridView1.DataSource = _bindsrc;
                this.Controls.Add(dataGridView1);
            }

            catch (Exception)
            {
                MessageBox.Show("Exception retrieving data.");
            }

            finally
            {
                myConn.Close();
            }

        }
    }
}
