using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ObjectLibrary;
using System.Data;

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace = "http://services.it.tcg/epicor/foxproservice")]
    public class FoxProService : IFoxProService
    {
        public datasetresult GetCIGUnitList()
        {
            try
            {
                datasetresult r = new datasetresult();
                FoxProDB fDB = new FoxProDB();
                r.ds = fDB.ReadTable(@"\\SARV-FILE01\cig\unitlist\data\", "Unitlist.DBF");
                return r;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public datasetresult GetCIGUnitInfo(string unitnum)
        {
            try
            {
                datasetresult r = new datasetresult();
                FoxProDB fDB = new FoxProDB();
                r.ds = fDB.ReadTableWhere(@"\\SARV-FILE01\cig\unitlist\data\", "Unitlist.DBF", String.Format("Unitnum = '{0}'", unitnum));
                return r;
            }
            catch (Exception ex)
            {
                throw new Exception("GetCIGUnitInfo: " + ex.Message + ";" + ex.InnerException + ";");
            }
        }

        public int InsertCIGPAReading(string unitnum, string watt, string pf, string volts, string amps, string hertz, string heater)
        {
            try
            {
                FoxProDB fDB = new FoxProDB();
                return fDB.InsertCIGPAReading(unitnum, watt, pf, volts, amps, hertz, heater);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
