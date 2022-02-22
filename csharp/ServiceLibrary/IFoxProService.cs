using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Data;

namespace ServiceLibrary
{
    #region Service Contracts

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/foxproservice")]
    public interface IFoxProService
    {
        [OperationContract]
        datasetresult GetCIGUnitList();

        [OperationContract]
        datasetresult GetCIGUnitInfo(string unitnum);

        [OperationContract]
        int InsertCIGPAReading(string unitnum, string watt, string pf, string volts, string amps, string hertz, string heater);
    }

    #endregion

    #region Data Contracts

    [DataContract()]
    public class datasetresult
    {
        [DataMember]
        public DataSet ds { get; set; }
    }

    #endregion
}
