using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erp.Proxy.BO;
using Erp.BO;
using Ice.Core;
using Erp.Contracts;
using static ObjectLibrary.TCBridgeInterface;
using ObjectLibrary;

namespace ObjectLibrary
{
    public class EngWorkbenchInterface : EpicorExtension<EngWorkBenchImpl, EngWorkBenchSvcContract>
    {
        public void CreateTCRev(string server, string database, string username, string password, PartInfo part)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);

                string eco = "serv";

                string outrev;
                string msg;
                bool flag;

                /// Change ECO to tcserv
                EngWorkBenchDataSet ds = BusinessObject.CheckOut(eco, part.PartNum, part.RevNum, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock(eco);
                ds = BusinessObject.GetDatasetForTree(eco, part.PartNum, part.RevNum, "", DateTime.Today, false, false);

                try
                {
                    if (!part.PhantomBOM)
                    {
                        foreach (string opr in part.RevBoo)
                        {
                            BusinessObject.GetNewECOOpr(ds, eco, part.PartNum, part.RevNum, "");
                            if (opr == "CIGSUBCO" || opr == "SUBCONLB")
                            {
                                ((EngWorkBenchDataSet.ECOOprRow)ds.ECOOpr.Rows[0]).SubContract = true;
                                BusinessObject.EcoOprInitSNReqSubConShip(ds);
                            }

                            try
                            {
                                string oprmsg = "";
                                BusinessObject.ChangeECOOprOpCode(opr, out oprmsg, ds);
                                BusinessObject.Update(ds);
                                if (oprmsg.Length > 0)
                                {
                                    throw new DataException("EngWorkbenchInterface.CreateTCRev: Invalid OPCODE \"" + opr + "\".");
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                            try
                            {
                                OprTimeDetail detail = OprTimesInterface.GetDetail(part.PartNum);
                                int openings = 0;
                                string suffix = "";
                                if (detail != null)
                                {
                                    openings = detail.Openings;
                                    suffix = detail.Suffix;
                                }
                                ((EngWorkBenchDataSet.ECOOprRow)ds.ECOOpr.Rows[0]).ProdStandard = OprTimesInterface.GetOprTime(part.ProdCode, openings, false, opr + suffix);
                                if (((EngWorkBenchDataSet.ECOOprRow)ds.ECOOpr.Rows[0]).ProdStandard > 0)
                                    ((EngWorkBenchDataSet.ECOOprRow)ds.ECOOpr.Rows[0]).LaborEntryMethod = "Q";
                                BusinessObject.Update(ds);

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        try
                        {
                            ((EngWorkBenchDataSet.ECORevRow)ds.ECORev.Rows[0]).AutoRecOpr = part.RevBoo.Count * 10;
                            ((EngWorkBenchDataSet.ECORevRow)ds.ECORev.Rows[0]).FinalOpr = part.RevBoo.Count * 10;
                            BusinessObject.Update(ds);
                        }
                        catch (Exception ex)
                        {
                            throw new DataException("EngWorkbenchInterface.CreateTCRev: Error setting Auto Rec Opr or Final Opr: " + ex.Message);
                        }
                    }

                    if (!String.IsNullOrEmpty(part.MakeFrom))
                    {
                        BusinessObject.GetNewECOMtl(ds, eco, part.PartNum, part.RevNum, "");
                        string x, y;
                        try
                        {
                            BusinessObject.CheckECOMtlMtlPartNum(part.MakeFrom, out x, out y, ds);
                            ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).MtlPartNum = part.MakeFrom;
                            BusinessObject.ChangeECOMtlMtlPartNum(ds);
                            BusinessObject.Update(ds);
                        }
                        catch (Exception ex)
                        {
                            throw new MissingPartException("EngWorkbenchInterface.CreateTCRev: Invalid MakeFrom \"" + part.MakeFrom + "\".  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
                        }
                        if (((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).RelatedOperation != 10)
                        {
                            try
                            {
                                ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).RelatedOperation = 10;
                                BusinessObject.ChangeECOMtlRelatedOperation(10, ds);
                                BusinessObject.Update(ds);
                            }
                            catch (Exception ex)
                            {
                                throw new DataException("EngWorkbenchInterface.CreateTCRev: Invalid default OprNum 10.  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
                            }
                        }

                        try
                        {
                            ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).QtyPer = part.MakeFromQty;
                            BusinessObject.Update(ds);
                        }
                        catch (Exception ex)
                        {
                            throw new DataException("EngWorkbenchInterface.CreateTCRev: Invalid MakeFromQty \"" + part.MakeFromQty.ToString() + "\".  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
                        }
                    }
                    else
                    {
                        foreach (BomInfo bom in part.BOM)
                        {
                            string x, y;
                            BusinessObject.GetNewECOMtl(ds, eco, part.PartNum, part.RevNum, "");
                            try
                            {
                                BusinessObject.CheckECOMtlMtlPartNum(bom.PartNum, out x, out y, ds);
                                ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).MtlPartNum = bom.PartNum;
                                BusinessObject.ChangeECOMtlMtlPartNum(ds);
                            }
                            catch (Exception ex)
                            {
                                throw new MissingPartException("EngWorkbenchInterface.CreateTCRev: Invalid or missing MtlPartNum \"" + bom.PartNum + "\".  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
                            }
                            try
                            {
                                if (((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).RelatedOperation != bom.Opr)
                                {
                                    BusinessObject.ChangeECOMtlRelatedOperation(bom.Opr, ds);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new DataException("EngWorkbenchInterface.CreateTCRev: mtl:" + bom.PartNum + " Invalid OprNum \"" + bom.Opr.ToString() + "\".  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
                            }

                            try
                            {
                                ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).QtyPer = bom.Qty;
                                BusinessObject.ChangeECOMtlQtyPer(ds);
                                ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).PullAsAsm = bom.PullAsAsm;
                                ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).ViewAsAsm = bom.ViewAsAsm;
                                BusinessObject.Update(ds);
                            }
                            catch (Exception ex)
                            {
                                throw new DataException("EngWorkbenchInterface.CreateTCRev: mtl:" + bom.PartNum + " Invalid qty \"" + bom.Qty.ToString() + "\", pullasasm \"" + bom.PullAsAsm.ToString() + "\", viewasasm \"" + bom.ViewAsAsm.ToString() + "\", or relatedoperation \"" + bom.Opr.ToString() + "\" (" + part.RevBoo + ").  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    BusinessObject.UndoCheckOut(eco, part.PartNum, part.RevNum, "", DateTime.Today, false, true, true, false, ds);
                    throw ex;
                }
                string result;
                ds = BusinessObject.ApproveAll(eco, part.PartNum, part.RevNum, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll(eco, part.PartNum, part.RevNum, "", DateTime.Today, false, true, true, false, false, "TC-ERP C# Bridge ECO", out result);
                BusinessObject.GroupUnLock(eco, part.PartNum, part.RevNum, "", DateTime.Today, false, false, true, false, ds);
            }
            catch (DataException ex)
            {
                throw ex;
            }
            catch (MissingPartException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("EngWorkbenchInterface.CreateTCRev: Unhandled exception.  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        public void AddMaterial(string server, string database, string username, string password, string part, string rev, string addpart, string addop, string addqty)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);

                string outrev;
                string msg;
                bool flag;

                
                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetECORevData("serv", true);
                BusinessObject.GetNewECOMtl(ds, "serv", part, rev, "");
                ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).MtlPartNum = addpart;
                BusinessObject.ChangeECOMtlMtlPartNum(ds);
                BusinessObject.ChangeECOMtlRelatedOperation(Int32.Parse(addop), ds);
                ((EngWorkBenchDataSet.ECOMtlRow)ds.ECOMtl.Rows[ds.ECOMtl.Rows.Count - 1]).QtyPer = Int32.Parse(addqty);
                BusinessObject.Update(ds);
                string result;
                ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);

                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveMaterial(string server, string database, string username, string password, string part, string rev, string mtlseq, string mtlpartnum)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);

                string outrev;
                string msg;
                bool flag;

                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                foreach (EngWorkBenchDataSet.ECOMtlRow row in ds.ECOMtl.Rows)
                {
                    if (/*row.MtlSeq == Int32.Parse(mtlseq) && */row.MtlPartNum == mtlpartnum)
                        row.Delete();
                }
                BusinessObject.Update(ds);
                string result;
                ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);

                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddOperation(string server, string database, string username, string password, string part, string rev, string opcode)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            string eco = "serv";
            string outrev;
            string msg;
            bool flag;

            EngWorkBenchDataSet ds = BusinessObject.CheckOut(eco, part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
            BusinessObject.GroupLock(eco);
            ds = BusinessObject.GetDatasetForTree(eco, part, rev, "", DateTime.Today, false, false);

            try
            {

                bool found = false;
                foreach (EngWorkBenchDataSet.ECOOprRow op in ds.ECOOpr.Rows)
                {
                    if (op.OpCode.ToLower() == opcode.ToLower())
                        found = true;
                }

                if (!found)
                {
                    BusinessObject.GetNewECOOpr(ds, eco, part, rev, "");
                    string oprmsg = "";
                    BusinessObject.ChangeECOOprOpCode(opcode, out oprmsg, ds);
                    BusinessObject.Update(ds);
                }

                string result;
                ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);
            }
            catch (Exception ex)
            {
                BusinessObject.UndoCheckOut(eco, part, rev, "", DateTime.Today, false, true, true, false, ds);
                throw ex;
            }
            finally
            {
                CloseSession();
            }


        }

        public void RemoveOperation(string server, string database, string username, string password, string part, string rev, string opseq)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);

                string outrev;
                string msg;
                bool flag;
                

                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                foreach (EngWorkBenchDataSet.ECOOprRow row in ds.ECOOpr.Rows)
                {
                    if (row.OprSeq == Int32.Parse(opseq))
                    {
                        row.Delete();
                    }
                }
                BusinessObject.Update(ds);
                string result;
                ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);

                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void OperationProductionTime(string server, string database, string username, string password, string part, string rev, string opseq, decimal time, string format, string eco)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);

                string outrev;
                string msg;
                bool flag;


                EngWorkBenchDataSet ds = BusinessObject.CheckOut(eco, part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock(eco);
                ds = BusinessObject.GetDatasetForTree(eco, part, rev, "", DateTime.Today, false, false);
                foreach (EngWorkBenchDataSet.ECOOprRow row in ds.ECOOpr.Rows)
                {
                    if (row.OprSeq == Int32.Parse(opseq))
                    {
                        row.ProdStandard = time;
                        row.StdFormat = format;
                        if (time > 0)
                            row.LaborEntryMethod = "Q";
                    }
                }
                BusinessObject.Update(ds);
                string result;
                ds = BusinessObject.ApproveAll(eco, part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll(eco, part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                BusinessObject.GroupUnLock(eco, part, rev, "", DateTime.Today, false, false, true, false, ds);

                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ReplaceMaterial(string server, string database, string username, string password, string part, string rev, string mtlseq, string oldmtlpartnum, string newmtlpartnum, bool bypassseqcheck = false)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);

                string outrev;
                string msg;
                bool flag;

                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetECORevData("serv", true);
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                foreach (EngWorkBenchDataSet.ECOMtlRow row in ds.ECOMtl.Rows)
                {
                    if (row.MtlPartNum.ToUpper() == oldmtlpartnum.ToUpper() && (bypassseqcheck || (!bypassseqcheck && row.MtlSeq == Int32.Parse(mtlseq))))
                    {
                        row.MtlPartNum = newmtlpartnum;
                        BusinessObject.ChangeECOMtlMtlPartNum(ds);
                    }
                }
                BusinessObject.Update(ds);
                string result;
                ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);

                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ReviseMaterial(string server, string database, string username, string password, string part, string rev, string mtlseq, string mtlpartnum, string newqty, string newopr, bool bypassseqcheck = false)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            string outrev;
            string msg;
            bool flag;

                       EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                      BusinessObject.GroupLock("serv");
                      ds = BusinessObject.GetECORevData("serv", true);
            ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);

            try
            {
                foreach (EngWorkBenchDataSet.ECOMtlRow row in ds.ECOMtl.Rows)
                {
                    if (row.MtlPartNum == mtlpartnum && (bypassseqcheck || (!bypassseqcheck && row.MtlSeq == Int32.Parse(mtlseq))))
                    {
                        if (!String.IsNullOrEmpty(newqty))
                            row.QtyPer = Decimal.Parse(newqty);
                        if (!String.IsNullOrEmpty(newopr))
                            row.RelatedOperation = Int32.Parse(newopr);
                    }
                }
                BusinessObject.Update(ds);
                string result;
                ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);
            }
            catch (Exception ex)
            {
                BusinessObject.UndoCheckOut("serv", part, rev, "", DateTime.Today, false, true, true, false, ds);
                throw ex;
            }
            finally
            {
                CloseSession();
            }

        }

        public void ReviseMaterial(string server, string database, string username, string password, string part, string rev, string mtlseq, string mtlpartnum, bool as_asm)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            string outrev;
            string msg;
            bool flag;

            try
            {
                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetECORevData("serv", true);
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                try
                {

                    foreach (EngWorkBenchDataSet.ECOMtlRow row in ds.ECOMtl.Rows)
                    {
                        if (row.MtlPartNum == mtlpartnum && row.MtlSeq == Int32.Parse(mtlseq))
                        {
                            row.ViewAsAsm = as_asm;
                            row.PullAsAsm = as_asm;
                        }
                    }
                    BusinessObject.Update(ds);
                    string result;
                    ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                    ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                    BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);
                }
                catch (Exception ex)
                {
                    BusinessObject.UndoCheckOut("serv", part, rev, "", DateTime.Today, false, true, true, false, ds);
                    throw ex;
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

        public void ReviseMaterial(string server, string database, string username, string password, string part, string rev, string mtlpartnum, bool pull, bool view)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            string outrev;
            string msg;
            bool flag;

            try
            {
                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetECORevData("serv", true);
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                try
                {

                    foreach (EngWorkBenchDataSet.ECOMtlRow row in ds.ECOMtl.Rows)
                    {
                        if (row.MtlPartNum == mtlpartnum)
                        {
                            row.ViewAsAsm = view;
                            row.PullAsAsm = pull;
                        }
                    }
                    BusinessObject.Update(ds);
                    string result;
                    ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                    ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                    BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);
                }
                catch (Exception ex)
                {
                    BusinessObject.UndoCheckOut("serv", part, rev, "", DateTime.Today, false, true, true, false, ds);
                    throw ex;
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

        public void ReviseMaterial(string server, string database, string username, string password, string part, string rev, string mtlseq, string mtlpartnum, int scrap)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            string outrev;
            string msg;
            bool flag;

            try
            {
                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetECORevData("serv", true);
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                try
                {

                    foreach (EngWorkBenchDataSet.ECOMtlRow row in ds.ECOMtl.Rows)
                    {
                        if (row.MtlPartNum == mtlpartnum && row.MtlSeq == Int32.Parse(mtlseq))
                        {
                            row.EstScrap = scrap;
                        }
                    }
                    BusinessObject.Update(ds);
                    string result;
                    ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                    ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                    BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);
                }
                catch (Exception ex)
                {
                    BusinessObject.UndoCheckOut("serv", part, rev, "", DateTime.Today, false, true, true, false, ds);
                    throw ex;
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

        public void MigrateMaterials(string server, string database, string username, string password, string part, string rev, string fromoprseq, string tooprseq)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);

                string outrev;
                string msg;
                bool flag;

                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetECORevData("serv", true);
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                foreach (EngWorkBenchDataSet.ECOMtlRow row in ds.ECOMtl.Rows)
                {
                    if (row.RelatedOperation == Int32.Parse(fromoprseq))
                        row.RelatedOperation = Int32.Parse(tooprseq);
                }
                BusinessObject.Update(ds);
                foreach (EngWorkBenchDataSet.ECOOprRow row in ds.ECOOpr.Rows)
                {
                    if (row.OprSeq == Int32.Parse(fromoprseq))
                    {
                        row.Delete();
                    }
                }
                BusinessObject.Update(ds);
                string result;
                ds = BusinessObject.ApproveAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, out result);
                ds = BusinessObject.CheckInAll("serv", part, rev, "", DateTime.Today, false, true, true, false, false, "C# Service ECO", out result);
                BusinessObject.GroupUnLock("serv", part, rev, "", DateTime.Today, false, false, true, false, ds);

                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ClearCheckouts(string server, string database, string username, string password, string eco)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);

                BusinessObject.GroupLock(eco);

                bool cont = true;
                while (cont)
                {
                    EngWorkBenchDataSet ds = BusinessObject.GetECORevData(eco, true);
                    if (ds.ECORev.Rows.Count > 0)
                    {
                        EngWorkBenchDataSet.ECORevRow row = ds.ECORev.Rows[0] as EngWorkBenchDataSet.ECORevRow;
                        BusinessObject.UndoCheckOut(eco, row.PartNum, row.RevisionNum, row.AltMethod, DateTime.Today, false, true, true, false, ds);
                    }
                    else
                        cont = false;
                }

                CloseSession();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
