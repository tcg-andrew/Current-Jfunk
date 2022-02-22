#region Usings

using Epicor.Mfg.BO;
using Epicor.Mfg.Core;
using System;
using System.Xml;

#endregion

namespace ObjectLibrary
{
    public class QuoteAssemblyInterface : EpicorExtension<QuoteAsm>
    {
        #region Public Methods

        #region Create Methods

        public void CreateFromXml(string server, string port, string username, string password, int quotenum, int linenum, string partnum, XmlQuoteParse xml, string db, string dbuser, string dbpass)
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

                foreach (XmlQuoteAsm asm in xml.Assemblies)
                {
                    if (asm.Template == "")
                    {
                        asm.AsmSeq = AddQuoteAsm(quotenum, linenum, asm.Name, asm.Qty, asm.Description, asm.Number05, asm.Number06, asm.Shortchar01, asm.AsAsm);
                        foreach (XmlQuoteOpr opr in asm.Operations)
                        {
                            int oprseq = AddQuoteOpr(quotenum, linenum, opr.Name, asm.AsmSeq, OprTimesInterface.GetOprTime(xml.ProdCode, asm.Number05, xml.Locks, opr.Name + OprTimesInterface.GetSegment(asm.Name)));
                            foreach (XmlQuoteMtl mtl in opr.Materials)
                            {
                                if (mtl.Qty > 0)
                                {
                                    try
                                    {
                                        if (mtl.PhantomBom)
                                        {
                                            PartInterface partInterface = new PartInterface();
                                            foreach (XmlQuoteMtl submtl in partInterface.GetPartMtls(server, db, dbuser, dbpass, "CRD", mtl.Name, mtl.Qty))
                                            {
                                                Part p = partInterface.GetPartInfo(server, port, username, password, submtl.Name);
                                                if (p.PartNum == submtl.Name)
                                                    AddQuoteMtl(quotenum, linenum, submtl.Name, asm.AsmSeq, oprseq, submtl.Qty, p, submtl.AsAsm);
                                            }
                                        }
                                        else
                                        {
                                            PartInterface partInterface = new PartInterface();
                                            Part p = partInterface.GetPartInfo(server, port, username, password, mtl.Name);
                                            if (p.PartNum == mtl.Name)
                                                AddQuoteMtl(quotenum, linenum, mtl.Name, asm.AsmSeq, oprseq, mtl.Qty, p, mtl.AsAsm);
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
                foreach (XmlQuoteAsm asm in xml.Assemblies)
                {
                    if (asm.Template == "")
                    {
                        foreach (XmlQuoteMtl mtl in asm.SubAssemblies)
                        {
                            PartInterface partInterface = new PartInterface();
                            Part p = partInterface.GetPartInfo(server, port, username, password, mtl.Name);
                            AddQuoteSubAsm(quotenum, linenum, asm.AsmSeq, mtl.Name, p.RevNum, (int)mtl.Qty, p.Desc, 0, 0, "");
                        }
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
                BusinessObject.GetDetails(quotenum, linenum, 0, "Method", 0, 0, "", 0, partnum, revision, "", false, false, out outstr);
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

        public int AddQuoteAsm(int quotenum, int linenum, string asm, int qty, string desc, int number05, int number06, string shortchar01, bool asasm)
        {
            int asmseq = 0;
            try
            {
                string outstr;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, 0, 0, true);
                BusinessObject.GetNewQuoteAsm(ds, quotenum, linenum);
                QuoteAsmDataSet.QuoteAsmRow asmrow = ((QuoteAsmDataSet.QuoteAsmRow)ds.QuoteAsm.Rows[ds.QuoteAsm.Rows.Count - 1]);
                asmrow.PartNum = asm;
                asmrow.QtyPer = qty;
                asmrow.BomLevel = 1;
                asmrow.Number05 = number05;
                asmrow.Number06 = number06;
                asmrow.ShortChar01 = shortchar01;
                BusinessObject.GetAsmPartInfo(ds, true);
                if (!String.IsNullOrEmpty(desc))
                {
                    asmrow.Description = desc;
//                    BusinessObject.Update(ds);
                }
                BusinessObject.Update(ds);
                asmseq = asmrow.AssemblySeq;
                if (asasm)
                {
                    string errors;
                    BusinessObject.GetDetails(quotenum, linenum, asmseq, "Method", 0, 0, "", 0, asm, "R0", "", false, false, out errors);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AddQuoteAsm (" + quotenum + ") (" + linenum + ") - " + ex.Message);
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
                asmrow.Number05 = number05;
                asmrow.Number06 = number06;
                asmrow.ShortChar01 = shortchar01;
                BusinessObject.GetAsmPartInfo(ds, true);
                if (!String.IsNullOrEmpty(desc))
                    asmrow.Description = desc;
                BusinessObject.Update(ds);
                asmseq = asmrow.AssemblySeq;
                BusinessObject.GetDetails(quotenum, linenum, asmseq, "Method", 0, 0, "", 0, asm, rev, "", false, false, out outstr);
            }
            catch (Exception ex)
            {
                throw new Exception("AddQuoteSubAsm (" + quotenum + ") (" + linenum + ") - part " + asm + " - " + ex.Message);
            }
            return asmseq;
        }

        public int AddQuoteOpr(int quotenum, int linenum, string opcode, int asmseq, decimal optime = 0)
        {
            int opseq = 0;
            try
            {
                string outstr;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, asmseq, asmseq, true);
                BusinessObject.GetNewOperation(ds, quotenum, linenum, asmseq, false);
                BusinessObject.GetOprOpCodeInfo(opcode, out outstr, ds);
                QuoteAsmDataSet.QuoteOprRow row = ((QuoteAsmDataSet.QuoteOprRow)ds.QuoteOpr.Rows[ds.QuoteOpr.Rows.Count - 1]);
                row.ProdStandard = optime;
                BusinessObject.Update(ds);
                opseq = ((QuoteAsmDataSet.QuoteOprRow)ds.QuoteOpr.Rows[ds.QuoteOpr.Rows.Count - 1]).OprSeq;
            }
            catch (Exception ex)
            {
                throw new Exception("CreateQuoteAsm (" + quotenum + ") (" + linenum + ") - " + ex.Message);
            }
            return opseq;
        }

        public void AddQuoteMtl(int quotenum, int linenum, string partnum, int asm, int op, decimal qty, Part part, bool as_asm)
        {
            try
            {
                string outstr;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, asm, asm, true);
                BusinessObject.GetNewQuoteMtl(ds, quotenum, linenum, asm);
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).PartNum = partnum;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).Description = part.Desc;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).Class = part.Class;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).ClassDescription = part.ClassDesc;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).BuyIt = false;
                BusinessObject.GetMtlPartInfo(ds, partnum);
                BusinessObject.GetMtlOprInfo(op, ds);
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).QtyPer = qty;
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

                string outstr;
                QuoteAsmDataSet ds = BusinessObject.GetDatasetForTree(quotenum, linenum, 0, 0, true);
                BusinessObject.GetNewQuoteMtl(ds, quotenum, linenum, asm);
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).PartNum = partnum;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).Description = part.Desc;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).Class = part.Class;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).ClassDescription = part.ClassDesc;
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).BuyIt = false;
                BusinessObject.GetMtlPartInfo(ds, partnum);
                BusinessObject.GetMtlOprInfo(op, ds);
                ((QuoteAsmDataSet.QuoteMtlRow)ds.QuoteMtl.Rows[ds.QuoteMtl.Rows.Count - 1]).QtyPer = qty;
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
