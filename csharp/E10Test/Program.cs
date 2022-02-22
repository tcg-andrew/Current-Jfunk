using Erp.BO;
using Erp.Contracts;
using Erp.Proxy.BO;
using Ice.Core;
using Ice.Lib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectLibrary;

namespace E10Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PartInterface partInterface = new PartInterface();
            partInterface.GetAllParts("SARV-SQLPROD01", "EpicorDB", "RailsAppUserP", "wA7tA1FaBS1MpLaU", 0);
        }
    }

    public class CustomerInterface : EpicorExtension<PartImpl, PartSvcContract>
    {
        public void Get()
        {
            OpenSession("SARV-APPEPCRP01", "EpicorDB", "jfunk", "B4r7lB33", Session.LicenseType.Default);
            PartDataSet ds = BusinessObject.GetByID("00D3S5SB-R");

            bool found = false;
            if (ds.Part.Rows.Count == 0)
                throw new Exception("IsShipToValid - No customer found matching Customer ID " + "1001");
            else
            {
                foreach (PartDataSet.PartRow row in ds.Part.Rows)
                {
                    string i = row.PartNum;
                }
            }

        }
    }

        // TODO:5 Add unit tests
        public abstract class EpicorExtension<T, C> where T : Epicor.ServiceModel.Channels.ImplBase
    {
        #region Values

        private Session objSess;
        private T busObj;

        #endregion

        #region Properties

        protected T BusinessObject
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

        protected void OpenSession(string server, string database, string username, string password, Session.LicenseType licenseType)
        {
            string connString = "net.tcp://" + server + "/" + database;

            try
            {
                objSess = new Session(username, password, connString, licenseType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void CloseSession()
        {
            try
            {
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
