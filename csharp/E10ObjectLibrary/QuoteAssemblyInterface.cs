#region Usings

using System;
using System.Xml;
using Erp.Proxy.BO;
using Erp.BO;
using Ice.Core;
using Erp.Contracts;
using ObjectLibrary;

#endregion

namespace ObjectLibrary
{
    public class QuoteAssemblyInterface : EpicorExtension<QuoteAsmImpl, QuoteAsmSvcContract>
    {
        #region Public Methods

        #region Create Methods

        public void CreateFromXml(string server, string port, string username, string password, int quotenum, int linenum, string partnum, XmlQuoteParse xml, string dbserver, string db, string dbuser, string dbpass)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                /*                if (!String.IsNullOrEmpty(xml.Description))
                                {
                                    QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, 0, 0, true);
                                    ((QuoteAsmDataSet.QuoteAsmRow)ds.QuoteAsm.Rows[ds.QuoteAsm.Rows.Count - 1]).Description = xml.Description;
                                    BusinessObject.Update(ds);
                                }*/
                PartInterface pi = new PartInterface();
                foreach (XmlQuoteAsm asm in xml.Assemblies)
                {
                    if (asm.Qty > 0)
                    {
                        if (asm.Template == "" || asm.Template == "TC")
                        {
                            asm.AsmSeq = AddQuoteAsm(quotenum, linenum, asm.Name, pi.GetPartInfo(server, port, username, password, asm.Name).RevNum, asm.Qty, asm.Description, asm.Number05, asm.Number06, asm.Shortchar01, (asm.AsAsm || asm.Template == "TC"));
                            foreach (XmlQuoteOpr opr in asm.Operations)
                            {
                                OprTimeDetail detail = OprTimesInterface.GetDetail(asm.Name);
                                int openings = 0;
                                string suffix = "";
                                if (detail != null)
                                {
                                    openings = detail.Openings;
                                    suffix = detail.Suffix;
                                }

                                int oprseq = AddQuoteOpr(quotenum, linenum, opr.Name, asm.AsmSeq, OprTimesInterface.GetOprTime(xml.ProdCode, asm.Number05, xml.Locks.Contains("Yes"), opr.Name + suffix));
                                opr.Seq = oprseq;
                                foreach (XmlQuoteMtl mtl in opr.Materials)
                                {
                                    if (mtl.Qty > 0)
                                    {
                                        try
                                        {
                                            if (mtl.PhantomBom)
                                            {
                                                PartInterface partInterface = new PartInterface();
                                                foreach (XmlQuoteMtl submtl in partInterface.GetPartMtls(dbserver, db, dbuser, dbpass, "CRD", mtl.Name, mtl.Qty))
                                                {
                                                    try
                                                    {
                                                        Part p = partInterface.GetPartInfo(server, port, username, password, submtl.Name);
                                                        if (p.PartNum == submtl.Name)
                                                            AddQuoteMtl(quotenum, linenum, submtl.Name, asm.AsmSeq, oprseq, submtl.Qty, p, submtl.AsAsm, 0);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        if (ex.Message != "GetPartDescription - " + submtl.Name + " - Record not found.")
                                                            throw ex;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                PartInterface partInterface = new PartInterface();
                                                try
                                                {
                                                    Part p = partInterface.GetPartInfo(server, port, username, password, mtl.Name);
                                                    if (p.PartNum == mtl.Name)
                                                        AddQuoteMtl(quotenum, linenum, mtl.Name, asm.AsmSeq, oprseq, mtl.Qty, p, mtl.AsAsm, mtl.Scrap);
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.Message != "GetPartDescription - " + mtl.Name + " - Record not found.")
                                                        throw ex;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (XmlQuoteAsm asm in xml.Assemblies)
                {
                    if (asm.Template == "" || asm.Template == "TC")
                    {
                        foreach (XmlQuoteMtl mtl in asm.SubAssemblies)
                        {
                            PartInterface partInterface = new PartInterface();
                            try
                            {
                                if (asm.Template == "TC")
                                {
                                    Part a = partInterface.GetPartInfo(server, port, username, password, asm.Name);
                                    Part p = partInterface.GetPartInfo(server, port, username, password, mtl.Name);

                                    AddQuoteMtl(quotenum, linenum, p.PartNum, asm.AsmSeq, partInterface.GetPartOperations(dbserver, db, dbuser, dbpass, asm.Name, a.RevNum).Find(opr => opr.OpCode == p.PhantomOpr).Seq, mtl.Qty, p, false, 0);
                                }
                                else
                                {
                                    Part p = partInterface.GetPartInfo(server, port, username, password, mtl.Name);
                                    AddQuoteSubAsm(quotenum, linenum, asm.AsmSeq, mtl.Name, p.RevNum, (int)mtl.Qty, p.Desc, 0, 0, "");
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message != "GetPartDescription - " + mtl.Name + " - Record not found.")
                                    throw ex;
                            }
                        }
                    }
                }

                foreach (XmlQuoteAsm asm in xml.Assemblies)
                {
                    foreach (string del in asm.Replacements.Keys)
                    {
                        PartInterface partInterface = new PartInterface();
                        foreach (XmlQuoteMtl delmtl in partInterface.GetPartMtls(dbserver, db, dbuser, dbpass, "CRD", del, 1))
                        {
                            if (delmtl.AsAsm)
                            {
                                RemoveQuoteSubAsm(quotenum, linenum, delmtl.Name, delmtl.Qty);
                            }
                            else
                            {
                                RemoveQuoteMtl(quotenum, linenum, asm.AsmSeq, delmtl.Name, delmtl.Qty);
                            }
                        }
                        Part p = partInterface.GetPartInfo(server, port, username, password, asm.Replacements[del]);
                        AddQuoteSubAsm(quotenum, linenum, asm.AsmSeq, p.PartNum, p.RevNum, 1, p.Desc, 0, 0, "");

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateFromXml (" + quotenum + ") (" + linenum + ") - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }

/*            foreach (string op in line.MOM.Keys)
            {
                int opseq = quoteAssemblyInterface.AddQuoteOpr(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, op);
                foreach (string part in line.MOM[op].Keys)
                {
                    Part p = partInterface.GetPartInfo(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, part);
                    quoteAssemblyInterface.AddQuoteMtl(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, part, opseq, line.MOM[op][part], p);
                }
            }*/
        
        }

        public void CreateQuoteAsm(string server, string port, string username, string password, int quotenum, int linenum, string partnum, string revision)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                string outstr;
                BusinessObject.GetDetails(quotenum, linenum, 0, "Method", 0, 0, "", 0, partnum, revision, "", false, false, false, out outstr);
            }
            catch (Exception ex)
            {
                throw new Exception("CreateQuoteAsm (" + quotenum + ") (" + linenum + ") " + partnum + " (rev " + revision +") - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        public int AddQuoteAsm(int quotenum, int linenum, string asm, string rev, int qty, string desc, int number05, int number06, string shortchar01, bool asasm)
        {
            int asmseq = 0;
            try
            {
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, 0, 0, true);
                BusinessObject.GetNewAssembly(ds, quotenum, linenum, 0, 0, 0);
//                BusinessObject.GetNewQuoteAsm(ds, quotenum, linenum);
//                BusinessObject.Update(ds);
                QuoteAsmDataSet.QuoteAsmRow asmrow = ((QuoteAsmDataSet.QuoteAsmRow)ds.QuoteAsm.Rows[ds.QuoteAsm.Rows.Count - 1]);
                asmrow.PartNum = asm;
                asmrow.QtyPer = qty;
                asmrow.BomLevel = 1;
                asmrow["number05"] = number05;
                asmrow["number06"] = number06;
                asmrow["shortchar01"] = shortchar01;
                BusinessObject.GetAsmPartInfo(ds, true);
                BusinessObject.Update(ds);
                if (!String.IsNullOrEmpty(desc) && desc != "Missing kit assembly")
                {
                    asmrow.Description = desc;
                    BusinessObject.Update(ds);
                }
                asmseq = asmrow.AssemblySeq;
                if (asasm)
                {
                    string errors;
                    BusinessObject.GetDetails(quotenum, linenum, asmseq, "Method", 0, 0, "", 0, asm, rev, "", false, false, false, out errors);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("AddQuoteAsm (" + quotenum + ") (" + linenum + ") - (" + asm + ") " + ex.Message);
            }
            return asmseq;
        }

        public int AddQuoteSubAsm(int quotenum, int linenum, int parentasmseq, string asm, string rev, int qty, string desc, int number05, int number06, string shortchar01)
        {
            int asmseq = 0;
            try
            {
                string outstr;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, parentasmseq, 0, false);
                BusinessObject.GetNewAssembly(ds, quotenum, linenum, parentasmseq, 1, 0);
//                BusinessObject.GetNewQuoteAsm(ds, quotenum, linenum);
                QuoteAsmDataSet.QuoteAsmRow asmrow = ((QuoteAsmDataSet.QuoteAsmRow)ds.QuoteAsm.Rows[ds.QuoteAsm.Rows.Count - 1]);
                asmrow.PartNum = asm;
                asmrow.QtyPer = qty;
                asmrow.Parent = parentasmseq;
                asmrow["number05"] = number05;
                asmrow["number06"] = number06;
                asmrow["shortchar01"] = shortchar01;
                BusinessObject.GetAsmPartInfo(ds, true);
                if (!String.IsNullOrEmpty(desc) && desc != "Missing kit assembly")
                    asmrow.Description = desc;
                BusinessObject.Update(ds);
                asmseq = asmrow.AssemblySeq;
                BusinessObject.GetDetails(quotenum, linenum, asmseq, "Method", 0, 0, "", 0, asm, rev, "", false, false, false, out outstr);
            }
            catch (Exception ex)
            {
                throw new Exception("AddQuoteSubAsm (" + quotenum + ") (" + linenum + ") - part " + asm + " - " + ex.Message);
            }
            return asmseq;
        }

        public void RemoveQuoteSubAsm(int quotenum, int linenum, string asm, decimal qty)
        {
            try
            {
                decimal rmqty = qty;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, 0, 0, false);
                foreach (QuoteAsmDataSet.QuoteAsmRow asmrow in ds.QuoteAsm.Rows)
                {
                    if (rmqty > 0 && asmrow.PartNum == asm && asmrow.BomLevel > 1)
                    {
                        if (asmrow.QtyPer >= rmqty)
                        {
                            asmrow.QtyPer -= rmqty;
                            rmqty = 0;
                            if (asmrow.QtyPer == 0)
                                asmrow.Delete();
                        }
                        else
                        {
                            rmqty -= asmrow.QtyPer;
                            asmrow.Delete();
                        }
                    }
                }
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("RemoveQuoteSubAsm (" + quotenum + ") (" + linenum + ") - part " + asm + " - " + ex.Message);
            }
        }

        public int AddQuoteOpr(int quotenum, int linenum, string opcode, int asmseq, decimal prodtime)
        {
            int opseq = 0;
            try
            {
                string outstr;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, asmseq, asmseq, true);
                BusinessObject.GetNewOperation(ds, quotenum, linenum, asmseq, false);
                BusinessObject.GetOprOpCodeInfo(opcode, out outstr, ds);
                ((QuoteAsmDataSet.QuoteOprRow)ds.QuoteOpr.Rows[ds.QuoteOpr.Rows.Count - 1]).ProdStandard = prodtime;
                if (prodtime > 0)
                    ((QuoteAsmDataSet.QuoteOprRow)ds.QuoteOpr.Rows[ds.QuoteOpr.Rows.Count - 1]).LaborEntryMethod = "Q";
                BusinessObject.Update(ds);
                opseq = ((QuoteAsmDataSet.QuoteOprRow)ds.QuoteOpr.Rows[ds.QuoteOpr.Rows.Count - 1]).OprSeq;
            }
            catch (Exception ex)
            {
                throw new Exception("CreateQuoteAsm (" + quotenum + ") (" + linenum + ") - " + ex.Message);
            }
            return opseq;
        }

        public void RemoveQuoteMtl(int quotenum, int linenum, int asm, string partnum, decimal qty)
        {
            try
            {
                decimal rmqty = qty;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, asm, asm, true);
                foreach (QuoteAsmDataSet.QuoteMtlRow row in ds.QuoteMtl.Rows)
                {
                    if (rmqty > 0 && row.PartNum == partnum)
                    {
                        if (row.QtyPer >= rmqty)
                        {
                            row.QtyPer -= rmqty;
                            rmqty = 0;
                            if (row.QtyPer == 0)
                                row.Delete();
                        }
                        else
                        {
                            rmqty -= row.QtyPer;
                            row.Delete();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("RemoveQuoteAsm (" + quotenum + ") (" + linenum + ") - " + ex.Message);
            }
        }

        public void AddQuoteMtl(int quotenum, int linenum, string partnum, int asm, int op, decimal qty, Part part, bool as_asm, decimal scrap)
        {
            try
            {
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, asm, asm, true);
                BusinessObject.GetNewQuoteMtl(ds, quotenum, linenum, asm);
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).PartNum = partnum;
                if (!String.IsNullOrEmpty(part.Desc) && part.Desc != "Missing kit assembly")
                    ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).Description = part.Desc;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).Class = part.Class;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).ClassDescription = part.ClassDesc;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).BuyIt = false;
                BusinessObject.GetMtlPartInfo(ds, partnum);
                BusinessObject.GetMtlOprInfo(op, ds);
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).QtyPer = qty;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).IUM = part.Unit;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).EstScrap = scrap;
                BusinessObject.ChangeOpMtlReqQty(ds);
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("CreateQuoteAsm (" + quotenum + ") (" + linenum + ") - " + ex.Message);
            }
        }

        public int AddQuoteOpr(string server, string port, string username, string password, int quotenum, int linenum, string opcode)
        {
            int opseq = 0;
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                
                string outstr;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, 0, 0, true);
                BusinessObject.GetNewOperation(ds, quotenum, linenum, 0, false);
                BusinessObject.GetOprOpCodeInfo(opcode, out outstr, ds);
                BusinessObject.Update(ds);
                opseq = ((QuoteAsmDataSet.QuoteOprRow)ds.QuoteOpr.Rows[ds.QuoteOpr.Rows.Count - 1]).OprSeq;
            }
            catch (Exception ex)
            {
                throw new Exception("CreateQuoteAsm (" + quotenum + ") (" + linenum + ") - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return opseq;
        }

        public void AddQuoteMtl(string server, string port, string username, string password, int quotenum, int linenum, string partnum, int asm, int op, decimal qty, Part part)
        {
            try
            {
                
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, 0, 0, true);
                BusinessObject.GetNewQuoteMtl(ds, quotenum, linenum, asm);
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).PartNum = partnum;
                if (!String.IsNullOrEmpty(part.Desc) && part.Desc != "Missing kit assembly")
                    ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).Description = part.Desc;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).Class = part.Class;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).ClassDescription = part.ClassDesc;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).BuyIt = false;
                BusinessObject.GetMtlPartInfo(ds, partnum);
                BusinessObject.GetMtlOprInfo(op, ds);
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).QtyPer = qty;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).IUM = part.Unit;
                BusinessObject.ChangeOpMtlReqQty(ds);
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("CreateQuoteAsm (" + quotenum + ") (" + linenum + ") - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }


        #endregion

        #endregion
    }
}
