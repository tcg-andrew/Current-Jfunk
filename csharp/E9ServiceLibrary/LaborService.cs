using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using E9ObjectLibrary;
using System.Configuration;

namespace E9ServiceLibrary
{
    [ServiceBehavior(Namespace = "http://services.it.tcg/epicor/laborservice")]
    public class LaborService : ILaborService
    {
        #region Service Behaviors

        public clockinresult clockin(string company, string employeeid, string shift)
        {
            clockinresult result = new clockinresult();
            try
            {
                string username = company.ToUpper() + "Service";
                LaborInterface laborInterface = new LaborInterface();
                if (laborInterface.EmployeeIsClockedIn(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid))
                    result.exception = "Employee is already clocked in";
                else
                {
                    EmployeeInterface employeeInterface = new EmployeeInterface();
                    employeeInterface.ClockIn(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid, Int32.Parse(shift));
                }

            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }
            return result;
        }

        public clockoutresult clockout(string company, string employeeid)
        {
            clockoutresult result = new clockoutresult();
            try
            {
                string username = company.ToUpper() + "Service";
                LaborInterface laborInterface = new LaborInterface();
                if (!laborInterface.EmployeeIsClockedIn(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid))
                    result.exception = "Employee is not clocked in";
                else
                {
                    if (laborInterface.EmployeeCanClockOut(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid))
                    {
                        EmployeeInterface employeeInterface = new EmployeeInterface();
                        employeeInterface.ClockOut(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid);
                        if (company == "NEM")
                        {
                            laborInterface.RoundLastClock(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid);
                        }
                    }
                    else
                        result.exception = "Employee has active labor";
                }

            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }
            return result;
        }

        public startactivityresult startactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq)
        {
            startactivityresult result = new startactivityresult();
            string username = company.ToUpper() + "Service";
            try
            {
                if (company == "CIG")
                    username = company + plant + "Service";
                LaborInterface laborInterface = new LaborInterface();
                if (company == "NEM")
                {
                    if (!laborInterface.EmployeeHasActiveJobLabor(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid, jobnum))
                    {
                        laborInterface.StartActivity(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid, jobnum, Int32.Parse(assemblyseq), Int32.Parse(oprseq));
                    }
                    else
                        result.exception = "Employee has active labor on this job already.  Cannot start additional activity";
                }
                else
                {
                    laborInterface.StartActivity(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid, jobnum, Int32.Parse(assemblyseq), Int32.Parse(oprseq));
                }
            }
            catch (Exception ex)
            {
                result.exception = "User = " + username + " Company = " + company + ". " + ex.Message;
            }

            return result;
        }

        public startindirectresult startindirect(string company, string employeeid, string indirectcode, string resourcegroup)
        {
            startindirectresult result = new startindirectresult();
            try
            {
                string username = company.ToUpper() + "Service";
                LaborInterface laborInterface = new LaborInterface();
                if (laborInterface.EmployeeHasActiveLabor(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid))
                    result.exception = "Employee has active labor.  Cannot start indirect activity";
                else
                    laborInterface.StartIndirectActivity(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid, indirectcode, resourcegroup);
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public endindirectresult endindirect(string company, string employeeid)
        {
            endindirectresult result = new endindirectresult();
            try
            {
                string username = company.ToUpper() + "Service";
                LaborInterface laborInterface = new LaborInterface();
                laborInterface.EndIndirectActivity(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid);
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public endactivityresult endactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq, string qty, string scrap)
        {
            endactivityresult result = new endactivityresult();

            try
            {
                string username = company.ToUpper() + "Service";
                string scrapcode = "SCRAP";
                string scrapdesc = "Scrapped Item";
                if (company == "CIG")
                {
                    username = "CIG" + plant + "Service";
                    scrapdesc = "Scrapped Glass";
                }
                LaborInterface laborInterface = new LaborInterface();
                laborInterface.EndActivity(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, "vantage", employeeid, jobnum, Int32.Parse(assemblyseq), Int32.Parse(oprseq), String.IsNullOrEmpty(qty) ? 0 : Decimal.Parse(qty), String.IsNullOrEmpty(scrap) ? 0 : Decimal.Parse(scrap), scrapcode, scrapdesc);
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public getindirectcodesresult getindirectcodes(string company)
        {
            getindirectcodesresult result = new getindirectcodesresult();

            try
            {
                LaborInterface laborInterface = new LaborInterface();
                result.epicor = laborInterface.GetIndirectCodes(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company);
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public getresourcecodesresult getresourcecodes(string company)
        {
            getresourcecodesresult result = new getresourcecodesresult();

            try
            {
                LaborInterface laborInterface = new LaborInterface();
                result.epicor = laborInterface.GetResourceCodes(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company);
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public getstringresult getvacationhours(string company, string id)
        {
            getstringresult result = new getstringresult();

            try
            {
                PREmployeeInterface i = new PREmployeeInterface();
                result.epicor = i.GetVacationTimeRemaining(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company, id);
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }
            return result;
        }

        public getstringresult getempname(string company, string id)
        {
            getstringresult result = new getstringresult();

            try
            {
                PREmployeeInterface i = new PREmployeeInterface();
                result.epicor = i.GetName(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company, id);
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
