#region Usings

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ObjectLibrary;

#endregion

namespace ServiceLibrary
{
    #region Service Contract

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/jobservice")]
    public interface IJobService
    {
        [OperationContract]
        jobgetmismatchresult GetJobsWithMismatchedDates();

        [OperationContract]
        jobgetstringresult UpdateJobDates(string username, string password, List<Job> jobs);
    }

    #endregion

    #region Data Contract

    [DataContract(Namespace = "http://services.it.tcg/epicor/jobservice")]
    public class jobgetmismatchresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<Job> epicor { get; set; }

        public jobgetmismatchresult()
        {
            epicor = new List<Job>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/jobservice")]
    public class jobgetstringresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }
    }

    #endregion
}
