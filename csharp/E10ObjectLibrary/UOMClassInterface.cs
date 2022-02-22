using Erp.BO;
using Erp.Contracts;
using Erp.Proxy.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary
{
    public class UOMClassInterface : EpicorExtension<UOMClassImpl, UOMClassSvcContract>
    {
        public void Create(string server, string database, string username, string password, string uom)
        {
            try
            {
                decimal u;
                if (Decimal.TryParse(uom, out u))
                {
                    OpenSession(server, database, username, password, Ice.Core.Session.LicenseType.Default);

                    UOMClassDataSet ds = BusinessObject.GetByID("Length");

                    bool found = false;
                    foreach (UOMClassDataSet.UOMConvRow row in ds.UOMConv)
                    {
                        if (row.UOMCode == uom)
                            found = true;
                    }

                    if (!found)
                    {
                        BusinessObject.GetNewUOMConv(ds, "LENGTH");
                        ((UOMClassDataSet.UOMConvRow)ds.UOMConv.Rows[ds.UOMConv.Rows.Count - 1]).UOMCode = uom;
                        ((UOMClassDataSet.UOMConvRow)ds.UOMConv.Rows[ds.UOMConv.Rows.Count - 1]).ConvFactor = Decimal.Parse(uom) / 12.0m;
                        ((UOMClassDataSet.UOMConvRow)ds.UOMConv.Rows[ds.UOMConv.Rows.Count - 1]).UOMDesc = "Stick " + uom + " IN";
                        BusinessObject.Update(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UOMInterface.CheckCreate: Unhandled exception. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }
    }
}
