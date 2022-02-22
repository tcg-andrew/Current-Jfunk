#region Usings

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ObjectLibrary;

#endregion

namespace ServiceLibrary
{
    #region Service Contracts

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/freightrateservice")]
    public interface IFreightRateService
    {
        [OperationContract]
        freightrategetresult getrate(string classcode1, string weight1, string classcode2, string weight2, string classcode3, string weight3, string classcode4, string weight4, string classcode5, string weight5, string fromzip, string tozip, int location);
    }

    #endregion

    #region Data Contracts

    [DataContract(Namespace="http://services.it.tcg/epicor/freightrateservice")]
    public class freightrategetresult
    {
        [DataMember(Order = 0)]
        public string exception;

        [DataMember(Order = 1)]
        public List<freightrate> epicor;

        public freightrategetresult()
        {
            epicor = new List<freightrate>();
        }
    }

    #endregion

    #region Data Forms

    public class freightrate
    {
        #region Properties

        public string carrier { get; set; }
        public decimal totalcost { get; set; }
        public string jointline { get; set; }
        public int transitdays { get; set; }

        #endregion

        #region Constructors

        public freightrate()
        {
            carrier = "";
            totalcost = 0;
            jointline = "";
            transitdays = 0;
        }

        public freightrate(FreightRate rate)
        {
            carrier = rate.Carrier;
            totalcost = rate.TotalCost;
            jointline = rate.JointLine;
            transitdays = rate.TransitDays;
        }

        #endregion
    }

    #endregion
}
