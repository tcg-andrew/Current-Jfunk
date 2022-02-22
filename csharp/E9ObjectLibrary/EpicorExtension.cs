using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epicor.Mfg.Core;
using System.Reflection;

namespace E9ObjectLibrary
{
    public abstract class EpicorExtension<T>
    {
        #region Values

        private Session objSess;
        private T busObj;

        #endregion

        #region Properties

        protected BLConnectionPool ConnectionPool { get { return objSess.ConnectionPool; } }
        protected T BusinessObject
        {
            get
            {
                if (busObj == null)
                {
                    try
                    {
                        Type classType = typeof(T);
                        ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { objSess.ConnectionPool.GetType() });
                        busObj = (T)classConstructor.Invoke(new object[] { objSess.ConnectionPool });
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

        protected void OpenSession(string server, string port, string username, string password, Session.LicenseType licenseType)
        {
            string connString = "AppServerDC://" + server + ":" + port;

            try
            {
                objSess = new Session(username, password, connString, licenseType);
            }
            catch (Exception ex)
            {
                throw new Exception("OpenSession - Check AppServer - username = " + username + ", password = " + password + ", connString = " + connString + ", licenseType = " + licenseType + ". " + ex.Message);
            }
        }

        protected void CloseSession()
        {
            try
            {
//                objSess.Close();
                objSess.Dispose();
                busObj = default(T);
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
