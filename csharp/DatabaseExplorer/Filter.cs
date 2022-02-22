using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseExplorer
{
    static class Filter
    {
        static private List<string> _filter = new List<string>
        {
            "company", "PROGRESS_RECID", "PROGRESS_RECID_IDENT_", "description", "commenttext", "custnum", "invoicecomment", "invoicedate",
            "linedesc", "changedate", "changedby", "changetime", "minunitcost", "advpaychart", "advpaydept", "advpaydiv", "vendornum",
            "advancepayamt", "advgainloss", "assemblyseq", "camatchchart", "camatchdept", "camatchdiv", "costpercode", "docadvancepayamt",
            "docadvancepayamt", "docextcost", "doclinediscamt", "doctotalmiscchrg", "docunitcost", 

        };

        static private List<string> _tablefilter = new List<string>
        {
            "_tcg_order_notifications", "_tcg_cpartnum", "_tcg_batch_jobs"
        };

        static public string GetTableFilter()
        {
            string r = "";
            foreach (string f in ConfigurationManager.AppSettings["tablefilter"].Split(','))
            {
                if (r.Length > 0)
                    r += ", ";
                r += "'" + f + "'";
            }
            return r;

        }

        static public string GetFilter()
        {
            string r = "";
            foreach(string f in ConfigurationManager.AppSettings["filter"].Split(','))
            {
                if (r.Length > 0)
                    r += ", ";
                r += "'" + f + "'";
            }
            return r;
        }
    }
}
