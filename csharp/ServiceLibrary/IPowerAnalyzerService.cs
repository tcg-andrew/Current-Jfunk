using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ObjectLibrary;

namespace ServiceLibrary
{
    [ServiceContract(Namespace = "http://services.it.tcg/epicor/poweranalyzerservice")]
    public interface IPowerAnalyzerService
    {
        [OperationContract]
        powertableupdateresult UpdatePowerTable(List<PowerAnalyzerSetting> settings);
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/poweranalyzerservice")]
    public class powertableupdateresult
    {
        [DataMember]
        public string exception { get; set; }
    }
}
