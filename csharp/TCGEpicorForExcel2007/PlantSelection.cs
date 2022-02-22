#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TCGEpicorForExcel2007.PartServiceReference;

#endregion

namespace TCGEpicorForExcel2007
{
    public partial class PlantSelection : Form
    {
        #region Properties

        public string SelectedPlant
        {
            get
            {
                return comboBox1.SelectedItem.ToString();
            }
            set
            {
                comboBox1.SelectedItem = value;
            }
        }

        public List<string> Plants
        {
            set
            {
                comboBox1.Items.Clear();
                foreach (string str in value)
                    comboBox1.Items.Add(str);

                if (comboBox1.Items.Count >= 1)
                    comboBox1.SelectedItem = comboBox1.Items[0];
            }
        }

        public List<partclass> Classes
        {
            set
            {
                comboBox2.Items.Clear();
                partclass all = new partclass();
                all.id = "%";
                all.description = "All";
                value.Insert(0, all);
                comboBox2.DataSource = value;
                comboBox2.DisplayMember = "Description";
                comboBox2.ValueMember = "ID";
                comboBox2.SelectedItem = comboBox2.Items[0];
            }
        }

        public string SelectedClass
        {
            get
            {
                return comboBox2.SelectedValue.ToString();
            }
        }

        #endregion

        #region Constructors

        public PlantSelection()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #endregion
    }
}
