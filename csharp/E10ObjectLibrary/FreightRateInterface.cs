#region Usings

using System.Data;
using ObjectLibrary.MihlfeldServiceReference;
using System;
using System.Collections.Generic;

#endregion

namespace ObjectLibrary
{
    #region Class FreightRate

    public class FreightRate: IComparable<FreightRate>
    {
        public string Carrier { get; set; }
        public decimal TotalCost { get; set; }
        public string JointLine { get; set; }
        public int TransitDays { get; set; }

        public FreightRate()
        {
            Carrier = "";
            TotalCost = 0;
            JointLine = "";
            TransitDays = 0;
        }

        public FreightRate(string carrier, decimal totalcost, string jointline, int transitdays)
        {
            Carrier = carrier;
            TotalCost = totalcost;
            JointLine = jointline;
            TransitDays = transitdays;
        }

        public int CompareTo(FreightRate other)
        {
            if (this.TransitDays == other.TransitDays)
                return Decimal.Compare(this.TotalCost, other.TotalCost);
            else
                return this.TransitDays.CompareTo(other.TransitDays);
        }
    }

    #endregion

    public class FreightRateInterface
    {
        #region Retrieve Methods

        public FreightRate GetRate(string Username, string Password, string ClassCode1, string Weight1, string ClassCode2, string Weight2, string ClassCode3, string Weight3, string ClassCode4, string Weight4, string ClassCode5, string Weight5, string fromzip, string tozip, DateTime shipdate, int reqID)
        {
            try
            {
                using (OpenSourceSoapClient client = new OpenSourceSoapClient("OpenSourceSoap"))
                {
                    List<FreightRate> results = new List<FreightRate>();

                    DataTable dt = client.Get_Rates(Username, Password, ClassCode1, Weight1, ClassCode2, Weight2, ClassCode3, Weight3, ClassCode4, Weight4, ClassCode5, Weight5, fromzip, tozip, "O", shipdate.ToString("MM/dd/yyyy"), "P", reqID);

                    foreach (DataRow row in dt.Rows)
                        if (row[1].ToString().ToLower() == "total")
                        {
                            int days = 0;
                            Int32.TryParse(row[8].ToString(), out days);
                            results.Add(new FreightRate(row[0].ToString(), Math.Ceiling(Decimal.Parse(row[2].ToString()) * (decimal)1.25), row[7].ToString(), days));
                        }

                    if (results.Count == 0)
                        return null;
                    else if (results.Count == 1)
                        return results[0];
                    else
                    {
                        results.Sort();
                        return results[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
