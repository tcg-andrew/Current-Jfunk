using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epicor.Mfg.Core;
using Epicor.Mfg.BO;

namespace ObjectLibrary
{
    public class EngWorkbenchInterface : EpicorExtension<Epicor.Mfg.BO.EngWorkBench>
    {
        public void AddMaterial(string server, string port, string username, string password, string part, string rev, string addpart, string addop, string addqty)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

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

        public void RemoveMaterial(string server, string port, string username, string password, string part, string rev, string mtlseq, string mtlpartnum)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

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

        public void RemoveOperation(string server, string port, string username, string password, string part, string rev, string opseq)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                string outrev;
                string msg;
                bool flag;

                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                foreach (EngWorkBenchDataSet.ECOOprRow row in ds.ECOOpr.Rows)
                {
                    if (row.OprSeq == Int32.Parse(opseq))
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

        public void ReplaceMaterial(string server, string port, string username, string password, string part, string rev, string mtlseq, string oldmtlpartnum, string newmtlpartnum, bool bypassseqcheck = false)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

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

        public void ReviseMaterial(string server, string port, string username, string password, string part, string rev, string mtlseq, string mtlpartnum, string newqty, bool bypassseqcheck = false)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                string outrev;
                string msg;
                bool flag;

                EngWorkBenchDataSet ds = BusinessObject.CheckOut("serv", part, rev, "", DateTime.Today, false, true, false, true, false, out outrev, out msg, out flag);
                BusinessObject.GroupLock("serv");
                ds = BusinessObject.GetECORevData("serv", true);
                ds = BusinessObject.GetDatasetForTree("serv", part, rev, "", DateTime.Today, false, false);
                foreach (EngWorkBenchDataSet.ECOMtlRow row in ds.ECOMtl.Rows)
                {
                    if (row.MtlPartNum == mtlpartnum && (bypassseqcheck || (!bypassseqcheck && row.MtlSeq == Int32.Parse(mtlseq))))
                        row.QtyPer = Decimal.Parse(newqty);
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
    }
}
