#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

#endregion

namespace ServiceLibrary
{
    #region ServiceContract

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public interface ILaborService
    {
        [OperationContract]
        startactivityresult startactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq);

        [OperationContract]
        endactivityresult endactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq, string qty, string scrap);

        [OperationContract]
        startindirectresult startindirect(string company, string employeeid, string indirectcode, string resourcegroup);

        [OperationContract]
        endindirectresult endindirect(string company, string employeeid);

        [OperationContract]
        clockinresult clockin(string company, string employeeid, string shift);

        [OperationContract]
        clockoutresult clockout(string company, string employeeid);

        [OperationContract]
        getindirectcodesresult getindirectcodes(string company);

        [OperationContract]
        getresourcecodesresult getresourcecodes(string company);

        [OperationContract]
        getstringresult getvacationhours(string company, string id);

        [OperationContract]
        getstringresult getempname(string company, string id);

        [OperationContract]
        smartactivityresult smartactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq, string qty, string scrap);
    }

    #endregion

    #region DataContract

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class getstringresult
    {
        [DataMember]
        public string exception { get; set; }

        [DataMember]
        public string epicor { get; set; }

        public getstringresult()
        {
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class smartactivityresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class startactivityresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class endactivityresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class startindirectresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class endindirectresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class clockinresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class clockoutresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class getindirectcodesresult
    {
        [DataMember]
        public Dictionary<string, string> epicor { get; set; }

        [DataMember]
        public string exception { get; set; }

        public getindirectcodesresult()
        {
            epicor = new Dictionary<string, string>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class getresourcecodesresult
    {
        [DataMember]
        public Dictionary<string, string> epicor { get; set; }

        [DataMember]
        public string exception { get; set; }

        public getresourcecodesresult()
        {
            epicor = new Dictionary<string, string>();
        }
    }
    #endregion
}
