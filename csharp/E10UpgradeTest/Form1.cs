using Ice.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E10UpgradeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string server = ConfigurationManager.AppSettings["EpicorServer"].ToString();
            string database = ConfigurationManager.AppSettings["EpicorDatabase"].ToString();
            string username = "CRDService";
            string password = "gfd723trajsdc97";

            string connString = "net.tcp://" + server + "/" + database;
            //            string configpath = @"C:\Epicor\ERP10\LocalClients\EpicorDB_D\config\default.sysConfig";
            string configpath = @"\\10.78.70.224\Epicor_Apps\defaultUpgrade.sysConfig";
            //if (username == "CRDService" || username == "CIGFLService" || username == "CIGTNService")
            //  configpath = @"D:\Installpoint\Epicor\default.sysConfig";

            try
            {
                Session objSess = new Session(username, password, connString, Session.LicenseType.Default, configpath);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
        }
    }
}
