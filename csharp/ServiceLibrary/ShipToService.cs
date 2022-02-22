#region Usings

using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using ObjectLibrary;
using System;

#endregion

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace="http://services.it.tcg/epicor/shiptoservice")]
    public class ShipToService : IShipToService
    {
        #region Service Behaviors

        public shiptogetresult getshiptobycustid(string custid)
        {
            shiptogetresult result = new shiptogetresult();

            try
            {
                ShipToInterface shipToInterface = new ShipToInterface();
                foreach (ShipToAddress address in shipToInterface.GetShipToByCustID(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), custid))
                    result.epicor.Add(new shipto(address));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public salesrepgetresult getsalesreplist(string company)
        {
            salesrepgetresult result = new salesrepgetresult();

            try
            {
                ShipToInterface shipToInterface = new ShipToInterface();
                foreach (SalesRep sr in shipToInterface.GetSalesRepList(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company))
                    result.epicor.Add(new salesrep(sr));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }
/*        public shiptogetresult getsingleshipto(string shiptonum, int custnum)
        {
            shiptogetresult result = new shiptogetresult();

            try
            {
                result.epicor.Add(new shipto(ShipToInterface.GetSingleShipTo(ConfigurationManager.AppSettings["Server"].ToString(), ConfigurationManager.AppSettings["Database"].ToString(), ConfigurationManager.AppSettings["Username"].ToString(), ConfigurationManager.AppSettings["Password"].ToString(), shiptonum, custnum)));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }
        */
        #endregion
    }
}
