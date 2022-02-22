using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCAttributizer
{
    public partial class Form1 : Form
    {
        private Dictionary<string, List<string>> attributes;
        private Dictionary<string, List<string>> inputs;

        public Form1()
        {
            InitializeComponent();
            attributes = new Dictionary<string, List<string>>();
            inputs = new Dictionary<string, List<string>>();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_NewInput.Text))
            {
                if (inputs.ContainsKey(txt_NewInput.Text))
                {
                    MessageBox.Show("Input " + txt_NewInput.Text + " already exists.");
                }
                else
                {
                    GroupBox newGrp = new GroupBox();
                    newGrp.Text = txt_NewInput.Text;
                    newGrp.Width = grp_NewInput.Width;
                    newGrp.Height = 72;

                    ComboBox newCmb = new ComboBox();
                    newCmb.Width = 190;
                    newCmb.DropDownStyle = ComboBoxStyle.DropDownList;
                    newCmb.Location = new Point(6, 20);
                    newGrp.Controls.Add(newCmb);

                    TextBox newTxt = new TextBox();
                    newTxt.Width = 150;
                    newTxt.Location = new Point(6, 46);
                    newGrp.Controls.Add(newTxt);

                    Button newOptBtn = new Button();
                    newOptBtn.Text = "Add Opt";
                    newOptBtn.Width = 75;
                    newOptBtn.Location = new Point(165, 44);
                    newOptBtn.Click += new EventHandler(AddOption);
                    newGrp.Controls.Add(newOptBtn);

                    Button newCondBtn = new Button();
                    newCondBtn.Text = ">";
                    newCondBtn.Width = 33;
                    newCondBtn.Height = 21;
                    newCondBtn.Location = new Point(206, 19);
                    newCondBtn.Click += new EventHandler(AddCondition);

                    newGrp.Controls.Add(newCondBtn);

                    flp_Inputs.Controls.Add(newGrp);

                    inputs.Add(txt_NewInput.Text, new List<string>());
                }
                txt_NewInput.Text = "";

            }
        }

        private void AddOption(Object sender, EventArgs e)
        {
            GroupBox grp = ((Button)sender).Parent as GroupBox;
            string input = grp.Text;

            TextBox txt = grp.Controls[1] as TextBox;
            if (!String.IsNullOrEmpty(txt.Text))
            {
                if (inputs[input].Contains(txt.Text))
                {
                    MessageBox.Show("Value " + txt.Text + " already exists for Input " + input);
                }
                else
                {
                    ComboBox cmb = grp.Controls[0] as ComboBox;
                    cmb.Items.Add(txt.Text);
                    cmb.SelectedIndex = cmb.Items.Count - 1;

                    inputs[input].Add(txt.Text);
                }
            }
            txt.Text = "";

        }

        private void AddCondition(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmb_Value.SelectedItem.ToString()))
            {
                GroupBox grp = ((Button)sender).Parent as GroupBox;
                string input = grp.Text;

                string value = ((ComboBox)grp.Controls[0]).SelectedItem.ToString();

                GroupBox newCond = new GroupBox();
                newCond.Text = input;
                newCond.Width = 225;
                newCond.Height = 41;

                Label newValue = new Label();
                newValue.Text = value;
                newValue.Location = new Point(6, 16);
                newCond.Controls.Add(newValue);

                Button newBtn = new Button();
                newBtn.Text = "X";
                newBtn.Width = 25;
                newBtn.Height = 23;
                newBtn.Location = new Point(194, 11);
                newCond.Controls.Add(newBtn);

                flp_Cond.Controls.Add(newCond);
            }
        }

        private void btn_AddAttr_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_Attr.Text))
            {
                if (!attributes.ContainsKey(txt_Attr.Text))
                {
                    cmb_Attr.Items.Add(txt_Attr.Text);
                    attributes.Add(txt_Attr.Text, new List<string>());
                    cmb_Attr.SelectedIndex = cmb_Attr.Items.Count - 1;
                }
                else
                {
                    MessageBox.Show("Attribute " + txt_Attr.Text + " already exists");
                }
                txt_Attr.Text = "";
            }
        }

        private void cmb_Attr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmb_Attr.SelectedItem.ToString()))
            {
                grp_Values.Visible = true;
                PopulateAttributeValues();
                PopulateValueConditions();
            }
            else
            {
                grp_Values.Visible = false;
            }
        }

        private void btn_AddValue_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_Value.Text))
            {
                if (!attributes[cmb_Attr.SelectedItem.ToString()].Contains(txt_Value.Text))
                {
                    attributes[cmb_Attr.SelectedItem.ToString()].Add(txt_Value.Text);
                    cmb_Value.Items.Add(txt_Value.Text);
                    cmb_Value.SelectedIndex = cmb_Value.Items.Count - 1;
                }
                else
                {
                    MessageBox.Show("Value " + txt_Value.Text + " already exists for Attribute " + cmb_Attr.SelectedItem.ToString());
                }
                txt_Value.Text = "";
            }
        }

        private void PopulateAttributeValues()
        {
            cmb_Value.Items.Clear();
            foreach (string str in attributes[cmb_Attr.SelectedItem.ToString()])
                cmb_Value.Items.Add(str);
            if (cmb_Value.Items.Count > 0)
                cmb_Value.SelectedIndex = 0;
        }

        private void PopulateValueConditions()
        {
            flp_Cond.Controls.Clear();
        }
    }
}
