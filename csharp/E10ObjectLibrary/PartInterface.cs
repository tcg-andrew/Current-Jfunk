#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Erp.Proxy.BO;
using Erp.BO;
using Ice.Core;
using Erp.Contracts;
using static ObjectLibrary.TCBridgeInterface;
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
        public int Seq { get; set; }

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

        public Operation(string opcode, int seq)
        {
            OpCode = opcode;
            Seq = seq;
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
        public string PhantomOpr { get; set; }
        public List<Operation> Opr { get; set; }

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
            PhantomOpr = "";
            Opr = new List<Operation>();
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
            Opr = new List<Operation>();
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
    public class PartInterface : EpicorExtension<PartImpl, PartSvcContract>
    {
        #region Public Methods

        #region Create Methods

        private string WriteTCPartInfo(PartDataSet ds, PartInfo part, bool exists)
        {
            string errors = "";

            PartDataSet.PartRow partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            string changed1;
            string changed2;

            #region Set Part Values

            try
            {
                BusinessObject.ChangePartNum(part.PartNum, ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing PartNum \"" + part.PartNum + "\": " + ex.Message);
            }
            try
            {
                partRow.PartDescription = part.PartDesc;
                partRow.SearchWord = part.PartDesc.Substring(0, (part.PartDesc.Length > 8 ? 8 : part.PartDesc.Length));
                if (!exists)
                    partRow.InActive = true;
                if (part.UOMClassID != partRow.UOMClassID)
                    partRow.UOMClassID = part.UOMClassID;
                if (part.InvUOM != partRow.IUM)
                    partRow.IUM = part.InvUOM;
                if (part.PurUOM != partRow.PUM)
                    partRow.PUM = part.PurUOM;
                if (part.SalesUOM != partRow.SalesUM)
                partRow.SalesUM = part.SalesUOM;
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing Description, SearchWord, InActive, UOMClassID, IUM, PUM, or SalesUM: " + ex.Message);
            }
            try
            {
                partRow.ProdCode = part.ProdCode;
                BusinessObject.ChangePartProdCode(part.ProdCode, ds);
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing ProdCode \"" + part.ProdCode + "\": " + ex.Message);
            }
            try
            {
                partRow.ClassID = part.ClassID;
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing ClassID \"" + part.ClassID + "\": " + ex.Message);
            }
            try
            {
                partRow.TypeCode = part.TypeCode;
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing TypeCode \"" + part.TypeCode + "\": " + ex.Message);
            }
            try
            {
                partRow.NonStock = part.NonStock;
                partRow.QtyBearing = part.QtyBearing;
                partRow.PhantomBOM = part.PhantomBOM;
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing NonStock, QtyBearing, or PhantomBOM: " + ex.Message);
            }
            try
            {
                partRow.NetWeight = part.NetWeight;
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing NetWeight \"" + part.NetWeight.ToString() + "\": " + ex.Message);
            }
            try
            {
                partRow["shortchar07"] = part.BOLType;
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing BOLType \"" + part.BOLType.ToString() + "\": " + ex.Message);
            }
            try
            {
                partRow["number05"] = part.FrameDoors;
                partRow["number06"] = part.Door;
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing FrameDoors \"" + part.FrameDoors.ToString() + "\" or Door \"" + part.Door.ToString() + "\": " + ex.Message);
            }
            if (!String.IsNullOrEmpty(part.DrawingLink))
            {
                try
                {
                    foreach (PartDataSet.PartAttchRow row in ds.PartAttch.Rows)
                    {
                        if (row.DrawDesc == "DrawingLink")
                            row.Delete();
                    }
                    BusinessObject.Update(ds);
                    BusinessObject.GetNewPartAttch(ds, part.PartNum);
                    PartDataSet.PartAttchRow drawrow = ds.PartAttch.Rows[ds.PartAttch.Rows.Count - 1] as PartDataSet.PartAttchRow;
                    drawrow.DrawDesc = "DrawingLink";
                    drawrow.FileName = part.DrawingLink;
                    BusinessObject.Update(ds);
                    partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
                }
                catch (Exception ex)
                {
                    return TCBridgeInterface.AddErrorMessage(errors, "Error creating DrawingLink attachment: " + ex.Message);
                }
            }

            PartDataSet.PartPlantRow plantRow = ((PartDataSet.PartPlantRow)ds.PartPlant.Rows[0]);
            try
            {
                plantRow.Plant = part.Plant;
                foreach (PartDataSet.PartWhseRow whse in ds.PartWhse)
                {
                    if (whse.WarehouseCode == "SFL")
                    {
                        whse.PrimBinNum = "SFLMAINBIN";
                    }
                }
                plantRow.NonStock = part.NonStock;
                plantRow.SourceType = part.TypeCode;
                plantRow.QtyBearing = part.QtyBearing;
                plantRow.PhantomBOM = part.PhantomBOM;
                plantRow.BackFlush = true;

                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error writing Plant (PlantInfo) \"" + part.Plant + "\" or BackFlush: " + ex.Message);
            }
            try
            {
                partRow.UsePartRev = true;
                BusinessObject.Update(ds);
                partRow = ds.Part.Rows[0] as PartDataSet.PartRow;
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error setting UsePartRev: " + ex.Message);
            }

            #endregion

            #region Create Part Revision

            try
            {
                bool found = false;
                bool approved = false;
                foreach (PartDataSet.PartRevRow rev in ds.PartRev.Rows)
                {
                    if (rev.RevisionNum == part.RevNum)
                    {
                        found = true;
                        if (rev.Approved)
                            approved = true;
                    }
                }
                if (approved)
                {
                    return TCBridgeInterface.AddErrorMessage(errors, "Attempting to modify approved Revision \"" + part.RevNum + "\".  Revision not modified.");
                }
                else
                {
                    if (found)
                    {
                        foreach (PartDataSet.PartRevRow rev in ds.PartRev.Rows)
                        {
                            if (rev.RevisionNum == part.RevNum)
                                rev.Delete();
                        }
                        /*foreach (PartDataSet.PartCOPartRow cop in ds.PartCOPart.Rows)
                        {
                            if (cop.RevisionNum == part.RevNum)
                                cop.Delete();
                        }*/
                        BusinessObject.Update(ds);
                    }

                    BusinessObject.GetNewPartRev(ds, part.PartNum, "");

                    foreach (PartDataSet.PartRevRow rev in ds.PartRev.Rows)
                    {
                        if (rev.RevisionNum == "(new)")
                        {
                            rev.RevisionNum = part.RevNum;
                            rev.RevShortDesc = part.RevShortDesc;
                            rev.RevDescription = part.RevDesc;
                            if (part.PhantomBOM)
                                rev["phantomopr_c"] = part.RevBoo[0];
                            
                            BusinessObject.ChangePartRevApproved(true, true, ds);
                            rev.Approved = true;

                        }
                    }
                    BusinessObject.Update(ds);
                }
            }
            catch (Exception ex)
            {
                return TCBridgeInterface.AddErrorMessage(errors, "Error updating revision: " + ex.Message);
            }

            #endregion

            return errors;

        }

        public bool GetRevStatus(string server, string database, string username, string password, string partnum, string revnum)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                bool result = false;

                PartDataSet ds = new PartDataSet();
                ds = BusinessObject.GetByID(partnum);

                    foreach (PartDataSet.PartRevRow rev in ds.PartRev.Rows)
                    {
                        if (rev.RevisionNum == revnum)
                        {
                            result = rev.Approved;
                        }
                    }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("PartInterface.UnapproveRevision: Unhandled exception. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        public void UnapproveOtherRevision(string server, string database, string username, string password, PartInfo part)
        {
            try
            {
                try
                {
                    OpenSession(server, database, username, password, Session.LicenseType.Default);
                }
                catch (Exception ex)
                {
                    throw new EpicorException("PartInterface.UnapproveRevision: Error opening session. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
                }

                PartDataSet ds = new PartDataSet();
                try
                {
                    ds = BusinessObject.GetByID(part.PartNum);
                }
                catch (Exception ex)
                {
                    CloseSession();
                    throw new EpicorException("PartInterface.Unapproverevision: Error getting new part. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
                }

                try
                {
                    foreach (PartDataSet.PartRevRow rev in ds.PartRev.Rows)
                    {
                        if (rev.RevisionNum != part.RevNum)
                        {
                            //BusinessObject.ChangePartRevApproved(false, true, ds);
                            rev.Approved = false;
                        }
                    }
                    BusinessObject.Update(ds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseSession();
                }

                CloseSession();
            }
            catch (EpicorException ex)
            {
                throw ex;
            }
            catch (TCBridgeInterface.DataException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("PartInterface.UnapproveRevision: Unhandled exception. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
            }
            finally
            {
                CloseSession();
            }

        }

        public void UnapproveRevision(string server, string database, string username, string password, string partnum, string revnum)
        {
            try
            {
                try
                {
                    OpenSession(server, database, username, password, Session.LicenseType.Default);
                }
                catch (Exception ex)
                {
                    throw new EpicorException("PartInterface.UnapproveRevision: Error opening session. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
                }

                PartDataSet ds = new PartDataSet();
                try
                {
                    ds = BusinessObject.GetByID(partnum);
                }
                catch (Exception ex)
                {
                    CloseSession();
                    throw new EpicorException("PartInterface.Unapproverevision: Error getting new part. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
                }

                try
                {
                    foreach (PartDataSet.PartRevRow rev in ds.PartRev.Rows)
                    {
                        if (rev.RevisionNum == revnum)
                        {
                            //BusinessObject.ChangePartRevApproved(false, true, ds);
                            rev.Approved = false;
                        }
                    }
                    BusinessObject.Update(ds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseSession();
                }

                CloseSession();
            }
            catch (EpicorException ex)
            {
                throw ex;
            }
            catch (TCBridgeInterface.DataException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("PartInterface.UnapproveRevision: Unhandled exception. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
            }
            finally
            {
                CloseSession();
            }

        }

        public void CreateTCPart(string server, string database, string username, string password, PartInfo part)
        {
            try
            {
                try
                {
                    OpenSession(server, database, username, password, Session.LicenseType.Default);
                }
                catch (Exception ex)
                {
                    throw new EpicorException("PartInterface.CreateTCPart: Error opening session. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
                }

                PartDataSet ds = new PartDataSet();
                try
                {
                    BusinessObject.GetNewPart(ds);
                }
                catch (Exception ex)
                {
                    throw new EpicorException("PartInterface.CreateTCPart: Error getting new part. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
                }

                try
                {
                    string errors = WriteTCPartInfo(ds, part, false);
                    if (errors.Length > 0)
                        throw new TCBridgeInterface.DataException("PartInterface.CreateTCPart: PartInfo errors: " + errors);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (EpicorException ex)
            {
                throw ex;
            }
            catch (TCBridgeInterface.DataException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("PartInterface.CreateTCPart: Unhandled exception. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        public void UpdateTCPart(string server, string database, string username, string password, PartInfo part)
        {
            try
            {
                try
                {
                    OpenSession(server, database, username, password, Session.LicenseType.Default);
                }
                catch (Exception ex)
                {
                    throw new EpicorException("PartInterface.UpdateTCPart: Error opening session. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
                }

                try
                {
                    PartDataSet ds = new PartDataSet();
                    try
                    {
                        ds = BusinessObject.GetByID(part.PartNum);
                    }
                    catch (Exception ex)
                    {
                        throw new EpicorException("PartInterface.UpdateTCPart: Error getting existing part \"" + part.PartNum + "\". server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
                    }

                    try
                    {
                        string errors = WriteTCPartInfo(ds, part, true);
                        if (errors.Length > 0)
                            throw new TCBridgeInterface.DataException("PartInterface.UpdateTCPart: PartInfo errors: " + errors);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (EpicorException ex)
            {
                throw ex;
            }
            catch (TCBridgeInterface.DataException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("PartInterface.UpdateTCPart: Unhandled exception. server: " + server + ", database: " + database + ", username: " + username + ". Exception: " + ex.Message);
            }
            finally
            {
                CloseSession();
            }

        }

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
            /// TODO: Figure out fix or replace
            //part.WholeUnit = true;
            part.ClassID = partclass;
            part.IUM = "EA";
            part.PUM = "EA";
            part.SalesUM = "EA";

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
                {
                    result.Add(new Part(row["partnum"].ToString(), row["desc"].ToString(), row["typecode"].ToString(), Boolean.Parse(row["nonstock"].ToString()), row["group"].ToString(), row["class"].ToString(), row["unit"].ToString(), System.Convert.ToDecimal(row["price"].ToString()), "", 0, 0, 0, System.Convert.ToDecimal(row["freightclass"].ToString()), System.Convert.ToDecimal(row["weight"].ToString()), (decimal)0.0));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Operation> GetAllOperations(string server, string database, string username, string password, string company = "CRD")
        {
            List<Operation> result = new List<Operation>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_OperationLookup @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

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

        public List<Operation> GetPartOperations(string server, string database, string username, string password, string partnum, string rev, string company = "CRD")
        {
            List<Operation> result = new List<Operation>();
            SqlCommand command = new SqlCommand("exec [dbo].sp_GetPartOperations @Company, @Partnum, @RevisionNum");
            command.Parameters.AddWithValue("Company", company);
            command.Parameters.AddWithValue("Partnum", partnum);
            command.Parameters.AddWithValue("RevisionNum", rev);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, command);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new Operation(row["opcode"].ToString(), Int32.Parse(row["oprseq"].ToString())));
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

        public bool PartGetsDetails(string server, string database, string username, string password, string partnum)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            bool result = false;
            try
            {
                PartDataSet ds = BusinessObject.GetByID(partnum);
                bool found = false;
                foreach (PartDataSet.PartRow row in ds.Part.Rows)
                {
                    if (row.PartNum == partnum)
                    {
                        found = true;
                        result = row.NonStock && row.TypeCode == "M";
                    }
                }
                if (!found)
                    throw new Exception("Invalid part");
            }
            catch (Exception ex)
            {
                throw new Exception("PartIsNonStock - " + ex.Message);
            }
            finally
            {
                CloseSession();
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

        public bool GetPartActive(string server, string port, string username, string password, string partnum)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            bool result = false;

            try
            {
                PartDataSet ds = BusinessObject.GetByID(partnum);
                result = !((PartDataSet.PartRow)ds.Part.Rows[0]).InActive;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }
            return result;

        }

        public decimal GetPartListPrice(string server, string port, string username, string password, string partnum)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            decimal result = 0;

            try
            {
                PartDataSet ds = BusinessObject.GetByID(partnum);
                result = ((PartDataSet.PartRow)ds.Part.Rows[0]).UnitPrice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
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
                string rev = "";
                DateTime rev_approved = DateTime.Now;
                if (ds.PartRev.Rows.Count > 0)
                {
                    for(int x = 0; x < ds.PartRev.Rows.Count; x++)
                    {
                        PartDataSet.PartRevRow r = ((PartDataSet.PartRevRow)ds.PartRev.Rows[x]);
                        if (r.Approved)
                        {
                            if (String.IsNullOrEmpty(rev) || r.EffectiveDate > rev_approved || (r.EffectiveDate == rev_approved && r.RevisionNum.CompareTo(rev) > 0))
                            {
                                rev = r.RevisionNum;
                                if (result.PhantomBom)
                                    result.PhantomOpr = r["phantomopr_c"].ToString();
                                rev_approved = r.EffectiveDate;
                            }
                        }
                    }
                    result.RevNum = rev;
                }
                result.Unit = ((PartDataSet.PartRow)ds.Part.Rows[0]).IUM;
            }
            catch (Exception ex)
            {
                throw new Exception("GetPartDescription - " + partnum + " - " + ex.Message);
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
                    newmat.AsAsm = (bool)row[3];
                    newmat.PhantomBom = (bool)row[4];

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

        public void ChangePrimaryBin(string server, string database, string username, string password, string partnum, string code, string bin)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);
                PartDataSet ds = BusinessObject.GetByID(partnum);

                foreach (PartDataSet.PartWhseRow row in ds.PartWhse.Rows)
                {
                    if (row.WarehouseCode == code)
                        row.PrimBinNum = bin;

                }

                BusinessObject.Update(ds);
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
        public void CopyAllMES(string server, string database, string company, string original, string newname)
        {
            SqlCommand command = new SqlCommand("[dbo].sp_UD03_CopyAllLinkedMES");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Company", company);
            command.Parameters.AddWithValue("@OrigPart", original);
            command.Parameters.AddWithValue("NewPart", newname);

            SQLAccess.NonQuery(server, database, command);
        }

        public void DeactivatePart(string server, string database, string username, string password, string partnum)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);
                PartDataSet ds = BusinessObject.GetByID(partnum);

                PartDataSet.PartRow part = ds.Part.Rows[0] as PartDataSet.PartRow;

                part.InActive = true;
                BusinessObject.Update(ds);
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


        public void ActivatePart(string server, string database, string username, string password, string partnum)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);
            try
            {
                PartDataSet ds = BusinessObject.GetByID(partnum);

                PartDataSet.PartRow part = ds.Part.Rows[0] as PartDataSet.PartRow;

                part.InActive = false;
                BusinessObject.Update(ds);
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

        public void DuplicatePart(string server, string database, string username, string password, string original, string newname)
        {
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);
                
                PartDataSet ds = BusinessObject.GetByID(original);

                PartDataSet.PartRow part = ds.Part.Rows[0] as PartDataSet.PartRow;

                BusinessObject.DuplicatePart(original, newname, part.PartDescription, "COPY", "", "", "PC");

                ds = BusinessObject.GetByID(newname);
                ((PartDataSet.PartRevRow)ds.PartRev[0]).RowMod = "U";
                BusinessObject.ChangePartRevApproved(true, true, ds);
                BusinessObject.Update(ds);

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
                    PartDataSet ds = BusinessObject.GetRows(where.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, out outbool);

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

        public bool UpdateUnitPrice(string server, string database, string username, string password, string partnum, decimal unit_price)
        {
            bool success = true;
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            try
            {

                PartDataSet ds = BusinessObject.GetByID(partnum);
                foreach (PartDataSet.PartRow row in ds.Part.Rows)
                {
                    if (row.PartNum == partnum)
                    {
                        row.UnitPrice = unit_price;
                    }
                }
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                success = false;
            }
            finally
            {
                CloseSession();
            }
            return success;
        }

        public bool UpdateTCFix(string server, string database, string username, string password, string partnum, string bol_type, string frames, string doors)
        {
            bool success = true;
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            try
            {

                PartDataSet ds = BusinessObject.GetByID(partnum);
                foreach (PartDataSet.PartRow row in ds.Part.Rows)
                {
                    if (row.PartNum == partnum)
                    {
                        row["shortchar07"] = bol_type;
                        row["number05"] = frames;
                        row["number06"] = doors;
                    }
                }
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                success = false;
            }
            finally
            {
                CloseSession();
            }
            return success;
        }

        public bool UpdateTradeData(string server, string database, string username, string password, string partnum,
            string CountryOrig1, string ECCN, string ECCN_Date, string ECCN_UpdBy, string NAFTA_Date, string NAFTA_HTS, string NAFTA_UpdBy, string NAFTAElig, string NAFTAElig_Date,
            string SchedBNo, string SchedBNo_Date, string SchedBNo_UpdBy, string Supplier, string Supplier_Date, string Supplier_UpdBy)
        {
            bool success = true;
            OpenSession(server, database, username, password, Session.LicenseType.Default);

            try
            {

                PartDataSet ds = BusinessObject.GetByID(partnum);
                foreach (PartDataSet.PartRow row in ds.Part.Rows)
                {
                    if (row.PartNum == partnum)
                    {
                        row["CountryOrig1_c"] = CountryOrig1;
                        row["ECCN_c"] = ECCN;
                        if (!String.IsNullOrEmpty(ECCN_Date))
                            row["ECCN_Date_c"] = DateTime.Parse(ECCN_Date);
                        row["ECCN_UpdBy_c"] = ECCN_UpdBy;
                        if (!String.IsNullOrEmpty(NAFTA_Date))
                            row["NAFTA_Date_c"] = DateTime.Parse(NAFTA_Date);
                        row["NAFTA_HTS_c"] = NAFTA_HTS;
                        row["NAFTA_UpdBy_c"] = NAFTA_UpdBy;
                        if (!String.IsNullOrEmpty(NAFTAElig))
                            row["NAFTAElig_c"] = NAFTAElig == "1";
                        if (!String.IsNullOrEmpty(NAFTAElig_Date))
                            row["NAFTAElig_Date_c"] = DateTime.Parse(NAFTAElig_Date);
                        row["SchedBNo_c"] = SchedBNo;
                        if (!String.IsNullOrEmpty(SchedBNo_Date))
                            row["SchedBNo_Date_c"] = DateTime.Parse(SchedBNo_Date);
                        row["SchedBNo_UpdBy_c"] = SchedBNo_UpdBy;
                        row["Supplier_c"] = Supplier;
                        if (!String.IsNullOrEmpty(Supplier_Date))
                            row["Supplier_Date_c"] = DateTime.Parse(Supplier_Date);
                        row["Supplier_UpdBy_c"] = Supplier_UpdBy;
                    }
                }
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                success = false;
                throw ex;
            }
            finally
            {
                CloseSession();
            }
            return success;
        }

        #endregion

        #endregion
    }
}
