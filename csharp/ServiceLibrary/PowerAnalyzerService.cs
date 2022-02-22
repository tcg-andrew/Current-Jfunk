using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ObjectLibrary;
using System.Configuration;
using System.Security.Permissions;
using System.Threading;

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace = "http://services.it.tcg/epicor/poweranalyzerservice")]
    public class PowerAnalyzerService : IPowerAnalyzerService
    {
//        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Power Analyzer")]
        public powertableupdateresult UpdatePowerTable(List<ObjectLibrary.PowerAnalyzerSetting> settings)
        {
            powertableupdateresult result = new powertableupdateresult();

            try
            {
                string revisionComment = Thread.CurrentPrincipal.Identity.Name + " updated at " + DateTime.Now.ToString();
                PowerAnalyzerInterface powerAnalyzerInterface = new PowerAnalyzerInterface();
                powerAnalyzerInterface.CreatePowerTableRevision(ConfigurationManager.AppSettings["PowerAnalyzerServer"].ToString(), ConfigurationManager.AppSettings["PowerAnalyzerDatabase"].ToString(), ConfigurationManager.AppSettings["PowerAnalyzerUsername"].ToString(), ConfigurationManager.AppSettings["PowerAnalyzerPassword"].ToString(), settings, revisionComment);
            }
            catch (Exception ex)
            {
                result.exception = "Server: " + ex.Message;
            }

            return result;
        }
    }
}
