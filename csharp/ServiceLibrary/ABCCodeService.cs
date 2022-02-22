#region Usings

using System;
using System.Collections.Generic;
using System.Configuration;
using ObjectLibrary;
using System.ServiceModel;
using System.Security.Permissions;

#endregion

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace="http://services.it.tcg/epicor/abccodeservice")]
    public class ABCCodeService : IABCCodeService
    {
        #region Service Behaviors

        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service ABC Code")]
        public abccodegetresult getallabccodes()
        {
            abccodegetresult result = new abccodegetresult();

            try
            {
                ABCCodeInterface abcCodeInterface = new ABCCodeInterface();
                foreach (ABCCode code in abcCodeInterface.GetCodes(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString()))
                    result.epicor.Add(new abccode(code));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }
           
            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service ABC Code")]
        public abccodeupdateresult updateabccodes(abccodeupdaterequest data)
        {
            abccodeupdateresult result = new abccodeupdateresult();

            try
            {
                ABCCodeInterface abcCodeInterface = new ABCCodeInterface();
                foreach (abccode code in data.epicor)
                    abcCodeInterface.UpdateCode(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), data.username, data.password, new ABCCode(code.code, code.minvol, code.mincost, code.freq));
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
