#region Usings

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ObjectLibrary;

#endregion

namespace ServiceLibrary
{
    #region ServiceContract

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/abccodeservice")]
    public interface IABCCodeService
    {
        [OperationContract]
        abccodegetresult getallabccodes();

        [OperationContract]
        abccodeupdateresult updateabccodes(abccodeupdaterequest data);
    }

    #endregion

    #region DataContract

    [DataContract(Namespace = "http://services.it.tcg/epicor/abccodeservice")]
    public class abccodegetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<abccode> epicor { get; set; }

        public abccodegetresult()
        {
            epicor = new List<abccode>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/abccodeservice")]
    public class abccodeupdaterequest
    {
        [DataMember(Order = 0)]
        public string username { get; set; }

        [DataMember(Order = 1)]
        public string password { get; set; }

        [DataMember(Order = 2)]
        public List<abccode> epicor { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/abccodeservice")]
    public class abccodeupdateresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    #endregion

    #region Data Formats

    public class abccode
    {
        #region Properties

        public string code { get; set; }
        public decimal minvol { get; set; }
        public decimal mincost { get; set; }
        public int freq { get; set; }

        #endregion

        #region Constructors

        public abccode()
        {
            code = "";
            minvol = 0;
            mincost = 0;
            freq = 0;
        }

        public abccode(ABCCode fromcode)
        {
            code = fromcode.Code;
            minvol = fromcode.MinVol;
            mincost = fromcode.MinCost;
            freq = fromcode.Frequency;
        }

        #endregion
    }

    #endregion
}
