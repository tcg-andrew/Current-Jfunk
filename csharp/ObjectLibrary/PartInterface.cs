#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Epicor.Mfg.Core;
using Epicor.Mfg.BO;
using System.Text;

#endregion

namespace ObjectLibrary
{
    #region Class MiscCharge

    public class MiscCharge
    {
        public string Code { get; set; }
        public string Desc { get; set; }

        public MiscCharge()
        {
            Code = "";
            Desc = "";
        }

        public MiscCharge(string code, string desc)
        {
            Code = code;
            Desc = desc;
        }
    }

    #endregion

    #region Class PartClass

    public class PartClass
    {
        #region Properties

        public string ID { get; set; }
        public string Description { get; set; }

        #endregion

        #region Constructors

        public PartClass()
        {
            ID = "";
            Description = "";
        }

        public PartClass(string id, string desc)
        {
            ID = id;
            Description = desc;
        }

        #endregion
    }

    #endregion

    #region Class Operation

    public class Operation
    {
        #region Properties

        public string OpCode { get; set; }
        public string Desc { get; set; }

        #endregion

        #region Constructors

        public Operation()
        {
            OpCode = "";
            Desc = "";
        }

        public Operation(string opcode, string desc)
        {
            OpCode = opcode;
            Desc = desc;
        }

        #endregion
    }

    #endregion

    #region Class Part

    public class Part
    {
        #region Properties

        public string PartNum { get; set; }
        public string RevNum { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public bool NonStock { get; set; }
        public string Group { get; set; }
        public string Class { get; set; }
        public string ClassDesc { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public string Plant { get; set; }
        public decimal MonthlyUsage { get; set; }
        public decimal MinimumQty { get; set; }
        public decimal PercentDiff { get; set; }
        public decimal FreightClass { get; set; }
        public decimal Weight { get; set; }
        public decimal AvgCost { get; set; }
        public bool PhantomBom { get; set; }

        #endregion

        #region Constructors

        public Part()
        {
            PartNum = "";
            Desc = "";
            Type = "";
            NonStock = false;
            Group = "";
            Class = "";
            Unit = "";
            Price = 0;
            Plant = "";
            MonthlyUsage = 0;
            MinimumQty = 0;
            PercentDiff = 0;
            FreightClass = 0;
            Weight = 0;
            AvgCost = 0;
        }

        public Part(string partnum, string desc, string type, bool nonstock, string group, string cl, string unit, decimal price,
            string plant, decimal usage, decimal minqty, decimal pctdiff, decimal freightclass, decimal weight, decimal avgcost)
        {
            PartNum = partnum;
            Desc = desc;
            Type = type;
            NonStock = nonstock;
            Group = group;
            Class = cl;
            Unit = unit;
            Price = price;
            Plant = plant;
            MonthlyUsage = usage;
            MinimumQty = minqty;
            PercentDiff = pctdiff;
            FreightClass = freightclass;
            Weight = weight;
            AvgCost = avgcost;
        }

        #endregion
    }

    #endregion

    #region Class Part Price List

    public class PartPriceList
    {
        #region Properties

        public string CustNum { get; set; }
        public string CustID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PartNum { get; set; }
        public decimal Price { get; set; }

        #endregion

        #region Constructors

        public PartPriceList()
        {
            CustNum = "";
            CustID = "";
            StartDate = "";
            EndDate = "";
            PartNum = "";
            Price = 0.0m;
        }

        public PartPriceList(string custnum, string custid, string startdate, string enddate, string partnum, decimal price)
        {
            CustNum = custnum;
            CustID = custid;
            StartDate = startdate;
            EndDate = enddate;
            PartNum = partnum;
            Price = price;
        }

        #endregion

    }

    #endregion

    // TODO:5 Add unit tests
    public class PartInterface : EpicorExtension<Epicor.Mfg.BO.Part>
    {
        #region Public Methods

        #region Create Methods

        public void CreatePurchasedPart(string server, string port, string username, string password, string partnum, string description, string prodcode, string partclass, string binnum, int vendor, bool backflush, string buyer)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);

            PartDataSet ds = new PartDataSet();
            BusinessObject.GetNewPart(ds);
            PartDataSet.PartRow part = ds.Part.Rows[0] as PartDataSet.PartRow;
            part.PartNum = partnum;
            part.PartDescription = description;
            part.SearchWord = description.Substring(0, 8);
            part.UnitPrice = 99999;
            part.UsePartRev = false;
            part.WholeUnit = true;
            part.ClassID = partclass;

            BusinessObject.ChangePartProdCode(prodcode, ds);

            BusinessObject.Update(ds);

            ((PartDataSet.PartWhseRow)ds.PartWhse.Rows[0]).PrimBinNum = binnum;
            ((PartDataSet.PartPlantRow)ds.PartPlant.Rows[0]).VendorNum = vendor;
            ((PartDataSet.PartPlantRow)ds.PartPlant.Rows[0]).BackFlush = backflush;
            ((PartDataSet.PartPlantRow)ds.PartPlant.Rows[0]).BuyerID = buyer;

            BusinessObject.Update(ds);
        }

        #endregion

        #region Retrieve Methods

        public List<Part> GetAllParts(string server, string database, string username, string password, int limit)
        {
            List<Part> result = new List<Part>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_PartLookup @limit");
            sqlCommand.Parameters.AddWithValue("limit", limit);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new Part(row["partnum"].ToString(), row["desc"].ToString(), row["typecode"].ToString(), System.Convert.ToInt32(row["nonstock"].ToString()) == 1, row["group"].ToString(), row["class"].ToString(), row["unit"].ToString(), System.Convert.ToDecimal(row["price"].ToString()), "", 0, 0, 0, System.Convert.ToDecimal(row["freightclass"].ToString()), System.Convert.ToDecimal(row["weight"].ToString()), (decimal)0.0));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Operation> GetAllOperations(string server, string database, string username, string password)
        {
            List<Operation> result = new List<Operation>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_OperationLookup");

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new Operation(row["opcode"].ToString(), row["desc"].ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Part> GetPartsNeedingQtyUpdate(string server, string database, string username, string password, string company, string plant, string partclass)
        {
            List<Part> result = new List<Part>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_PartTran_Find_Min_To_Change @Company, @Plant, @PartClass");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("Plant", plant);
            sqlCommand.Parameters.AddWithValue("PartClass", partclass);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new Part(row["partnum"].ToString(), row["partdescription"].ToString(), "", false, "", "", "", 0, row["plant"].ToString(), System.Convert.ToDecimal(row["monthlyusage"].ToString()), System.Convert.ToDecimal(row["minimumqty"].ToString()), System.Convert.ToDecimal(row["percentdiff"].ToString()), 0, 0, Decimal.Parse(row["avgcost"].ToString())));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<string> GetPlants(string server, string database, string username, string password, string company)
        {
            List<string> result = new List<string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPlantsForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<PartClass> GetPartClasses(string server, string database, string username, string password, string company)
        {
            List<PartClass> result = new List<PartClass>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPartClassesForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new PartClass(row[0].ToString(), row[1].ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<MiscCharge> GetMiscChargeClasses(string server, string database, string username, string password)
        {
            List<MiscCharge> result = new List<MiscCharge>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_MiscChargeLookup");

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                result.Add(new MiscCharge("MATRIX", "Matrix Pricing"));
                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new MiscCharge(row["Code"].ToString(), row["Description"].ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool PartIsSalesKit(string server, string port, string username, string password, string partnum)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            bool result = false;
            try
            {
                bool outbool;
                BusinessObject.PartIsSalesKit(partnum, out result, out outbool);
            }
            catch (Exception ex)
            {
                throw new Exception("PartIsSalesKit - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        public bool PartIsNonStock(string server, string database, string username, string password, string partnum)
        {
            bool result = false;
            try
            {
                List<Part> parts = GetAllParts(server, database, username, password, 0);
                bool found = false;
                foreach (Part part in parts)
                {
                    if (part.PartNum == partnum)
                    {
                        found = true;
                        result = part.NonStock;
                    }
                }
                if (!found)
                    throw new Exception("Invalid part");
            }
            catch (Exception ex)
            {
                throw new Exception("PartIsNonStock - " + ex.Message);
            }
            return result;
        }

        public bool PartExists(string server, string port, string username, string password, string partnum)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            bool result = false;
            try
            {
                result = BusinessObject.PartExists(partnum);
            }
            catch (Exception ex)
            {
                throw new Exception("PartExists - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        public List<PartPriceList> GetPartPriceList(string server, string database, string username, string password, string company)
        {
            List<PartPriceList> result = new List<PartPriceList>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPartPriceList @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new PartPriceList(row["custnum"].ToString(), row["custid"].ToString(), row["startdate"].ToString(), row["enddate"].ToString(), row["partnum"].ToString(), Decimal.Parse(row["baseprice"].ToString())));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }

        public Part GetPartInfo(string server, string port, string username, string password, string partnum)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            Part result = new Part();
            try
            {
                PartDataSet ds = BusinessObject.GetByID(partnum);
                result.PartNum = ((PartDataSet.PartRow)ds.Part.Rows[0]).PartNum;
                result.Desc = ((PartDataSet.PartRow)ds.Part.Rows[0]).PartDescription;
                result.Class = ((PartDataSet.PartRow)ds.Part.Rows[0]).ClassID;
                result.ClassDesc = ((PartDataSet.PartRow)ds.Part.Rows[0]).ClassDescription;
                result.PhantomBom = ((PartDataSet.PartRow)ds.Part.Rows[0]).PhantomBOM;
                result.RevNum = ((PartDataSet.PartRevRow)ds.PartRev.Rows[0]).RevisionNum;
            }
            catch (Exception ex)
            {
                //throw new Exception("GetPartDescription - " + partnum + " - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        public List<XmlQuoteMtl> GetPartMtls(string server, string database, string username, string password, string company, string partnum, decimal qty)
        {
            List<XmlQuoteMtl> results = new List<XmlQuoteMtl>();

            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPartMaterials @Company, @Part");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("Part", partnum);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    XmlQuoteMtl newmat = new XmlQuoteMtl(row[1].ToString(), Decimal.Parse(row[2].ToString()));
                    newmat.AsAsm = row[3].ToString() == "1";
                    newmat.PhantomBom = row[4].ToString() == "1";

                    if (newmat.PhantomBom && newmat.AsAsm)
                    {
                        foreach (XmlQuoteMtl r in GetPartMtls(server, database, username, password, company, newmat.Name, newmat.Qty))
                        {
                            results.Add(newmat);
                        }
                    }
                    else
                    {
                        newmat.Qty *= qty;
                        results.Add(newmat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;
        }

        #endregion

        #region Update Methods

        public void CopyAllMES(string server, string database, string company, string original, string newname)
        {
            SqlCommand command = new SqlCommand("[dbo].sp_UD03_CopyAllLinkedMES");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Company", company);
            command.Parameters.AddWithValue("@OrigPart", original);
            command.Parameters.AddWithValue("NewPart", newname);

            SQLAccess.NonQuery(server, database, command);
        }

        public void DeactivatePart(string server, string port, string username, string password, string partnum)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                PartDataSet ds = BusinessObject.GetByID(partnum);

                PartDataSet.PartRow part = ds.Part.Rows[0] as PartDataSet.PartRow;

                part.InActive = true;
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DuplicatePart(string server, string port, string username, string password, string original, string newname)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                PartDataSet ds = BusinessObject.GetByID(original);

                PartDataSet.PartRow part = ds.Part.Rows[0] as PartDataSet.PartRow;

                BusinessObject.DuplicatePart(original, newname, part.PartDescription);

                ds = BusinessObject.GetByID(newname);
                ((PartDataSet.PartRevRow)ds.PartRev[0]).RowMod = "U";
                BusinessObject.ChangePartRevApproved(true, true, ds);
                BusinessObject.Update(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePartMinimumValue(string server, string port, string username, string password, Part p)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                PartDataSet ds = BusinessObject.GetByID(p.PartNum);

                foreach (PartDataSet.PartPlantRow pp in ds.PartPlant.Rows)
                {
                    if (pp.Plant == p.Plant)
                        pp.MinimumQty = p.MinimumQty;
                }

                BusinessObject.Update(ds);
                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePartsMiniumumValue(string server, string port, string username, string password, List<Part> parts)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                int cursor = 0;

                while (cursor < parts.Count)
                {
                    StringBuilder where = new StringBuilder();
                    List<Part> subset = new List<Part>();
                    for (int i = 0; i < parts.Count && i < cursor + 200; i++)
                    {
                        if (where.Length > 0)
                            where.Append(" OR ");
                        where.Append("Partnum = '" + parts[i].PartNum + "'");
                        subset.Add(parts[i]);
                    }


                    bool outbool;
                    PartDataSet ds = BusinessObject.GetRows(where.ToString(), "", "", "", "", "", "", "", "", "", "", 0, 0, out outbool);

                    foreach (Part p in subset)
                        foreach (PartDataSet.PartPlantRow pp in ds.PartPlant.Rows)
                        {
                            if (pp.Plant == p.Plant)
                            {
                                pp.MinimumQty = p.MinimumQty;
                                break;
                            }
                        }
//                    BusinessObject.Update(ds);
 //                   if (cursor < parts.Count)                    
                }

                for (int i = 0; i < parts.Count; i++)
                {
                }

                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion
    }
}
