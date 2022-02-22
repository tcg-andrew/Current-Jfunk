using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epicor.Mfg.BO;
using Epicor.Mfg.Core;
using System.Data;
using System.Data.SqlClient;

namespace ObjectLibrary
{
    public class OrderInterface: EpicorExtension<SalesOrder>
    {
        public List<int> GetOrdersForQuoteLine(string server, string database, string username, string password, int quotenum, int quoteline)
        {
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetOrdersForQuoteLine @Company, @Quotenum, @Quoteline");
            sqlCommand.Parameters.AddWithValue("Company", "CRD");
            sqlCommand.Parameters.AddWithValue("Quotenum", quotenum);
            sqlCommand.Parameters.AddWithValue("Quoteline", quoteline);
            List<int> result = new List<int>();

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.Add(Int32.Parse(row[0].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void UpdateOrderComment(string server, string port, string username, string password, int ordernum, int quotenum, int quoteline, string comment)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                SalesOrderDataSet ds = BusinessObject.GetByID(ordernum);

                foreach (SalesOrderDataSet.OrderDtlRow row in ds.OrderDtl.Rows)
                {
                    if (row.QuoteNum == quotenum && row.QuoteLine == quoteline)
                    {
                        row.OrderComment = comment;
                        row.RowMod = "U";
                    }
                }

                BusinessObject.Update(ds);
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

        public void UpdateOrderJobComment(string server, string port, string username, string password, int ordernum, int quotenum, int quoteline, string comment)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                SalesOrderDataSet ds = BusinessObject.GetByID(ordernum);

                foreach (SalesOrderDataSet.OrderDtlRow row in ds.OrderDtl.Rows)
                {
                    if (row.QuoteNum == quotenum && row.QuoteLine == quoteline)
                    {
                        row.PickListComment = comment;
                        row.RowMod = "U";
                    }
                }

                BusinessObject.Update(ds);
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

        public void UpdateOrderReqDate(string server, string port, string username, string password, Job j)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                SalesOrderDataSet ds = BusinessObject.GetByID(Int32.Parse(j.OrderNum));
                
                foreach (Epicor.Mfg.BO.SalesOrderDataSet.OrderRelRow rel in ds.OrderRel.Rows)
                {
                    if (rel.OrderLine == j.OrderLine && rel.OrderRelNum == j.OrderRel)
                        rel.ReqDate = j.OrderDate;
                }

                BusinessObject.Update(ds);
                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateOrderLineDates(string server, string port, string username, string password, string ordernum, string orderline, DateTime needbydate, DateTime reqdate)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                SalesOrderDataSet ds = BusinessObject.GetByID(Int32.Parse(ordernum));

                foreach (Epicor.Mfg.BO.SalesOrderDataSet.OrderDtlRow dtl in ds.OrderDtl.Rows)
                {
                    if (dtl.OrderLine == Int32.Parse(orderline))
                    {
                        dtl.NeedByDate = needbydate;
                        dtl.RequestDate = reqdate;
                    }
                }

                foreach (Epicor.Mfg.BO.SalesOrderDataSet.OrderRelRow rel in ds.OrderRel.Rows)
                {
                    if (rel.OrderLine == Int32.Parse(orderline) && rel.OpenRelease)
                    {
                        rel.NeedByDate = needbydate;
                        rel.ReqDate = reqdate;
                    }
                }

                BusinessObject.Update(ds);
                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOrderLinesWithWrongDate(string server, string database, string username, string password)
        {
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetOrderLines_WrongDate");
            DataSet result = new DataSet();

            try
            {
                result = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
