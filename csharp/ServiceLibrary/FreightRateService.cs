#region Usings

using System;
using System.ServiceModel;
using ObjectLibrary;

#endregion

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace="http://services.it.tcg/epicor/freightrateservice")]
    public class FreightRateService : IFreightRateService
    {
        #region Service Behaviors

        public freightrategetresult getrate(string classcode1, string weight1, string classcode2, string weight2, string classcode3, string weight3, string classcode4, string weight4, string classcode5, string weight5, string fromzip, string tozip, int location)
        {
            freightrategetresult result = new freightrategetresult();
            try
            {
                FreightRateInterface freightRateInterface = new FreightRateInterface();
                result.epicor.Add(new freightrate(freightRateInterface.GetRate("CRDC", "T4H58TK7D6W3", classcode1, weight1, classcode2, weight2, classcode3, weight3, classcode4, weight4, classcode5, weight5, fromzip, tozip, DateTime.Now, location)));
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
