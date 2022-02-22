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
    public class UOMInterface : EpicorExtension<UOMImpl, UOMSvcContract>
    {
        public bool CheckCreate(string server, string database, string username, string password, string uom)
        {
            bool created = false;
            try
            {
                decimal u;
                if (Decimal.TryParse(uom, out u))
                {
                    OpenSession(server, database, username, password, Ice.Core.Session.LicenseType.Default);

                    bool notfound = false;
                    UOMDataSet ds = new UOMDataSet();
                    try
                    {
                         ds = BusinessObject.GetByID(uom);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "Record not found.")
                            notfound = true;
                        else
                            throw ex;
                    }
                    if (notfound)
                    {
                        if (ds.UOM.Rows.Count == 0)
                        {

                            BusinessObject.GetNewUOM(ds);

                            BusinessObject.OnChangeUOMCode(uom, ds);
                            ((UOMDataSet.UOMRow)ds.Tables[0].Rows[0]).UOMCode = uom;
                            ((UOMDataSet.UOMRow)ds.Tables[0].Rows[0]).UOMDesc = "Stick " + uom + " IN";
                            BusinessObject.Update(ds);
                            created = true;

                        }
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
            return created;
        }
    }
}
