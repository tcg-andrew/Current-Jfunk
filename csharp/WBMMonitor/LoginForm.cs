using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WBMMonitor
{
    public partial class LoginForm : Form
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AuthToken { get; set; }
        public string UserID { get; set; }
        public string DepartmentID { get; set; }
        public bool LoggedIn { get; set; }

        public string web_address = "http://10.78.70.92/wbm/";
        //public string web_address = "http://rails2/";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                LoggedIn = true;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(web_address + "users/sign_in.json");
                request.ContentType = "application/json";
                request.ContentLength = 0;
                request.Method = "POST";
                string data = "{ \"user\" : { \"email\" : \"" + txt_Email.Text + "\", \"password\" : \"" + txt_Password.Text + "\" } }";
                request.ContentLength = sizeof(Byte) * data.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(data);
                writer.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                content = content.Replace("{", "").Replace("}", "").Replace("\"", "");
                foreach (string val in content.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string key = val.Split(new char[] { ':' })[0];
                    string value = val.Split(new char[] { ':' })[1];
                    switch (key)
                    {
                        case "success":
                            if (value.ToUpper() != "TRUE")
                                LoggedIn = false;
                            break;
                        case "token":
                            AuthToken = value;
                            break;
                        case "first_name":
                            FirstName = value;
                            break;
                        case "last_name":
                            LastName = value;
                            break;
                        case "department_id":
                            DepartmentID = value;
                            break;
                        case "user_id":
                            UserID = value;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggedIn = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
