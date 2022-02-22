#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;

#endregion

namespace TCGEpicor
{
    public partial class ServiceConnection : Form
    {
        #region Properties

        public string Username
        {
            get
            {
                return txtUser.Text;
            }
            set
            {
                txtUser.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return txtPass.Text;
            }
        }

        #endregion

        #region Constructor

        public ServiceConnection()
        {
            InitializeComponent();
            this.Shown += new EventHandler(ServiceConnection_Shown);
        }

        #endregion

        #region Event Handlers

        private void ServiceConnection_Shown(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;

            txtPass.Text = Password;
            txtUser.Text = Username;

            if (String.IsNullOrEmpty(txtUser.Text))
                txtUser.Focus();
            else if (string.IsNullOrEmpty(txtPass.Text))
                txtPass.Focus();
            else
                txtUser.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                this.Cursor = Cursors.WaitCursor;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        #endregion

        #region Private Methods

        private bool ValidateInput()
        {
            lbl_UsernameError.Visible = false;
            lbl_PasswordError.Visible = false;
            lbl_ErrorSummary.Text = "";

            bool valid = true;

            if (String.IsNullOrEmpty(txtUser.Text))
            {
                lbl_UsernameError.Visible = true;

                if (!String.IsNullOrEmpty(lbl_ErrorSummary.Text))
                    lbl_ErrorSummary.Text += Environment.NewLine;

                lbl_ErrorSummary.Text += "Username is required";

                valid = false;
            }

            if (String.IsNullOrEmpty(txtPass.Text))
            {
                lbl_PasswordError.Visible = true;

                if (!String.IsNullOrEmpty(lbl_ErrorSummary.Text))
                    lbl_ErrorSummary.Text += Environment.NewLine;

                lbl_ErrorSummary.Text += "Password is required";

                valid = false;
            }

            if (!valid)
                lbl_ErrorSummary.Visible = true;

            return valid;
        }

        #endregion
    }
}