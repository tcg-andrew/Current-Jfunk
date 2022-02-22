#region Usings

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ObjectLibrary;
using System.Text;

#endregion

namespace HardMOMRevision
{
    public partial class Form1 : Form
    {
        #region Values

        SqlConnection connection;
        /*        string Server = "192.168.82.240";
                string Database = "epicor905";
                string Port = "9431";
                string Company = "PRC1";
                string VantageUser = "PRC1Service";
                string VantagePass = "v4nt4g3";
         */

        string Company = "CIG";
        //string Company = "CRD";

                string server = "SARV-SQLPROD01";
                string database = "EpicorDB";
                string username = "RailsAppUserP";
                        string password = "wA7tA1FaBS1MpLaU";

        /*string server = "SARV-SQLDEV01";
        string database = "EpicorDB_D";
        string username = "RailsAppUser";
        string password = "2fe8wJcH";*/


                string VantageServer = "SARV-APPEPCRP01";
        //string VantageServer = "SARV-APPEPCRD01";
        //string VantageUser = "CRDService";
        string VantageUser = "CIGFLService";
        string VantagePass = "gfd723trajsdc97";
        

        #endregion

        #region Constructor

        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection("data source="+server+";initial catalog=" + database + ";integrated security=SSPI");
            gb_Routine.Visible = false;
            this.Text = "Batch Part Maintenance - " + server + " - " + database;
        }

        #endregion

        #region Event Handlers

        private void btn_AddMOM_Click(object sender, EventArgs e)
        {
            MOMSearch s = new MOMSearch(server, database, Company);
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string str in s.Selected.Keys)
                {
                    if (!lb_MOMs.Items.Contains(new ListViewItem(new string[] { str, s.Selected[str] })))
                        lb_MOMs.Items.Add(new ListViewItem(new string[] { str, s.Selected[str] }));
                }
                if (lb_MOMs.Items.Count > 0)
                {
                    EnableBoxes(true);
                    PopulateOperations();
                    PopulateMaterials();
                }
            }
        }

        private void btn_ClearMom_Click(object sender, EventArgs e)
        {
            lb_MOMs.Items.Clear();
            EnableBoxes(false);
        }

        private void btn_RemoveMOM_Click(object sender, EventArgs e)
        {
            while (lb_MOMs.SelectedItems.Count > 0)
            {
                lb_MOMs.Items.Remove(lb_MOMs.SelectedItems[0]);
            }
            if (lb_MOMs.Items.Count == 0)
            {
                EnableBoxes(false);
            }
        }

        private void btn_AddMaterial_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            try
            {
                int qty;
                if (String.IsNullOrEmpty(txt_Add_Partnum.Text))
                    MessageBox.Show("Missing Part # To Add");
                else if (String.IsNullOrEmpty(txt_Add_Qty.Text))
                    MessageBox.Show("Missing Qty To Add");
                else if (!Int32.TryParse(txt_Add_Qty.Text, out qty))
                    MessageBox.Show("Qty Is Not A Number");
                else if (String.IsNullOrEmpty(ddl_Add_Operations.SelectedValue.ToString()))
                    MessageBox.Show("Operation Missing");
                else
                {
                    // Validate selected operation exists on all selected MOMs
                    bool valid = true;
                    foreach (ListViewItem item in lb_MOMs.Items)
                    {
                        string partnum = item.SubItems[0].Text;
                        string revnum = item.SubItems[1].Text;

                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "exec [dbo].sp_GetPartOperations @Company, @Partnum, @RevisionNum";
                        command.Parameters.AddWithValue("Company", Company);
                        command.Parameters.AddWithValue("Partnum", partnum);
                        command.Parameters.AddWithValue("RevisionNum", revnum);

                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        bool found = false;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["opcode"].ToString() == ((DataRowView)ddl_Add_Operations.SelectedItem)["opcode"].ToString() && row["oprseq"].ToString() == ((DataRowView)ddl_Add_Operations.SelectedItem)["oprseq"].ToString())
                                found = true;
                        }
                        if (!found)
                        {
                            MessageBox.Show("Validation Failed.  Operation '" + ((DataRowView)ddl_Add_Operations.SelectedItem)["opcode"].ToString() + "' not found on Part #" + partnum + ".  All selected MOMs must have the chosen operation.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            valid = false;
                        }
                    }

                    // Operation Validation passed
                    // Validate existance of part # to add
                    if (valid)
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "exec [dbo].sp_GetPartsExactMatch @Company, @Partnum";
                        command.Parameters.AddWithValue("Company", Company);
                        command.Parameters.AddWithValue("Partnum", txt_Add_Partnum.Text);

                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            MessageBox.Show("Validation Failed.  Part #" + txt_Add_Partnum.Text + " does not exist.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            valid = false;
                        }

                    }

                    // Part existance validation passed
                    // Add the material to each MOM
                    if (valid)
                    {
                        EngWorkbenchInterface workbench = new EngWorkbenchInterface();
                        // Cycle through each selected MOM
                        foreach (ListViewItem item in lb_MOMs.Items)
                        {
                            string part = item.SubItems[0].Text;
                            string revnum = item.SubItems[1].Text;


                            /*// Get current revision num
                            SqlCommand command = connection.CreateCommand();
                            command.CommandText = "exec [dbo].sp_GetCurrentPartRevision @Company, @Partnum";
                            command.Parameters.AddWithValue("Company", Company);
                            command.Parameters.AddWithValue("Partnum", part);

                            SqlDataAdapter sda = new SqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            sda.Fill(ds);

                            string revnum = ds.Tables[0].Rows[0][0].ToString();*/

                            workbench.AddMaterial(VantageServer, database, VantageUser, VantagePass, part, revnum, txt_Add_Partnum.Text, ddl_Add_Operations.SelectedValue.ToString(), txt_Add_Qty.Text);
                        }

                        MessageBox.Show("Material Added");
                        txt_Add_Partnum.Text = "";
                        txt_Add_Qty.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                EnableBoxes(true);
                PopulateMaterials();
            }
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            try
            {
                // Validate selected part exists on all selected MOMs
                bool valid = true;
                foreach (ListViewItem item in lb_MOMs.Items)
                {
                    string partnum = item.SubItems[0].Text;
                    string revnum = item.SubItems[1].Text;

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "exec [dbo].sp_GetPartMaterials @Company, @Partnum";
                    command.Parameters.AddWithValue("Company", Company);
                    command.Parameters.AddWithValue("Partnum", partnum);
                    command.Parameters.AddWithValue("RevisionNum", revnum);

                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["mtlseq"].ToString() == ((DataRowView)ddl_Remove_Part.SelectedItem)["mtlseq"].ToString() && row["mtlpartnum"].ToString() == ((DataRowView)ddl_Remove_Part.SelectedItem)["mtlpartnum"].ToString())
                            found = true;
                    }
               /*     if (!found)
                    {
                        MessageBox.Show("Validation Failed.  Mtl Part #" + ((DataRowView)ddl_Remove_Part.SelectedItem)["mtlpartnum"].ToString() + "' not found on Part #" + str + ".  All selected MOMs must have the chosen material.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        valid = false;
                    }*/
                }

                // Part existance validation passed
                // Remove material from each mom
                if (valid)
                {
                    EngWorkbenchInterface workbench = new EngWorkbenchInterface();
                    // Cycle through each selected MOM
                    foreach (ListViewItem item in lb_MOMs.Items)
                    {
                        string part = item.SubItems[0].Text;
                        string revnum = item.SubItems[1].Text;
                        /*
                        // Get current revision num
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "exec [dbo].sp_GetCurrentPartRevision @Company, @Partnum";
                        command.Parameters.AddWithValue("Company", Company);
                        command.Parameters.AddWithValue("Partnum", part);

                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        string revnum = ds.Tables[0].Rows[0][0].ToString();*/

                        workbench.RemoveMaterial(VantageServer, database, VantageUser, VantagePass, part, revnum, ((DataRowView)ddl_Remove_Part.SelectedItem)["mtlseq"].ToString(), ((DataRowView)ddl_Remove_Part.SelectedItem)["mtlpartnum"].ToString());
                    }

                    MessageBox.Show("Material Removed");
                    txt_Add_Partnum.Text = "";
                    txt_Add_Qty.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                EnableBoxes(true);
                PopulateMaterials();
            }

        }

        private void btn_RemoveOpr_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            try
            {
                // Validate selected part exists on all selected MOMs
                bool valid = true;
                foreach (ListViewItem item in lb_MOMs.Items)
                {
                    string partnum = item.SubItems[0].Text;
                    string revnum = item.SubItems[1].Text;

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "exec [dbo].sp_GetPartOperations @Company, @Partnum, @RevisionNum";
                    command.Parameters.AddWithValue("Company", Company);
                    command.Parameters.AddWithValue("Partnum", partnum);
                    command.Parameters.AddWithValue("RevisionNum", revnum);

                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["oprseq"].ToString() == ((DataRowView)ddl_Remove_Operations.SelectedItem)["oprseq"].ToString() && row["opcode"].ToString() == ((DataRowView)ddl_Remove_Operations.SelectedItem)["opcode"].ToString())
                            found = true;
                    }
                    if (!found)
                    {
                        MessageBox.Show("Validation Failed.  Operation #" + ((DataRowView)ddl_Remove_Operations.SelectedItem)["oprseq"].ToString() + "' not found on Part #" + partnum + ".  All selected MOMs must have the chosen operation.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        valid = false;
                    }
                }

                // Part existance validation passed
                // Remove material from each mom
                if (valid)
                {
                    EngWorkbenchInterface workbench = new EngWorkbenchInterface();
                    // Cycle through each selected MOM
                    foreach (ListViewItem item in lb_MOMs.Items)
                    {
                        string part = item.SubItems[0].Text;
                        string revnum = item.SubItems[1].Text;

                        /*// Get current revision num
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "exec [dbo].sp_GetCurrentPartRevision @Company, @Partnum";
                        command.Parameters.AddWithValue("Company", Company);
                        command.Parameters.AddWithValue("Partnum", part);

                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        string revnum = ds.Tables[0].Rows[0][0].ToString();*/

                        workbench.RemoveOperation(VantageServer, database, VantageUser, VantagePass, part, revnum, ((DataRowView)ddl_Remove_Operations.SelectedItem)["oprseq"].ToString());
                    }

                    MessageBox.Show("Operation Removed");
                    txt_Add_Partnum.Text = "";
                    txt_Add_Qty.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                EnableBoxes(true);
                PopulateMaterials();
                PopulateOperations();
            }


        }

        private void btn_Replace_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            try
            {
                // Validate selected part exists on all selected MOMs
                bool valid = true;
                foreach (ListViewItem item in lb_MOMs.Items)
                {
                    string partnum = item.SubItems[0].Text;
                    string revnum = item.SubItems[1].Text;

                    if (!String.IsNullOrEmpty(partnum))
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "exec [dbo].sp_GetPartMaterials @Company, @Partnum";
                        command.Parameters.AddWithValue("Company", Company);
                        command.Parameters.AddWithValue("Partnum", partnum);
                        command.Parameters.AddWithValue("RevisionNum", revnum);

                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        bool found = false;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (/*row["mtlseq"].ToString() == ((DataRowView)ddl_Replace_Part.SelectedItem)["mtlseq"].ToString() &&*/ row["mtlpartnum"].ToString().ToUpper() == ((DataRowView)ddl_Replace_Part.SelectedItem)["mtlpartnum"].ToString().ToUpper())
                                found = true;
                        }
                        if (!found)
                        {
                            MessageBox.Show("Validation Failed.  Mtl Part #" + ((DataRowView)ddl_Replace_Part.SelectedItem)["mtlpartnum"].ToString() + "' not found on Part #" + partnum + ".  All selected MOMs must have the chosen material.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            valid = false;
                        }
                    }
                }

                // Part existance validation passed
                if (valid)
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "exec [dbo].sp_GetPartsExactMatch @Company, @Partnum";
                    command.Parameters.AddWithValue("Company", Company);
                    command.Parameters.AddWithValue("Partnum", txt_Replace_Part.Text);

                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Validation Failed.  Part #" + txt_Replace_Part.Text + " does not exist.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        valid = false;
                    }
                }



                // Replace material from each mom
                if (valid)
                {
                    EngWorkbenchInterface workbench = new EngWorkbenchInterface();
                    // Cycle through each selected MOM
                    foreach (ListViewItem item in lb_MOMs.Items)
                    {
                        string part = item.SubItems[0].Text;
                        string revnum = item.SubItems[1].Text;

                        if (!String.IsNullOrEmpty(part))
                        {
                            // Get current revision num
                            /*SqlCommand command = connection.CreateCommand();
                            command.CommandText = "exec [dbo].sp_GetCurrentPartRevision @Company, @Partnum";
                            command.Parameters.AddWithValue("Company", Company);
                            command.Parameters.AddWithValue("Partnum", part);

                            SqlDataAdapter sda = new SqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            sda.Fill(ds);

                            string revnum = ds.Tables[0].Rows[0][0].ToString();*/
                            workbench.ReplaceMaterial(VantageServer, database, VantageUser, VantagePass, part, revnum, ((DataRowView)ddl_Replace_Part.SelectedItem)["mtlseq"].ToString(), ((DataRowView)ddl_Replace_Part.SelectedItem)["mtlpartnum"].ToString(), txt_Replace_Part.Text, true);
                        }
                    }

                    MessageBox.Show("Material Replaced");
                    txt_Replace_Part.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                EnableBoxes(true);
                PopulateMaterials();
            }

        }

        private void btn_Revise_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            try
            {
                decimal qty;
                
                if (String.IsNullOrEmpty(txt_Revise_Qty.Text) && String.IsNullOrEmpty(ddl_ReviseOpr.SelectedItem.ToString()))
                    MessageBox.Show("Missing revision Qty or Operation");
                else if (!String.IsNullOrEmpty(txt_Revise_Qty.Text) && !Decimal.TryParse(txt_Revise_Qty.Text, out qty))
                    MessageBox.Show("Qty Is Not A Number");
                else
                {
                    // Validate selected part exists on all selected MOMs
                    bool valid = true;
                    foreach (ListViewItem item in lb_MOMs.Items)
                    {
                        string partnum = item.SubItems[0].Text;
                        string revnum = item.SubItems[1].Text;

                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "exec [dbo].sp_GetPartMaterials @Company, @Partnum";
                        command.Parameters.AddWithValue("Company", Company);
                        command.Parameters.AddWithValue("Partnum", partnum);
                        command.Parameters.AddWithValue("RevisionNum", revnum);

                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        bool found = false;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (/*row["mtlseq"].ToString() == ((DataRowView)ddl_Revise_Part.SelectedItem)["mtlseq"].ToString() && */row["mtlpartnum"].ToString() == ((DataRowView)ddl_Revise_Part.SelectedItem)["mtlpartnum"].ToString())
                                found = true;
                        }
                        if (!found)
                        {
                            MessageBox.Show("Validation Failed.  Mtl Part #" + ((DataRowView)ddl_Revise_Part.SelectedItem)["mtlpartnum"].ToString() + "' not found on Part #" + partnum + ".  All selected MOMs must have the chosen material.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            valid = false;
                        }
                    }

                    // Part existance validation passed
                    // Remove material from each mom
                    if (valid)
                    {
                        EngWorkbenchInterface workbench = new EngWorkbenchInterface();
                        // Cycle through each selected MOM
                        foreach (ListViewItem item in lb_MOMs.Items)
                        {
                            string part = item.SubItems[0].Text;
                            string revnum = item.SubItems[1].Text;
                            /*
                            // Get current revision num
                            SqlCommand command = connection.CreateCommand();
                            command.CommandText = "exec [dbo].sp_GetCurrentPartRevision @Company, @Partnum";
                            command.Parameters.AddWithValue("Company", Company);
                            command.Parameters.AddWithValue("Partnum", part);

                            SqlDataAdapter sda = new SqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            sda.Fill(ds);

                            string revnum = ds.Tables[0].Rows[0][0].ToString();*/

                            workbench.ReviseMaterial(VantageServer, database, VantageUser, VantagePass, part, revnum, ((DataRowView)ddl_Revise_Part.SelectedItem)["mtlseq"].ToString(), ((DataRowView)ddl_Revise_Part.SelectedItem)["mtlpartnum"].ToString(), txt_Revise_Qty.Text, ((DataRowView)ddl_ReviseOpr.SelectedItem)["oprseq"].ToString(), true);
                        }

                        MessageBox.Show("Material Revised");
                        txt_Revise_Qty.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                EnableBoxes(true);
                PopulateMaterials();
                PopulateOperations();
            }
        }

        private void but_Routine_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            if (cb_Routines.SelectedItem.ToString() == "Modline Frame Rename Add S Bracket Identifier")
            {
                ModlineFrameRename();
            }
            if (cb_Routines.SelectedItem.ToString() == "Modline Door New Handles")
            {
                ModlineDoorNewHandles();
            }
            EnableBoxes(true);
        }

        private void but_Duplicate_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            if (lb_MOMs.Items.Count != 1)
            {
                MessageBox.Show("Duplicate only works with exactly 1 MOM selected");
            }
            else if (String.IsNullOrEmpty(txt_DuplicateName.Text))
            {
                MessageBox.Show("No copy name provided");
            }
            else
            {
                PartInterface pi = new PartInterface();
                pi.DuplicatePart(VantageServer, database, VantageUser, VantagePass, ((ListViewItem)lb_MOMs.Items[0]).SubItems[0].Text, txt_DuplicateName.Text);
                pi.CopyAllMES(server, database, Company, ((ListViewItem)lb_MOMs.Items[0]).SubItems[0].Text, txt_DuplicateName.Text);
                MessageBox.Show("Part #" + ((ListViewItem)lb_MOMs.Items[0]).SubItems[0].Text + " and all MESs duplicated to " + txt_DuplicateName.Text);
                txt_DuplicateName.Text = "";
            }
            EnableBoxes(true);
        }

        #endregion

        #region Methods

        private void EnableBoxes(bool enabled)
        {
            gb_Add.Enabled = enabled;
            gb_Remove.Enabled = enabled;
            gb_Replace.Enabled = enabled;
            gb_Revise.Enabled = enabled;
            gb_RemoveOpr.Enabled = enabled;
            gb_Duplicate.Enabled = enabled;
            gb_Routine.Enabled = enabled;
            gb_MergeOpr.Enabled = enabled;
            gb_AddOpr.Enabled = enabled;
        }

        private void PopulateOperations()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "exec [dbo].sp_GetPartOperations @Company, @Partnum, @RevisionNum";
            command.Parameters.AddWithValue("Company", Company);
            command.Parameters.AddWithValue("Partnum", ((ListViewItem)lb_MOMs.Items[0]).SubItems[0].Text);
            command.Parameters.AddWithValue("RevisionNum", ((ListViewItem)lb_MOMs.Items[0]).SubItems[1].Text);

            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            ddl_Add_Operations.DataSource = ds.Tables[0];
            ddl_Add_Operations.ValueMember = "oprseq";
            ddl_Add_Operations.DisplayMember = "opcode";
            if (ddl_Add_Operations.Items.Count > 0)
                ddl_Add_Operations.SelectedItem = ddl_Add_Operations.Items[0];

            ddl_ReviseOpr.BindingContext = new BindingContext();
            ddl_ReviseOpr.DataSource = ds.Tables[0];
            ddl_ReviseOpr.ValueMember = "oprseq";
            ddl_ReviseOpr.DisplayMember = "opcode";
            ddl_ReviseOpr.SelectedIndex = -1;


            ddl_Remove_Operations.BindingContext = new BindingContext();
            ddl_Remove_Operations.DataSource = ds.Tables[0];
            ddl_Remove_Operations.ValueMember = "oprseq";
            ddl_Remove_Operations.DisplayMember = "opcode";
            if (ddl_Remove_Operations.Items.Count > 0)
                ddl_Remove_Operations.SelectedItem = ddl_Remove_Operations.Items[0];

            ddl_MergeFromOpr.BindingContext = new BindingContext();
            ddl_MergeFromOpr.DataSource = ds.Tables[0];
            ddl_MergeFromOpr.ValueMember = "oprseq";
            ddl_MergeFromOpr.DisplayMember = "opcode";
            if (ddl_MergeFromOpr.Items.Count > 0)
                ddl_MergeFromOpr.SelectedItem = ddl_MergeFromOpr.Items[0];

            ddl_MergeToOpr.BindingContext = new BindingContext();
            ddl_MergeToOpr.DataSource = ds.Tables[0];
            ddl_MergeToOpr.ValueMember = "oprseq";
            ddl_MergeToOpr.DisplayMember = "opcode";
            if (ddl_MergeToOpr.Items.Count > 0)
                ddl_MergeToOpr.SelectedItem = ddl_MergeToOpr.Items[0];

            ddl_AddOperation.Items.Clear();
            PartInterface pi = new PartInterface();
            foreach (Operation opr in pi.GetAllOperations(server, database, username, password, Company))
            {
                bool found = false;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["opcode"].ToString().ToLower() == opr.OpCode.ToLower())
                        found = true;
                }
                if (!found)
                    ddl_AddOperation.Items.Add(opr.OpCode);
            }
        }

        private void PopulateMaterials()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "exec [dbo].sp_GetPartMaterials @Company, @Partnum, @RevisionNum";
            command.Parameters.AddWithValue("Company", Company);
            command.Parameters.AddWithValue("Partnum", ((ListViewItem)lb_MOMs.Items[0]).SubItems[0].Text);
            command.Parameters.AddWithValue("RevisionNum", ((ListViewItem)lb_MOMs.Items[0]).SubItems[1].Text);

            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            ddl_Remove_Part.DataSource = ds.Tables[0];
            ddl_Remove_Part.ValueMember = "mtlseq";
            ddl_Remove_Part.DisplayMember = "mtlpartnum";
            ddl_Remove_Part.SelectedItem = ddl_Remove_Part.Items[0];

            ddl_Replace_Part.BindingContext = new BindingContext();
            ddl_Replace_Part.DataSource = ds.Tables[0];
            ddl_Replace_Part.ValueMember = "mtlseq";
            ddl_Replace_Part.DisplayMember = "mtlpartnum";
            ddl_Replace_Part.SelectedItem = ddl_Replace_Part.Items[0];

            ddl_Revise_Part.BindingContext = new BindingContext();
            ddl_Revise_Part.DataSource = ds.Tables[0];
            ddl_Revise_Part.ValueMember = "mtlseq";
            ddl_Revise_Part.DisplayMember = "mtlpartnum";
            ddl_Revise_Part.SelectedItem = ddl_Revise_Part.Items[0];
        }

        private void ModlineDoorNewHandles()
        {
            bool valid = true;
            foreach (string part in lb_MOMs.Items)
            {
                // Validate that the current 11th character is "-"
                if (part[0] != '0' && part[1] != '0' && part[4] != 'M' && part[10] != '1')
                {
                    valid = false;
                    MessageBox.Show("Part # " + part + " does not match naming convention for modline doors");
                }
            }
            if (valid)
            {
                int counter = 0;
                EngWorkbenchInterface workbench = new EngWorkbenchInterface();
                foreach (string part in lb_MOMs.Items)
                {
                    try
                    {
                        StringBuilder newpart = new StringBuilder(part);
                        newpart[10] = '3';
/*                        PartInterface pi = new PartInterface();
                        pi.DuplicatePart(Server, "8301", VantageUser, VantagePass, part, newpart.ToString());
 */
                        // Get current revision num
/*                                            SqlCommand command = connection.CreateCommand();
                                            command.CommandText = "exec [dbo].sp_GetCurrentPartRevision @Company, @Partnum";
                                            command.Parameters.AddWithValue("Company", Company);
                                            command.Parameters.AddWithValue("Partnum", newpart.ToString());

                                            SqlDataAdapter sda = new SqlDataAdapter(command);
                                            DataSet ds = new DataSet();
                                            sda.Fill(ds);

                                            string revnum = ds.Tables[0].Rows[0][0].ToString();
                                            if (newpart.ToString().Substring(6, 2) == "SB")
                                                workbench.ReplaceMaterial(Server, "8301", VantageUser, VantagePass, newpart.ToString(), revnum, "", "7406B", "7923B", true);
                                            else if (newpart.ToString().Substring(6, 2) == "BS")
                                                workbench.ReplaceMaterial(Server, "8301", VantageUser, VantagePass, newpart.ToString(), revnum, "", "7406S", "7923S", true);
                                            else
                                                MessageBox.Show(newpart.ToString() + ", no handle found");*/
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    counter++;
                    lbl_Status.Text = "Processed part " + counter.ToString() + " of " + lb_MOMs.Items.Count.ToString();

                }
            }
        }

        private void ModlineFrameRename()
        {
            bool valid = true;
            foreach (string part in lb_MOMs.Items)
            {
                // Validate that the current 11th character is "-"
                if (part[10] != '-')
                {
                    valid = false;
                    MessageBox.Show("Part # " + part + " does not match naming convention of '-' as 11th character");
                }
            }
            if (valid)
            {
                int counter = 0;
                foreach (string part in lb_MOMs.Items)
                {
                    string newpart = part.Insert(10, "S");
                    PartInterface pi = new PartInterface();
                    pi.DuplicatePart(server, database, VantageUser, VantagePass, part, newpart);
                    pi.DeactivatePart(server, database, VantageUser, VantagePass, part);
                    pi.CopyAllMES(server, database, Company, part, newpart);
                    counter++;
                    lbl_Status.Text = "Processed part " + counter.ToString() + " of " + lb_MOMs.Items.Count.ToString();
                }
            }
        }

        #endregion

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string t = Clipboard.GetText();
                string[] split = t.Split(new char[] { '\n' });
                foreach (string s in split)
                {
                    lb_MOMs.Items.Add(s.Trim());
                }
                gb_Add.Enabled = true;
                gb_Remove.Enabled = true;
                gb_Replace.Enabled = true;
                gb_Revise.Enabled = true;
                gb_RemoveOpr.Enabled = true;
                PopulateOperations();
                PopulateMaterials();
            }
            catch (Exception ex)
            {
                string b = ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            try
            {
                // Validate selected part exists on all selected MOMs
                bool valid = true;
                if (((DataRowView)ddl_MergeFromOpr.SelectedItem)["opcode"].ToString() == ((DataRowView)ddl_MergeToOpr.SelectedItem)["opcode"].ToString())
                {
                    MessageBox.Show("Cannot merge an operation to itself.");
                }
                else
                {
                    foreach (ListViewItem item in lb_MOMs.Items)
                    {
                        string partnum = item.SubItems[0].Text;
                        string revnum = item.SubItems[1].Text;

                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = "exec [dbo].sp_GetPartOperations @Company, @Partnum, @RevisionNum";
                        command.Parameters.AddWithValue("Company", Company);
                        command.Parameters.AddWithValue("Partnum", partnum);
                        command.Parameters.AddWithValue("RevisionNum", revnum);

                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        bool found = false;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["oprseq"].ToString() == ((DataRowView)ddl_MergeFromOpr.SelectedItem)["oprseq"].ToString() && row["opcode"].ToString() == ((DataRowView)ddl_MergeFromOpr.SelectedItem)["opcode"].ToString())
                                found = true;
                        }
                        if (!found)
                        {
                            MessageBox.Show("Validation Failed.  Merge From Operation #" + ((DataRowView)ddl_MergeFromOpr.SelectedItem)["oprseq"].ToString() + "' not found on Part #" + partnum + ".  All selected MOMs must have the chosen operation.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            valid = false;
                        }

                        found = false;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["oprseq"].ToString() == ((DataRowView)ddl_MergeToOpr.SelectedItem)["oprseq"].ToString() && row["opcode"].ToString() == ((DataRowView)ddl_MergeToOpr.SelectedItem)["opcode"].ToString())
                                found = true;
                        }
                        if (!found)
                        {
                            MessageBox.Show("Validation Failed.  Merge To Operation #" + ((DataRowView)ddl_MergeToOpr.SelectedItem)["oprseq"].ToString() + "' not found on Part #" + partnum + ".  All selected MOMs must have the chosen operation.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            valid = false;
                        }

                    }

                    // Part existance validation passed
                    // Remove material from each mom
                    if (valid)
                    {
                        EngWorkbenchInterface workbench = new EngWorkbenchInterface();
                        // Cycle through each selected MOM
                        foreach (ListViewItem item in lb_MOMs.Items)
                        {
                            string part = item.SubItems[0].Text;
                            string revnum = item.SubItems[1].Text;

                            /*// Get current revision num
                            SqlCommand command = connection.CreateCommand();
                            command.CommandText = "exec [dbo].sp_GetCurrentPartRevision @Company, @Partnum";
                            command.Parameters.AddWithValue("Company", Company);
                            command.Parameters.AddWithValue("Partnum", part);

                            SqlDataAdapter sda = new SqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            sda.Fill(ds);

                            string revnum = ds.Tables[0].Rows[0][0].ToString();*/

                            workbench.MigrateMaterials(VantageServer, database, VantageUser, VantagePass, part, revnum, ((DataRowView)ddl_MergeFromOpr.SelectedItem)["oprseq"].ToString(), ((DataRowView)ddl_MergeToOpr.SelectedItem)["oprseq"].ToString());
                        }

                        MessageBox.Show("Operation Merged");
                        txt_Add_Partnum.Text = "";
                        txt_Add_Qty.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                EnableBoxes(true);
                PopulateMaterials();
                PopulateOperations();
            }

        }

        private void btn_AddOpr_Click(object sender, EventArgs e)
        {
            EnableBoxes(false);
            try
            {
                EngWorkbenchInterface workbench = new EngWorkbenchInterface();
                // Cycle through each selected MOM
                foreach (ListViewItem item in lb_MOMs.Items)
                {
                    string part = item.SubItems[0].Text;
                    string revnum = item.SubItems[1].Text;

                    workbench.AddOperation(VantageServer, database, VantageUser, VantagePass, part, revnum, ddl_AddOperation.SelectedItem.ToString());
                }

                MessageBox.Show("Operation Added");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                EnableBoxes(true);
                PopulateMaterials();
                PopulateOperations();
            }
        }
    }
}
