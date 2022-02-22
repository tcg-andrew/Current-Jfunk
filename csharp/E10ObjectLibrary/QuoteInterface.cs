#region Usings

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Erp.Proxy.BO;
using Erp.BO;
using Ice.Core;
using Erp.Contracts;

#endregion

namespace ObjectLibrary
{
    // 1 - New Quote
    // Foreach Line Item
    // 2 - New Line Item
    // 3 - New Configuration
    // 4 - Update Line Quote
    // End foreach
    // 5 - Assembly

    // TODO:5 Add unit tests

    #region Class QuoteInfo

    public class QuoteInfo
    {
        public string Description { get; set; }
        public string Price { get; set; }
        public Dictionary<string, string> FreightWeight { get; set; }

        public QuoteInfo()
        {
            Description = "";
            Price = "";
            FreightWeight = new Dictionary<string, string>();
        }

        public QuoteInfo(string description, string price, Dictionary<string, string> weights)
        {
            Description = description;
            Price = price;
            foreach (string key in weights.Keys)
                FreightWeight.Add(key, weights[key]);
        }
    }

    #endregion

    public struct NewQuoteLine
    {
        public int linenum { get; set; }
        public string revision { get; set; }
        public bool forcePrice { get; set; }
    }

    public class QuoteInterface : EpicorExtension<QuoteImpl, QuoteSvcContract>
    {
        #region Public Methods

        #region Create Methods

        public int CreateQuote(string server, string port, string username, string password, string railsid, string custid, string shiptonum, string csr, string comment, string carrier, int transitdays, string jobref, string salesrepcode)
        {
            int quotenum = 0;
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteDataSet ds = new QuoteDataSet();
                BusinessObject.GetNewQuoteHed(ds);

                QuoteDataSet.QuoteHedRow newQuote = ds.QuoteHed.Rows[0] as QuoteDataSet.QuoteHedRow;
                newQuote.CustomerCustID = custid;
                newQuote.ConfidencePct = 100;
                newQuote.QuoteComment = comment;
                newQuote.Reference = csr;
                newQuote.SalesRepCode = salesrepcode;
                newQuote["shortchar03"] = railsid;
                newQuote["number03"] = transitdays;
                newQuote["shortchar09"] = jobref;
                BusinessObject.GetCustomerInfo(ds);
                newQuote.ShipToNum = shiptonum;
                BusinessObject.GetShipToInfo(ds);
                newQuote.ShipViaCode = carrier;
                BusinessObject.Update(ds);

                quotenum = newQuote.QuoteNum;
            }
            catch (Exception ex)
            {
                throw new Exception("CreateQuote - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return quotenum;
        }

        public NewQuoteLine CreateLineItem(string server, string port, string username, string password, string railsid, int quotenum, string partnum, string description, decimal discount, string qcomment, string jcomment, int sys_weight, XmlQuoteParse xml = null)
        {
            NewQuoteLine line = new NewQuoteLine();
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteDataSet ds = BusinessObject.GetByID(quotenum);
                QuoteDataSet.QuoteHedRow quote = ds.QuoteHed.Rows[0] as QuoteDataSet.QuoteHedRow;
                BusinessObject.GetNewQuoteDtl(ds, quote.QuoteNum);
                QuoteDataSet.QuoteDtlRow[] lineItems = quote.GetQuoteDtlRows();
                QuoteDataSet.QuoteDtlRow newQuoteLineItem = lineItems[lineItems.Length - 1];

                newQuoteLineItem.PartNum = partnum;
                BusinessObject.ChangePartNum(ds, false, "");
                BusinessObject.Update(ds);

                QuoteDataSet.QuoteQtyRow[] qtys = newQuoteLineItem.GetQuoteQtyRows();
                if (qtys.Length > 0 && (qtys[0].DocUnitPrice == 99999 || qtys[0].DocUnitPrice == 999999 || qtys[0].DocUnitPrice == 99999999))
                    line.forcePrice = true;

                newQuoteLineItem.ConfidencePct = 100;
                if (partnum != "SHIP")
                    newQuoteLineItem.DiscountPercent = discount;
                else
                    newQuoteLineItem.DiscountPercent = 0;

                newQuoteLineItem.QuoteComment = qcomment;
                newQuoteLineItem.JobComment = jcomment;
                newQuoteLineItem.CheckPartDescription = false;
                newQuoteLineItem["shortchar03"] = railsid;
                if (xml != null)
                {   if (!String.IsNullOrEmpty(xml.ProdCode))
                        newQuoteLineItem.ProdCode = xml.ProdCode;
                    newQuoteLineItem["number01"] = xml.Weight;
                    newQuoteLineItem["character01"] = xml.Character01;
                    newQuoteLineItem["character03"] = xml.Character03;
                    newQuoteLineItem["character05"] = xml.Character05;
                }
                newQuoteLineItem["number02"] = sys_weight;
                BusinessObject.Update(ds);

                if (partnum != "SYSTEM" && partnum != "SHELVING" && !partnum.StartsWith("00W") && !String.IsNullOrEmpty(description))
                    newQuoteLineItem.LineDesc = description;

                BusinessObject.Update(ds);

                line.linenum = newQuoteLineItem.QuoteLine;
                line.revision = newQuoteLineItem.RevisionNum;
            }
            catch (Exception ex)
            {
                throw new Exception("CreateLineItem - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }

            return line;
        }

        public void CreateMiscCharge(string server, string port, string username, string password, int quotenum, int linenum, decimal amt, string code, string desc)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                QuoteDataSet ds = BusinessObject.GetByID(quotenum);
                BusinessObject.GetNewQuoteMsc(ds, quotenum, linenum, 0);
                QuoteDataSet.QuoteMscRow newRow = ds.QuoteMsc[ds.QuoteMsc.Rows.Count - 1];
                newRow.MiscCode = code;
                BusinessObject.GetMiscChrgDefaults(ds, "QuoteMsc");
                //                ds.QuoteMsc[0].MiscCodeDescription = desc;
                //              ds.QuoteMsc[0].Description = desc;
                // newRow.DocMiscAmt = amt;
                newRow.DocDspMiscAmt = amt;
                newRow.DocMiscAmt = amt;
                newRow.MiscAmt = amt;
                newRow.DspMiscAmt = amt;
                newRow.FreqCode = "L";
                newRow.CurrencySwitch = false;
                newRow.MiscCodeDescription = ds.QuoteMsc[0].Description + " - " + desc;
                newRow.Description = desc;
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("CreateMiscCharge - {0}", ex.Message));
            }
            finally
            {
                CloseSession();
            }

        }

        #region To Be Deprecated

        public int NewLineItem(string server, string port, string username, string password, int quotenum, string partnum)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            QuoteDataSet ds = BusinessObject.GetByID(quotenum);
            QuoteDataSet.QuoteHedRow quote = ds.QuoteHed.Rows[0] as QuoteDataSet.QuoteHedRow;
            BusinessObject.GetNewQuoteDtl(ds, quote.QuoteNum);
            QuoteDataSet.QuoteDtlRow[] lineItems = quote.GetQuoteDtlRows();
            QuoteDataSet.QuoteDtlRow newQuoteLineItem = lineItems[lineItems.Length - 1];

            newQuoteLineItem.PartNum = partnum;
            BusinessObject.ChangePartNum(ds, false, "");
            BusinessObject.Update(ds);

            newQuoteLineItem.OrderQty = 1;
            newQuoteLineItem.SellingExpectedQty = 1;
            newQuoteLineItem.ConfidencePct = 100;
            newQuoteLineItem.CheckPartDescription = true;

            BusinessObject.GetDtlUnitPriceInfo(false, false, ds);
            BusinessObject.Update(ds);

            CloseSession();

            return newQuoteLineItem.QuoteLine;
        }

        public int NewQuote(string server, string port, string username, string password, string custid)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            QuoteDataSet ds = new QuoteDataSet();
            BusinessObject.GetNewQuoteHed(ds);

            QuoteDataSet.QuoteHedRow newQuote = ds.QuoteHed.Rows[0] as QuoteDataSet.QuoteHedRow;
            newQuote.CustomerCustID = custid;
            BusinessObject.GetCustomerInfo(ds);
            BusinessObject.Update(ds);

            CloseSession();

            return newQuote.QuoteNum;
        }

        #endregion

        #endregion

        #region Retrieve Methods

        public decimal GetQuoteTotal(string server, string port, string username, string password, int quotenum)
        {
            decimal sum = 0;
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                QuoteDataSet quote = BusinessObject.GetByID(quotenum);

                foreach (QuoteDataSet.QuoteDtlRow row in quote.QuoteDtl)
                    if (row.KitParentLine == row.QuoteLine)
                        sum += row.ExpectedRevenue;
            }
            catch (Exception ex)
            {
                throw new Exception("GetQuoteTotal - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return sum;
        }

        public QuoteInfo GetQuote(string server, string port, string username, string password, string database, string dbusername, string dbpassword, int quotenum)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            QuoteDataSet quote = BusinessObject.GetByID(quotenum);

            QuoteInfo result = new QuoteInfo();
            result.Description = quote.QuoteDtl[0].LineDesc;
            decimal sum = 0;
            foreach (QuoteDataSet.QuoteDtlRow row in quote.QuoteDtl)
                if (row.KitParentLine == row.QuoteLine)
                    sum += row.ExpectedRevenue;
            result.Price = String.Format("{0:0.00}", sum);

            CloseSession();

            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Quote_CalcWeight @Company, @QuoteNum");
            sqlCommand.Parameters.AddWithValue("Company", "CRD");
            sqlCommand.Parameters.AddWithValue("QuoteNum", quotenum);

            DataSet ds = SQLAccess.GetDataSet(server, database, dbusername, dbpassword, sqlCommand);

            foreach (DataRow row in ds.Tables[0].Rows)
                result.FreightWeight.Add(row["BOLClass"].ToString(), Int32.Parse(row["weight"].ToString().Split('.')[0]).ToString());

            return result;
        }

        #endregion

        #region Update Methods

        public void ReadyToQuote(string server, string port, string username, string password, int quotenum, int linenum, bool ready)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                QuoteDataSet ds = BusinessObject.GetByID(quotenum);

                foreach (QuoteDataSet.QuoteDtlRow row in ds.QuoteDtl.Rows)
                    if (row.QuoteLine == linenum)
                        row.ReadyToQuote = ready;

                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("ReadyToQuote - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        public void UpdateLineQty(string server, string port, string username, string password, int quotenum, int linenum, int qty)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                QuoteDataSet ds = BusinessObject.GetByID(quotenum);

                foreach (QuoteDataSet.QuoteDtlRow row in ds.QuoteDtl.Rows)
                    if (row.QuoteLine == linenum)
                        row.OrderQty = qty;

                BusinessObject.ChangeOrderQty(ds);
                BusinessObject.Update(ds);

                decimal discount = 0;
                foreach (QuoteDataSet.QuoteDtlRow row in ds.QuoteDtl.Rows)
                    if (row.QuoteLine == linenum)
                    {
                        row.SellingExpectedQty = qty;
                        row.MiscQtyNum = qty;
                        discount = row.DiscountPercent;
                        row.RowMod = "U";
                    }

                BusinessObject.ChangeSellingExpQty(ds);
                foreach (QuoteDataSet.QuoteDtlRow row in ds.QuoteDtl.Rows)
                    if (row.QuoteLine == linenum)
                    {
                        row.DiscountPercent = discount;
                        row.RowMod = "U";
                    }
                BusinessObject.Update(ds);

                foreach (QuoteDataSet.QuoteDtlRow row in ds.QuoteDtl.Rows)
                    if (row.QuoteLine == linenum)
                        row.RowMod = "U";

                BusinessObject.GetDtlUnitPriceInfo(false, false, ds);
                BusinessObject.Update(ds);

            }
            catch (Exception ex)
            {
                throw new Exception("UpdateLineQty - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }

        }

        public void UpdateLineQuote(string server, string port, string username, string password, int quotenum, int linenum)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteDataSet ds = BusinessObject.GetByID(quotenum);

                foreach (QuoteDataSet.QuoteQtyRow row in ds.QuoteQty.Rows)
                    if (row.QuoteLine == linenum)
                        row.RowMod = "U";

                bool outbool;

                BusinessObject.GetQtyPriceInfoCfgPart(ds, out outbool);
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateLineQuote - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        public void UpdateLinePrice(string server, string port, string username, string password, int quotenum, int linenum, decimal price)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteDataSet ds = BusinessObject.GetByID(quotenum);
                foreach (QuoteDataSet.QuoteQtyRow row in ds.QuoteQty.Rows)
                {
                    if (row.QuoteLine == linenum)
                    {
                        row.DocUnitPrice = price;
                        row.CurrencySwitch = false;
                    }
                }

                BusinessObject.RecalcWorksheet(quotenum, linenum, 1, ds);
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateLinePrice - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        public void UpdateKitComponents(string server, string port, string username, string password, string partnum, int quotenum, NewQuoteLine lineinfo)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteDataSet ds = BusinessObject.GetByID(quotenum);
                BusinessObject.RemoveKitComponents(partnum, quotenum, lineinfo.linenum, ds);

                string msg = "";
                BusinessObject.GetKitComponents(partnum, lineinfo.revision, "", 0, quotenum, lineinfo.linenum, false, true, ref msg, ds);

                ds = BusinessObject.GetByID(quotenum);
                foreach (QuoteDataSet.QuoteDtlRow row in ds.QuoteDtl.Rows)
                    if (row.QuoteLine == lineinfo.linenum)
                        row.KitsLoaded = true;
                BusinessObject.Update(ds);

            }
            catch (Exception ex)
            {
                throw new Exception("UpdateKitComponents - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        #endregion

        #region Delete Methods

        public void SyncLineData(string server, string port, string database, string username, string password, string dbusername, string dbpassword, int quotenum, List<QuoteLine> lineItems)
        {
            string quoteline = "";
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteDataSet ds = BusinessObject.GetByID(quotenum);
                int railsline = 0;
                foreach (QuoteLine line in lineItems)
                {
                    railsline++;
                    if (line.lineNum != 0)
                    {
                        quoteline = " Rails Line " + railsline.ToString() + " - Vantage Line " + line.lineNum.ToString();
                        for (int i = 0; i < ds.QuoteDtl.Rows.Count; i++)
                        {
                            if (((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).KitParentLine == ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine 
                                && ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine == line.lineNum)
                            {
                                if (((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteComment.Replace("\n", "").Replace("\r", "").Trim() != line.QuoteComment.Replace("\n", "").Replace("\r", "").Trim())
                                {
                                    ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteComment = line.QuoteComment;
                                    ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).RowMod = "U";

                                    OrderInterface orderinterface = new OrderInterface();
                                    List<int> orders = orderinterface.GetOrdersForQuoteLine(server, database, dbusername, dbpassword, quotenum, line.lineNum);
                                    foreach (int ordernum in orders)
                                        orderinterface.UpdateOrderComment(server, port, username, password, ordernum, quotenum, line.lineNum, line.QuoteComment);    

                                }
                                if (((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).JobComment.Replace("\n", "").Replace("\r", "").Trim() != line.JobComment.Replace("\n", "").Replace("\r", "").Trim())
                                {
                                    ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).JobComment = line.JobComment;
                                    ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).RowMod = "U";

                                    OrderInterface orderinterface = new OrderInterface();
                                    List<int> orders = orderinterface.GetOrdersForQuoteLine(server, database, dbusername, dbpassword, quotenum, line.lineNum);
                                    foreach (int ordernum in orders)
                                        orderinterface.UpdateOrderJobComment(server, port, username, password, ordernum, quotenum, line.lineNum, line.JobComment);    
                                }
                                
                            }
                        }
                    }
                }
                BusinessObject.Update(ds);

            }
            catch (Exception ex)
            {
                throw new Exception("SyncLineData - " + ex.Message + quoteline);
            }
            finally
            {
                CloseSession();
            }
        }

/*        public void PrepareReconfig(string server, string database, string port, string username, string password, string db_username, string db_password, int quotenum, List<QuoteLine> lineItems)
        {
            string quoteline = "";
            string reason = "";
            try
            {
                PartInterface partInterface = new PartInterface();
                ConfigurationInterface configInterface = new ConfigurationInterface();

                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteDataSet ds = BusinessObject.GetByID(quotenum);

                List<int> oldLines = new List<int>();
                foreach (QuoteLine line in lineItems)
                {
                    if (line.lineNum != 0 && !oldLines.Contains(line.lineNum))
                        oldLines.Add(line.lineNum);
                }

                for (int i = 0; i < ds.QuoteDtl.Rows.Count; i++)
                {
                    if (((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).KitParentLine == ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine)
                    {
                        bool foundmatch = false;
                        int railsline = 0;
                        foreach (QuoteLine line in lineItems)
                        {
                            railsline++;
                            if (foundmatch == false)
                            {
                                if (line.lineNum == 0)
                                {
                                    if (!oldLines.Contains(((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine))
                                    {
                                        if (line.Process && ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).PartNum.ToUpper() == line.ActualPartNum.ToUpper())
                                        {
                                            bool match = CompareLine(((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]), line, ds, configInterface, server, port, username, password, ref reason);

                                            if (match)
                                            {
                                                foundmatch = true;
                                                line.Process = false;
                                                line.lineNum = ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (line.Process && ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine == line.lineNum)
                                    {
                                        bool match = CompareLine(((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]), line, ds, configInterface, server, port, username, password, ref reason);

                                        if (match)
                                        {
                                            foundmatch = true;
                                            line.Process = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (!foundmatch)
                        {
                            try
                            {
                                quoteline = " Quote line " + ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine.ToString() + " ";
                                //                            ReadyToQuote(server, port, username, password, quotenum, ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine, false);
                                foreach (QuoteDataSet.QuoteMscRow row1 in ds.QuoteMsc.Rows)
                                    if (row1.RowState != DataRowState.Deleted && row1.QuoteLine == ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine)
                                        row1.Delete();
                                foreach (QuoteDataSet.QtmmkupRow row2 in ds.Qtmmkup.Rows)
                                    if (row2.RowState != DataRowState.Deleted && row2.QuoteLine == ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine)
                                        row2.Delete();
                                foreach (QuoteDataSet.QuoteQtyRow row3 in ds.QuoteQty.Rows)
                                    if (row3.RowState != DataRowState.Deleted && row3.QuoteLine == ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine)
                                        row3.Delete();

                                if (partInterface.PartIsSalesKit(server, port, username, password, ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).PartNum))
                                {
                                    int quoteLine = ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine;
                                    BusinessObject.RemoveKitComponents(((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).PartNum, ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteNum, ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine, ds);
                                    for (i = 0; i < ds.QuoteDtl.Rows.Count; i++)
                                        if (((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine == quoteLine)
                                            break;
                                }
                                ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).Delete();
                                BusinessObject.Update(ds);
                                i--;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("Delete not allowed"))
                                {
                                    try
                                    {
                                        OrderInterface oi = new OrderInterface();
                                        List<int> orders = oi.GetOrdersForQuoteLine(server, database, db_username, db_password, quotenum, ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine);
                                        string order_list = "";
                                        foreach (int o in orders) 
                                            order_list += order_list.Length > 0 ? ", " + o.ToString() : o.ToString();
                                        SQLAccess.SendMail(server, database, db_username, db_password, "bridge_problem@styleline.com", "Quote #" + quotenum.ToString() + " Line #" + ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine.ToString() + " reconfigured", "Quote #" + quotenum.ToString() + " Line #" + ((QuoteDataSet.QuoteDtlRow)ds.QuoteDtl.Rows[i]).QuoteLine.ToString() + " needs deletion but is on sales order(s) " + order_list + ".  Please cancel any related jobs and delete the associated order lines.");
                                    }
                                    catch (Exception ex1)
                                    {
                                        throw new Exception("Deletion notice:PrepareReconfig - " + ex1.Message + " Line #" + quoteline);
                                    }
                                }
                                else
                                    throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("PrepareReconfig - " + ex.Message + quoteline + reason);
            }
            finally
            {
                CloseSession();
            }
        }*/

        public void DeleteLineItems(string server, string port, string username, string password, int quotenum)
        {
            try
            {
                PartInterface partInterface = new PartInterface();

                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteDataSet ds = BusinessObject.GetByID(quotenum);
                while (ds.QuoteDtl.Count > 0)
                {
                    foreach (QuoteDataSet.QuoteMscRow row in ds.QuoteMsc.Rows)
                        if (row.QuoteLine == ds.QuoteDtl[0].QuoteLine)
                            row.Delete();
                    foreach (QuoteDataSet.QtmmkupRow row in ds.Qtmmkup.Rows)
                        if (row.QuoteLine == ds.QuoteDtl[0].QuoteLine)
                            row.Delete();
                    foreach (QuoteDataSet.QuoteQtyRow row in ds.QuoteQty.Rows)
                        if (row.QuoteLine == ds.QuoteDtl[0].QuoteLine)
                            row.Delete();

                    int quoteNum = quotenum;
                    int lineNum = ds.QuoteDtl[0].QuoteLine;
                    string partnum = ds.QuoteDtl[0].PartNum;

                    ds.QuoteDtl[0].Delete();
                    BusinessObject.Update(ds);

                    if (partInterface.PartIsSalesKit(server, port, username, password, partnum))
                        BusinessObject.RemoveKitComponents(partnum, quotenum, lineNum, ds);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteLineItems - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        #endregion

        #endregion

        /// TODO: Figureout fix or exclusion
/*        private bool CompareLine(QuoteDataSet.QuoteDtlRow quoteline, QuoteLine railsline, QuoteDataSet ds, ConfigurationInterface configInterface, string server, string port, string username, string password, ref string reason)
        {
            bool match = true;

            if (quoteline.DiscountPercent != railsline.Discount)
            {
                match = false;
                reason += "Rails Line " + railsline.ToString() + " Mismatch: Discount Percent " + railsline.Discount.ToString() + "(Rails) vs " + quoteline.DiscountPercent.ToString() + "(Vantage), ";
            }
            if (!railsline.IsConfig && !railsline.IsFauxConfig && quoteline.LineDesc.Replace("\n", "").Replace("\r", "") != railsline.Description.Replace("\n", "").Replace("\r", ""))
            {
                match = false;
                reason += "Rails Line " + railsline.ToString() + " Mismatch: Line Description '" + railsline.Description + "'(Rails) vs '" + quoteline.LineDesc + "'(Vantage), ";
            }
            if (quoteline.OrderQty != railsline.Quantity)
            {
                match = false;
                reason += "Rails Line " + railsline.ToString() + " Mismatch: Quantity " + railsline.Quantity.ToString() + "(Rails) vs " + quoteline.OrderQty.ToString() + "(Vantage), ";
            }

            if (match)
            {
                foreach (QuoteDataSet.QuoteQtyRow qtyrow in ds.QuoteQty.Rows)
                    if (qtyrow.RowState != DataRowState.Deleted && qtyrow.QuoteLine == quoteline.QuoteLine && qtyrow.DocUnitPrice != railsline.Price)
                    {
                        match = false;
                        reason += "Rails Line " + railsline.ToString() + " Mismatch: Price " + qtyrow.DocUnitPrice.ToString() + "(Vantage) vs " + railsline.Price.ToString() + "(Rails), ";
                    }
            }
            if (match && String.IsNullOrEmpty(railsline.PriceAdj_Code))
            {
                if (quoteline.GetQuoteMscRows().Length != 0)
                {
                    match = false;
                    reason += "Rails Line " + railsline.ToString() + " Mismatch: Misc Charge in Vantage converting to MATRIX Adjustment, ";
                }
            }
            else if (match && railsline.PriceAdj_Code != "MATRIX")
            {
                if (quoteline.GetQuoteMscRows().Length != 1)
                {
                    match = false;
                    reason += "Rails Line " + railsline.ToString() + " Mismatch: New Misc Charge, ";
                }
                else
                {
                    QuoteDataSet.QuoteMscRow mscrow = quoteline.GetQuoteMscRows()[0];
                    if (mscrow.MiscCode != railsline.PriceAdj_Code)
                    {
                        match = false;
                        reason += "Rails Line " + railsline.ToString() + " Mismatch: Misc Charge Code '" + railsline.PriceAdj_Code + "'(Rails) vs '" + mscrow.MiscCode + "'(Vantage), ";
                    }
                    if (mscrow.DocMiscAmt != railsline.PriceAdj)
                    {
                        match = false;
                        reason += "Rails Line " + railsline.ToString() + " Mismatch: Misc Charge Amt " + railsline.PriceAdj.ToString() + "(Rails) vs " + mscrow.DocMiscAmt + "(Vantage), ";
                    }
                    if (mscrow.Description.Trim() != railsline.PriceAdj_Desc.Trim())
                    {
                        match = false;
                        reason += "Rails Line " + railsline.ToString() + " Mismatch: Misc Charge Description '" + railsline.PriceAdj_Desc.Trim() + "'(Rails) vs '" + mscrow.Description.Trim() + "'(Vantage), ";
                    }
                }
            }

            if (match && railsline.IsConfig)
            {
                Dictionary<string, string> savedConfig = configInterface.GetConfigValues(server, port, username, password, quoteline.QuoteNum, quoteline.QuoteLine, quoteline.PartNum, quoteline.RevisionNum);
                foreach (string key in railsline.ConfigValues.Keys)
                {
                    if (!savedConfig.ContainsKey(key))
                    {
                        match = false;
                        reason += "Rails Line " + railsline.ToString() + " Mismatch: New Configuration Value '" + key + "', ";
                        break;
                    }
                    else
                    {
                        if (railsline.ConfigValues[key] != savedConfig[key])
                        {
                            if (key == "P01_DEC_QUOTELINE")
                            {
                                if (railsline.parentLine.lineNum.ToString() != savedConfig[key])
                                {
                                    match = false;
                                    reason += "Rails Line " + railsline.ToString() + " Mismatch: Changed Configuration Value " + key + ":'" + railsline.ConfigValues[key] + "'(Rails) vs '" + savedConfig[key] + "'(Vantage), ";
                                    break;
                                }
                            }
                            else
                            {
                                match = false;
                                reason += "Rails Line " + railsline.ToString() + " Mismatch: Changed Configuration Value " + key + ":'" + railsline.ConfigValues[key] + "'(Rails) vs '" + savedConfig[key] + "'(Vantage), ";
                                break;
                            }
                        }
                    }
                }
            }

            if (match && railsline.parentLine != null && railsline.parentLine.Process)
            {
                match = false;
                reason += "Rails Line " + railsline.ToString() + " Block 11 Mismatch, ";
            }

            return match;
        }*/
    }
}
