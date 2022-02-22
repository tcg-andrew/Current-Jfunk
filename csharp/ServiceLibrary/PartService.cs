#region Usings

using System;
using System.Collections.Generic;
using System.Configuration;
using ObjectLibrary;
using System.ServiceModel;
using System.Security.Permissions;
using System.Threading;

#endregion

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class PartService : IPartService
    {
        #region Service Behaviors

/*        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Part Minimum Qty")]*/
        public partgetallresult getallparts(int limit)
        {
            partgetallresult result = new partgetallresult();

            try
            {
                PartInterface partInterface = new PartInterface();
                foreach (Part part in partInterface.GetAllParts(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), limit))
                    result.epicor.Add(new part(part));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Part Minimum Qty")]
        public operationgetallresult getalloperations()
        {
            operationgetallresult result = new operationgetallresult();

            try
            {
                PartInterface partInterface = new PartInterface();
                foreach (Operation operation in partInterface.GetAllOperations(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString()))
                    result.epicor.Add(new operation(operation));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Part Minimum Qty")]
        public partgetneedingupdateresult getpartsneedingupdate(string plant, string partclass)
        {
            partgetneedingupdateresult result = new partgetneedingupdateresult();

            try
            {
                string company = "";
                if (Thread.CurrentPrincipal.IsInRole("CRD"))
                    company = "CRD";
                else if (Thread.CurrentPrincipal.IsInRole("CIG"))
                    company = "CIG";
                PartInterface partInterface = new PartInterface();
                foreach (Part part in partInterface.GetPartsNeedingQtyUpdate(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company, plant, partclass))
                    result.epicor.Add(new NeedingUpdate.part(part));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Part Minimum Qty")]
        public partgetstringresult getplants()
        {
            partgetstringresult result = new partgetstringresult();

            try
            {
                string company = "";
                if (Thread.CurrentPrincipal.IsInRole("CRD"))
                    company = "CRD";
                else if (Thread.CurrentPrincipal.IsInRole("CIG"))
                    company = "CIG";

                PartInterface partInterface = new PartInterface();
                foreach (string str in partInterface.GetPlants(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company))
                    result.epicor.Add(str);
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Part Minimum Qty")]
        public partgetclassresult getpartclasses()
        {
            partgetclassresult result = new partgetclassresult();

            try
            {
                string company = "";
                if (Thread.CurrentPrincipal.IsInRole("CRD"))
                    company = "CRD";
                else if (Thread.CurrentPrincipal.IsInRole("CIG"))
                    company = "CIG";

                PartInterface partInterface = new PartInterface();
                foreach (PartClass p in partInterface.GetPartClasses(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company))
                    result.epicor.Add(new partclass(p));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Part Minimum Qty")]
        public partupdateresult updatepartsminqty(partsminqtyupdaterequest request)
        {
            partupdateresult result = new partupdateresult();

            try
            {
                PartInterface partInterface = new PartInterface();
                foreach (NeedingUpdate.part p in request.epicor)
                    partInterface.UpdatePartMinimumValue(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), request.username, request.password, new Part(p.partnum, p.desc, "", false, "", "", "", 0, p.plant, p.monthlyusage, p.minimumqty, p.percentdiff, 0, 0, 0));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public partgetmiscchargeclassresult getmiscchargeclasses()
        {
            partgetmiscchargeclassresult result = new partgetmiscchargeclassresult();

            try
            {
                PartInterface partInterface = new PartInterface();
                foreach (MiscCharge charge in partInterface.GetMiscChargeClasses(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString()))
                    result.epicor.Add(new misccharge(charge));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public partgetpricelistresult getpartpricelist()
        {
            partgetpricelistresult result = new partgetpricelistresult();

            try
            {
                PartInterface partInterface = new PartInterface();
                foreach (PartPriceList p in partInterface.GetPartPriceList(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), "CRD"))
                    result.epicor.Add(new partpricelist(p));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;

        }

        #endregion


    }
}
