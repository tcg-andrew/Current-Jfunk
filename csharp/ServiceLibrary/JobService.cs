using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Permissions;
using ObjectLibrary;
using System.Configuration;

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace = "http://services.it.tcg/epicor/jobservice")]
    public class JobService : IJobService
    {
        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Job Mismatched Dates")]
        public jobgetmismatchresult GetJobsWithMismatchedDates()
        {
            jobgetmismatchresult result = new jobgetmismatchresult();

            try
            {
                JobInterface jobInterface = new JobInterface();
                foreach (Job job in jobInterface.GetJobsWithMismatchedDates(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString()))
                    result.epicor.Add(job);
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;

        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Vantage Service Job Mismatched Dates")]
        public jobgetstringresult UpdateJobDates(string username, string password, List<Job> jobs)
        {
            jobgetstringresult result = new jobgetstringresult();

            try
            {
                foreach (Job job in jobs)
                {
                    JobInterface jobInterface = new JobInterface();
                    jobInterface.UpdateJobDate(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, job);
                }
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }
    }
}
