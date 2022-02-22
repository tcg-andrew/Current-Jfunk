using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epicor.Mfg.BO;
using Epicor.Mfg.Core;

namespace ObjectLibrary
{
    public class IssueReturnInterface : EpicorExtension<IssueReturn>
    {
        public void IssueMaterial(string company, string jobnum, int assemblyseq, int mtlseq, int qty)
        {
            try
            {
                SelectedJobAsmblDataSet jds = new SelectedJobAsmblDataSet();
                SelectedJobAsmblDataSet.SelectedJobAsmblRow jrow = jds.SelectedJobAsmbl.NewSelectedJobAsmblRow();
                jrow.JobNum = jobnum;
                jrow.Company = company;
                jrow.AssemblySeq = assemblyseq;
                jds.SelectedJobAsmbl.AddSelectedJobAsmblRow(jrow);
                string pcmessage;
                IssueReturnDataSet ds = BusinessObject.GetNewJobAsmblMultiple("STK-MTL", "?", jds, out pcmessage);

                IssueReturnDataSet.IssueReturnRow irrow = ds.IssueReturn.Rows[0] as IssueReturnDataSet.IssueReturnRow;
                irrow.ToJobSeq = mtlseq;
                irrow.IssuedComplete = true;
                irrow.TranQty = qty;

                string negqty;
                BusinessObject.NegativeInventoryTest(irrow.PartNum, irrow.FromWarehouseCode, irrow.FromBinNum, irrow.LotNum, irrow.DimCode, irrow.DimConvFactor, irrow.TranQty, out negqty, out pcmessage);
                BusinessObject.PerformMaterialMovement(Boolean.Parse(negqty), ds, out pcmessage);
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
    }
}
