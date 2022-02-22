#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ObjectLibrary;
using System.Configuration;

#endregion

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public class ProductionService : IProductionService
    {
        #region Service Behaviors

        public onedaydetailgetresult getonedaydetail(string company, string lookupdate, string prior)
        {
            onedaydetailgetresult result = new onedaydetailgetresult();

            try
            {
                ProductionInterface productionInterface = new ProductionInterface();
                foreach (OneDayDetail odd in productionInterface.GetOneDayDetail(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company, lookupdate, prior))
                    result.epicor.Add(new onedaydetail(odd));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public sobasedschedulegetresult getsobasedschedule(string company)
        {
            sobasedschedulegetresult result = new sobasedschedulegetresult();

            try
            {
                ProductionInterface productionInterface = new ProductionInterface();
                foreach (SOBasedSchedule odd in productionInterface.GetSOBasedSchedule(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company))
                    result.epicor.Add(new sobasedschedule(odd));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public activelaborgetresult getactivelabor(string company)
        {
            activelaborgetresult result = new activelaborgetresult();

            try
            {
                ProductionInterface productionInterface = new ProductionInterface();
                foreach (ActiveLabor odd in productionInterface.GetActiveLabor(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company))
                    result.epicor.Add(new activelabor(odd));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public indirectactivitygetresult getindirectactivity(string company)
        {
            indirectactivitygetresult result = new indirectactivitygetresult();

            try
            {
                ProductionInterface productionInterface = new ProductionInterface();
                foreach (IndirectActivity odd in productionInterface.GetIndirectActivity(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company))
                    result.epicor.Add(new indirectactivity(odd));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public mesinfogetresult getmesinfo(string company, string jobnum)
        {
            mesinfogetresult result = new mesinfogetresult();

            try
            {
                ProductionInterface productionInterface = new ProductionInterface();
                foreach (MESInfo odd in productionInterface.GetMESInfoForJob(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company, jobnum))
                    result.epicor.Add(new mesinfo(odd));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public mesdrawinggetresult getmesdrawing(string company, string mesnum)
        {
            mesdrawinggetresult result = new mesdrawinggetresult();

            try
            {
                ProductionInterface productionInterface = new ProductionInterface();
                foreach (MESDrawing odd in productionInterface.GetMESDrawings(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company, mesnum))
                    result.epicor.Add(new mesdrawing(odd));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public padatagetresult getpadata(string company, string jobnum, int assemblyseq)
        {
            padatagetresult result = new padatagetresult();

            try
            {
                ProductionInterface productionInterface = new ProductionInterface();
                result.epicor.Add(new padata(productionInterface.GetPADataForJob(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company, jobnum, assemblyseq)));
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
