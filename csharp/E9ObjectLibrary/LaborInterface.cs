using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Epicor.Mfg.Core;
using Epicor.Mfg.BO;

namespace E9ObjectLibrary
{
    public class LaborInterface : EpicorExtension<Epicor.Mfg.BO.Labor>
    {
        #region Public Methods

        #region Select Methods

        public Dictionary<string, string> GetIndirectCodes(string server, string database, string username, string password, string company)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetIndirectCodesForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[0].ToString(), row[1].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Dictionary<string, string> GetResourceCodes(string server, string database, string username, string password, string company)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetResourceGroupsForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[0].ToString(), row[1].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool EmployeeHasActiveLabor(string server, string port, string username, string password, string employeeID)
        {
            bool result = false;
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);
                if (ds.LaborHedList.Rows.Count > 0)
                {
                    LaborHedListDataSet.LaborHedListRow row = ds.LaborHedList.Rows[0] as LaborHedListDataSet.LaborHedListRow;

                    int laborHedSeq = row.LaborHedSeq;

                    LaborDataSet laborDS = BusinessObject.GetByID(laborHedSeq);
                    for (int i = 0; i < laborDS.LaborDtl.Rows.Count; i++)
                    {
                        if (((LaborDataSet.LaborDtlRow)laborDS.LaborDtl.Rows[i]).ActiveTrans)
                            result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }

            return result;
        }

        public bool EmployeeHasActiveJobLabor(string server, string port, string username, string password, string employeeID, string jobnum)
        {
            bool result = false;
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);
                if (ds.LaborHedList.Rows.Count > 0)
                {
                    LaborHedListDataSet.LaborHedListRow row = ds.LaborHedList.Rows[0] as LaborHedListDataSet.LaborHedListRow;

                    int laborHedSeq = row.LaborHedSeq;

                    LaborDataSet laborDS = BusinessObject.GetByID(laborHedSeq);
                    for (int i = 0; i < laborDS.LaborDtl.Rows.Count; i++)
                    {
                        LaborDataSet.LaborDtlRow lrow = laborDS.LaborDtl.Rows[i] as LaborDataSet.LaborDtlRow;
                        if (lrow.ActiveTrans && lrow.JobNum == jobnum)
                            result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }

            return result;
        }

        public bool EmployeeIsClockedIn(string server, string port, string username, string password, string employeeID)
        {
            bool result = false;
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);
                if (ds.LaborHedList.Rows.Count > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }

            return result;
        }

        public void RoundLastClockIn(string server, string port, string username, string password, string employeeID)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);
                foreach (LaborHedListDataSet.LaborHedListRow row in ds.LaborHedList.Rows)
                {
                    LaborDataSet laborDS = BusinessObject.GetByID(row.LaborHedSeq);
                    BusinessObject.DefaultTime(laborDS, "ClockInTime", RoundToNearestQuarter(row.ActualClockInTime));
                    //                    ((LaborDataSet.LaborHedRow)laborDS.LaborHed.Rows[0]).ClockInTime = RoundToNearestQuarter(row.ActualClockInTime);
                    BusinessObject.Update(laborDS);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }
        }

        public void RoundLastClock(string server, string port, string username, string password, string employeeID)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = no", 0, 0, out morePages);
                int maxIndex = -1;
                decimal maxTime = 0;
                DateTime maxDate = new DateTime();
                for (int i = 0; i < ds.LaborHedList.Rows.Count; i++)
                {
                    if (maxIndex == -1)
                    {
                        maxIndex = i;
                        maxTime = ((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[i]).ActualClockOutTime;
                        maxDate = ((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[i]).ActualClockinDate;
                    }
                    else if (((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[i]).ActualClockinDate > maxDate
                        || (((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[i]).ActualClockinDate >= maxDate && ((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[i]).ActualClockOutTime > maxTime))
                    {
                        maxIndex = i;
                        maxTime = ((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[i]).ActualClockOutTime;
                        maxDate = ((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[i]).ActualClockinDate;
                    }
                }
                LaborDataSet laborDS = BusinessObject.GetByID(((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[maxIndex]).LaborHedSeq);
                ((LaborDataSet.LaborHedRow)laborDS.LaborHed.Rows[0]).RowMod = "U";
                ((LaborDataSet.LaborHedRow)laborDS.LaborHed.Rows[0]).Character01 = "WPB";
                BusinessObject.DefaultTime(laborDS, "ClockInTime", RoundToNearestQuarter(((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[maxIndex]).ClockInTime));
                BusinessObject.DefaultTime(laborDS, "ClockOutTime", RoundToNearestQuarter(((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[maxIndex]).ClockOutTime));
                //                ((LaborDataSet.LaborHedRow)laborDS.LaborHed.Rows[0]).ClockOutTime = RoundToNearestQuarter(((LaborHedListDataSet.LaborHedListRow)ds.LaborHedList.Rows[maxIndex]).ActualClockOutTime);
                BusinessObject.Update(laborDS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }
        }

        private decimal RoundToNearestQuarter(decimal toround)
        {
            int hours = (int)toround / 1;
            int minutes = (int)Math.Floor(60 * (toround % 1));
            string time = hours + ":" + minutes;
            int rounded = (int)Math.Round((decimal)minutes / 15.0m, MidpointRounding.ToEven) * 15;
            decimal final = (decimal)hours + ((decimal)rounded / 60.0m);
            return final;
        }

        public bool EmployeeCanClockOut(string server, string port, string username, string password, string employeeID)
        {
            bool result = true;
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);

                if (ds.LaborHedList.Rows.Count > 0)
                {
                    LaborHedListDataSet.LaborHedListRow row = ds.LaborHedList.Rows[0] as LaborHedListDataSet.LaborHedListRow;
                    int laborHedSeq = row.LaborHedSeq;

                    LaborDataSet laborDS = BusinessObject.GetByID(laborHedSeq);

                    foreach (LaborDataSet.LaborDtlRow dtlrow in laborDS.LaborDtl.Rows)
                    {
                        if (dtlrow.ActiveTrans)
                            result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }

            return result;
        }

        #endregion

        #region Update Methods

        public string StartActivity(string server, string port, string username, string password, string employeeID, string jobnum, int assemblyseq, int oprseq)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            string result = "";
            string debug = "";
            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);

                if (ds.LaborHedList.Rows.Count == 0)
                {
                    throw new Exception("The user #" + employeeID + " is not logged in.  Log in through MES View first");
                }
                else
                {
                    LaborHedListDataSet.LaborHedListRow row = ds.LaborHedList.Rows[0] as LaborHedListDataSet.LaborHedListRow;

                    int laborHedSeq = row.LaborHedSeq;

                    LaborDataSet laborDS = BusinessObject.GetByID(laborHedSeq);
                    debug = "1";
                    BusinessObject.StartActivity(laborHedSeq, "P", laborDS);
                    debug = "2";
                    BusinessObject.DefaultJobNum(laborDS, jobnum);
                    debug = "3";
                    BusinessObject.DefaultAssemblySeq(laborDS, assemblyseq);
                    debug = "4";

                    string message;
                    BusinessObject.DefaultOprSeq(laborDS, oprseq, out message);
                    if (!String.IsNullOrEmpty(message))
                        throw new Exception(message);
                    debug = "5";

                    string resourceID = ((LaborDataSet.LaborDtlRow)laborDS.LaborDtl.Rows[0]).ResourceID;
                    BusinessObject.CheckWarnings(laborDS, out message);
                    if (!String.IsNullOrEmpty(message))
                        throw new Exception(message);
                    debug = "6";

                    BusinessObject.CheckEmployeeActivity(employeeID, laborHedSeq, jobnum, assemblyseq, oprseq, resourceID, out message);
                    if (!String.IsNullOrEmpty(message))
                        throw new Exception(message);
                    else
                    {
                        debug = "7";
                        BusinessObject.CheckFirstArticleWarning(laborDS, out message);
                        if (!String.IsNullOrEmpty(message))
                            throw new Exception(message);

                        debug = "8";
                        BusinessObject.SetClockInAndDisplayTimeMES(laborDS);
                        debug = "9";
                        BusinessObject.Update(laborDS);
                        debug = "10";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("E Id = " + employeeID + ", jobnum " + jobnum + ", asm " + assemblyseq + ", opr " + oprseq + ". " +debug +". " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        public string EndActivity(string server, string port, string username, string password, string employeeID, string jobnum, int assemblyseq, int oprseq, decimal qty, decimal scrap, string scrapreason, string scrapreasondesc)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            string result = "";

            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);

                if (ds.LaborHedList.Rows.Count == 0)
                {
                    throw new Exception("The user #" + employeeID + " is not logged in.  Log in through MES View first");
                }
                else
                {
                    LaborHedListDataSet.LaborHedListRow row = ds.LaborHedList.Rows[0] as LaborHedListDataSet.LaborHedListRow;

                    int laborHedSeq = row.LaborHedSeq;

                    LaborDataSet laborDS = BusinessObject.GetByID(laborHedSeq);

                    int laborDtlSeq = -1;
                    foreach (LaborDataSet.LaborDtlRow dtlrow in laborDS.LaborDtl.Rows)
                    {
                        if (dtlrow.ActiveTrans && dtlrow.JobNum == jobnum && dtlrow.AssemblySeq == assemblyseq && dtlrow.OprSeq == oprseq)
                        {
                            laborDtlSeq = dtlrow.LaborDtlSeq;
                            break;
                        }
                    }

                    if (laborDtlSeq >= 0)
                    {
                        laborDS = BusinessObject.GetDetail(laborHedSeq, laborDtlSeq);
                        ((LaborDataSet.LaborDtlRow)laborDS.LaborDtl.Rows[0]).RowMod = "U";
                        BusinessObject.EndActivity(laborDS);

                        string message;
                        if (qty > 0)
                        {
                            BusinessObject.DefaultLaborQty(laborDS, qty, out message);

                            if (!String.IsNullOrEmpty(message))
                                throw new Exception(message);
                        }

                        if (scrap > 0)
                        {
                            ((LaborDataSet.LaborDtlRow)laborDS.LaborDtl.Rows[0]).ScrapQty = scrap;
                            ((LaborDataSet.LaborDtlRow)laborDS.LaborDtl.Rows[0]).ScrapReasonCode = scrapreason;
                            ((LaborDataSet.LaborDtlRow)laborDS.LaborDtl.Rows[0]).ScrapReasonDescription = scrapreasondesc;
                        }
                        BusinessObject.CheckWarnings(laborDS, out message);
                        if (!String.IsNullOrEmpty(message))
                            throw new Exception(message);

                        BusinessObject.Update(laborDS);
                    }
                    else
                        throw new Exception("Employeed #" + employeeID + " is not active on this task");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        public string StartIndirectActivity(string server, string port, string username, string password, string employeeID, string indirectcode, string resourcegroup)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            string result = "";
            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);

                if (ds.LaborHedList.Rows.Count == 0)
                {
                    throw new Exception("The user #" + employeeID + " is not logged in.  Log in through MES View first");
                }
                else
                {
                    LaborHedListDataSet.LaborHedListRow row = ds.LaborHedList.Rows[0] as LaborHedListDataSet.LaborHedListRow;

                    int laborHedSeq = row.LaborHedSeq;

                    LaborDataSet laborDS = BusinessObject.GetByID(laborHedSeq);

                    BusinessObject.StartActivity(laborHedSeq, "I", laborDS);
                    BusinessObject.DefaultIndirect(laborDS, indirectcode);
                    string message = "";
                    BusinessObject.DefaultWCCode(laborDS, resourcegroup, out result);

                    if (!String.IsNullOrEmpty(message))
                        throw new Exception(message);

                    BusinessObject.CheckWarnings(laborDS, out message);
                    if (!String.IsNullOrEmpty(message))
                        throw new Exception(message);

                    BusinessObject.Update(laborDS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        public string EndIndirectActivity(string server, string port, string username, string password, string employeeID)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            string result = "";

            try
            {
                bool morePages;
                LaborHedListDataSet ds = BusinessObject.GetList("EmployeeNum = '" + employeeID + "' and ActiveTrans = yes", 0, 0, out morePages);

                if (ds.LaborHedList.Rows.Count == 0)
                {
                    throw new Exception("The user #" + employeeID + " is not logged in.  Log in through MES View first");
                }
                else
                {
                    LaborHedListDataSet.LaborHedListRow row = ds.LaborHedList.Rows[0] as LaborHedListDataSet.LaborHedListRow;

                    int laborHedSeq = row.LaborHedSeq;

                    LaborDataSet laborDS = BusinessObject.GetByID(laborHedSeq);

                    int laborDtlSeq = -1;
                    foreach (LaborDataSet.LaborDtlRow dtlrow in laborDS.LaborDtl.Rows)
                    {
                        if (dtlrow.ActiveTrans && dtlrow.LaborType == "I")
                        {
                            laborDtlSeq = dtlrow.LaborDtlSeq;
                            break;
                        }
                    }

                    if (laborDtlSeq >= 0)
                    {
                        laborDS = BusinessObject.GetDetail(laborHedSeq, laborDtlSeq);
                        ((LaborDataSet.LaborDtlRow)laborDS.LaborDtl.Rows[0]).RowMod = "U";
                        BusinessObject.EndActivity(laborDS);

                        string message;
                        BusinessObject.CheckWarnings(laborDS, out message);
                        if (!String.IsNullOrEmpty(message))
                            throw new Exception(message);

                        BusinessObject.Update(laborDS);
                    }
                    else
                        throw new Exception("Employeed #" + employeeID + " has no indirect activity");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        #endregion

        #endregion
    }
}
