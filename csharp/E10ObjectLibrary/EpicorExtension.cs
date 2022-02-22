#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ice.Core;
using System.Reflection;
using Ice.Lib.Framework;

#endregion

namespace ObjectLibrary
{
    // TODO:5 Add unit tests
    public abstract class EpicorExtension<T, C> where T : Epicor.ServiceModel.Channels.ImplBase
    {
        #region Values

        protected Session objSess;
        private T busObj;

        #endregion

        #region Properties

        public T BusinessObject
        {
            get
            {
                if (busObj == null)
                {
                    try
                    {
                        busObj = WCFServiceSupport.CreateImpl<T>(objSess, Epicor.ServiceModel.Channels.ImplBase.GetUriPath(typeof(C)));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                return busObj;
            }
        }

        #endregion

        #region Constructors

        // Do this so EpicorExtension base class can't be inherited from outside this assembly and potention use an unsafe type, since I'm using a work around in the GetBusinessObject method to make that part work
        internal EpicorExtension()
        {
        }

        #endregion

        #region Protected Methods

        #region Session Management

        public void OpenSession(string server, string database, string username, string password, Session.LicenseType licenseType)
        {
            string connString = "net.tcp://" + server + "/" + database;
                        //string configpath = @"C:\Epicor\ERP10.1Client\Client\config\default.sysConfig";
            string configpath = @"\\10.77.146.183\Epicor_Apps\defaultUpgrade.sysConfig";
            //if (username == "CRDService" || username == "CIGFLService" || username == "CIGTNService")
            //  configpath = @"D:\Installpoint\Epicor\default.sysConfig";

            try
            {
                objSess = new Session(username, password, connString, licenseType, configpath);
            }
            catch (Exception ex)
            {
                throw new Exception("OpenSession - Check AppServer - username = " + username + ", password = REDACTED, connString = " + connString + ", licenseType = " + licenseType + ", config loc = " + configpath + ". " + ex.Message);
            }
        }

        protected void CloseSession()
        {
            try
            {
                if (objSess != null)
                {
                    objSess.Dispose();
                    busObj = default(T);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CloseSession - Check AppServer - " + ex.Message);
            }
        }

        #endregion

        #endregion
    }
}
