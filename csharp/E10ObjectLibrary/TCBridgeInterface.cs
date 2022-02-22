using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ObjectLibrary
{
    public class TCBridgeInterface
    {
        public class ConnectionException: Exception
        {
            public ConnectionException(string message): base(message) { }
        }

        public class DataException: Exception
        {
            public DataException(string message) : base(message) { }
        }

        public class EpicorException: Exception
        {
            public EpicorException(string message) : base(message) { }
        }

        public class MissingPartException: Exception
        {
            public MissingPartException(string message) : base(message) { }
        }

        public class PublishEvent
        {
            int _eventID;
            int _publishID;
            int _status;
            string _error;
            PartInfo _part;
            bool _success;

            public int EventID { get { return _eventID; } }
            public int PublishID { get { return _publishID; } }
            public PartInfo Part { get { return _part; } }
            public bool Success { get { return _success; } }
            public string Error { get { return _error; } }

            public PublishEvent()
            {
                _eventID = 0;
                _publishID = 0;
                _status = 0;
                _error = "Uninitialized";
                _success = false;
            }

            public PublishEvent(int event_id, int publish_id, int status, string error)
            {
                _eventID = event_id;
                _publishID = publish_id;
                _status = status;
                _error = error;
            }

            public void Complete()
            {
                _success = true;
            }

            public void SetPart(PartInfo part)
            {
                _part = part;
            }

            public void SetError(string error)
            {
                _error = error;
            }

            public void WriteError(string server, string database, string username, string password)
            {
                    SqlCommand command = new SqlCommand("UPDATE Teamcenter.PUBLISHEVENT SET ERROR = @ERROR, STATUS = 3 where EVENTID = @EVENTID");
                    command.Parameters.AddWithValue("EVENTID", _eventID);
                    command.Parameters.AddWithValue("ERROR", _error);

                    SQLAccess.NonQuery(server, database, username, password, command);
            }

            public void WriteComplete(string server, string database, string username, string password)
            {
                SqlCommand command = new SqlCommand("UPDATE Teamcenter.PUBLISHEVENT SET STATUS = 2, Error = '' where EVENTID = @EVENTID");
                command.Parameters.AddWithValue("EVENTID", _eventID);

                SQLAccess.NonQuery(server, database, username, password, command);
            }

        }

        public class PartInfo
        {
            int PKID;
            int PublishID;
            int _partTranID;
            string PartCompany;
            string _partNum;
            string _partDesc;
            string _partUOMClassID;
            string _partClassID;
            string _partInvUOM;
            string _partPurUOM;
            string _partSalesUOM;
            string _partTypeCode;
            bool _partNonStock;
            string _partProdCode;
            bool _partPhantomBOM;
            decimal _partNetWeight;
            bool _partQtyBearing;
            string _plantCompany;// removing
            string _plantPartNum;// removing
            string _plantInfo;
            string _plantSourceType; //removing
            bool _plantNonStock; //removing
            bool _plantQtyBearing; //removing
            string _revCompany;
            string _revPartnum;
            string _revNum;
            string _revShortDesc;
            string _revDesc;
            string _makefrom;
            decimal _makefromqty;
            string _drawinglink;
            string _boltype;
            decimal _framedoors;
            decimal _door;
            string _releaseStatus;

            List<string> _revBOO;
            List<BomInfo> _bom;

            public int PartTranID { get { return _partTranID; } }
            public string PartNum { get { return _partNum; } }
            public string PartDesc { get { return _partDesc; } }
            public string ClassID { get { return _partClassID; } }
            public string UOMClassID { get { return _partUOMClassID; } }
            public string InvUOM { get { return _partInvUOM; } }
            public string PurUOM { get { return _partPurUOM; } }
            public string SalesUOM { get { return _partSalesUOM; } }
            public string TypeCode { get { return _partTypeCode; } }
            public bool NonStock { get { return _partNonStock; } }
            public bool PhantomBOM { get { return _partPhantomBOM; } }
            public bool QtyBearing { get { return _partQtyBearing; } }
            public string ProdCode { get { return _partProdCode; } }
            public decimal NetWeight { get { return _partNetWeight; } }
            public string Plant { get { return _plantInfo; } }
            public string RevNum { get { return _revNum; } }
            public string RevShortDesc { get { return _revShortDesc; } }
            public string RevDesc { get { return _revDesc; } }
            public List<string> RevBoo { get { return _revBOO; } }
            public List<BomInfo> BOM { get { return _bom; } }
            public string MakeFrom { get { return _makefrom; } }
            public decimal MakeFromQty { get { return _makefromqty; } }
            public string DrawingLink { get { return _drawinglink; } }
            public string BOLType { get { return _boltype; } }
            public decimal FrameDoors { get { return _framedoors; } }
            public decimal Door { get { return _door; } }
            public string ReleaseStatus { get { return _releaseStatus; } }

            public PartInfo(int pk_id, int publish_id, int part_tran_id, string part_company, string partnum, string desc, string uomclass, string classid, string invuom, string puruom, string salesuom
                , string typecode, bool nonstock, string prodcode, bool phantombom, decimal weight, bool qtybearing, string plantcompany, string plantpartnum, string plant, string sourcetype, bool plantnonstock
                , bool plantqtybearing, string revcompany, string revpartnum, string revnum, string revshortdesc, string revdesc, List<string> revboo, string makefrom, decimal makefromqty, string drawinglink
                , string boltype, decimal framedoors, decimal door, string releasestatus)
            {
                PKID = pk_id;
                PublishID = publish_id;
                _partTranID = part_tran_id;
                PartCompany = part_company;
                _partNum = partnum;
                _partDesc = desc;
                if (releasestatus != "PRODUCTION")
                    _partDesc = releasestatus + " - " + _partDesc;
                _partUOMClassID = uomclass;
                _partClassID = classid;
                _partInvUOM = invuom;
                _partPurUOM = puruom;
                _partSalesUOM = salesuom;
                _partTypeCode = typecode;
                _partNonStock = nonstock;
                _partProdCode = prodcode;
                _partPhantomBOM = phantombom;
                _partNetWeight = weight;
                _partQtyBearing = qtybearing;
                _plantCompany = plantcompany;
                _plantPartNum = plantpartnum;
                _plantInfo = plant;
                _plantSourceType = sourcetype;
                _plantNonStock = plantnonstock;
                _plantQtyBearing = plantqtybearing;
                _revCompany = revcompany;
                _revPartnum = revpartnum;
                _revNum = revnum;
                _revShortDesc = revshortdesc;
                _revDesc = revdesc;
                _revBOO = revboo;
                _makefrom = makefrom;
                _makefromqty = makefromqty;
                _drawinglink = drawinglink;
                _boltype = boltype;
                _framedoors = framedoors;
                _door = door;
                _releaseStatus = releasestatus;
                _bom = new List<BomInfo>();
            }

            public void AddBOM(BomInfo mat)
            {
                _bom.Add(mat);
            }
        }

        public class BomInfo
        {
            int _bomTranID;
            int _partTranID;
            string _childCompany;
            string _childPartNum;
            string _childRevNum;
            int _bomLineSeqNum;
            string _parentPartNum;
            decimal _childQty;
            int _childRelOpr;
            bool _childPullAsAsm;
            bool _childViewAsAsm;

            public string PartNum { get { return _childPartNum; } }
            public string Rev { get { return _childRevNum; } }
            public decimal Qty { get { return _childQty; } }
            public int Opr { get { return _childRelOpr; } }
            public bool PullAsAsm { get { return _childPullAsAsm; } }
            public bool ViewAsAsm { get { return _childViewAsAsm; } }

            public BomInfo(int bom_tran_id, int part_tran_id, string child_company, string child_partnum, string child_revnum, int bom_lineseq, string parent_partnum, decimal child_qty, int child_relopr,
                bool child_pullasasm, bool child_viewasasm)
            {
                _bomTranID = bom_tran_id;
                _partTranID = part_tran_id;
                _childCompany = child_company;
                _childPartNum = child_partnum;
                _childRevNum = child_revnum;
                _bomLineSeqNum = bom_lineseq;
                _parentPartNum = parent_partnum;
                _childQty = child_qty;
                _childRelOpr = child_relopr;
                _childPullAsAsm = child_pullasasm;
                _childViewAsAsm = child_viewasasm;
            }
        }

        #region Retrieve Methods

        
        static public string AddErrorMessage(string full, string error)
        {
            if (full.Length > 0)
                full += ", ";
            full += error;
            return full;
        }

        public List<PublishEvent> GetPendingEvents(string server, string database, string username, string password, out string errors)
        {
            List<PublishEvent> result = new List<PublishEvent>();
            errors = "";
            
            try
            {
                SqlCommand command = new SqlCommand("SELECT EventID, PublishID, Status, Error FROM Teamcenter.PUBLISHEVENT where status = 1");
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, command);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        int event_id;
                        int publish_id;
                        int status;
                        string error = "";
                        if (!Int32.TryParse(row[0].ToString(), out event_id))
                            error = AddErrorMessage(error, "Could not parse EventID \"" + row[0].ToString() + "\"");
                        if (!Int32.TryParse(row[1].ToString(), out publish_id))
                            error = AddErrorMessage(error, "Could not parse PublishID \"" + row[1].ToString() + "\"");
                        if (!Int32.TryParse(row[2].ToString(), out status))
                            error = AddErrorMessage(error, "Count not parse Status \"" + row[2].ToString() + "\"");

                        if (error.Length > 0)
                            errors = AddErrorMessage(errors, error);
                        else
                            result.Add(new PublishEvent(event_id, publish_id, status, row[3].ToString()));
                    }
                    catch (Exception ex)
                    {
                        errors = AddErrorMessage(errors, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ConnectionException("TCBridgeInterface.GetPendingEvents: Error reading from database.  server:" + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
            }

            return result;
        }

        public PartInfo GetPartInfo(string server, string database, string username, string password, int publish_id, out string str_error)
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT PARTINFOPKID, PUBLISHID, PARTTRANID, PARTCOMPANY,PARTNUM,PARTDESC,PARTUOMCLASSID,PARTCLASSID,PARTINVUOM,PARTPURUOM,PARTSALESUOM,PARTTYPECODE,case PARTNONSTOCK when 0 then 'false' when 1 then 'true' else convert(varchar, PARTNONSTOCK) end,PARTPRODCODE,case PARTPHANTOMBOM when 0 then 'false' when 1 then 'true' else convert(varchar, PARTPHANTOMBOM) end,PARTNETWEIGHT,case PARTQTYBEARING when 0 then 'false' when 1 then 'true' else convert(varchar, PARTQTYBEARING) end,PLANTCOMPANY,PLANTPARTNUM,PLANTINFO,PLANTSOURCETYPE, case PLANTNONSTOCK when 0 then 'false' when 1 then 'true' else convert(varchar, PLANTNONSTOCK) end ,case PLANTQTYBEARING when 0 then 'false' when 1 then 'true' else convert(varchar, PLANTQTYBEARING) end,REVCOMPANY,REVPARTNUM,REVNUM,REVSHORTDESC,REVDESC,REVBOO, MAKEFROM, MAKEFROMQTY, DRAWINGLINK, BOLTYPE, FRAMEDOORS, Door, RELEASESTATUS FROM Teamcenter.PARTINFO where PUBLISHID = @PublishID");
                command.Parameters.AddWithValue("PublishID", publish_id);

                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, command);

                if (ds.Tables[0].Rows.Count == 0)
                    throw new DataException("TCBridgeInterface.GetPartInfo: No PARTINFO record found with PublishID: " + publish_id.ToString() + ".  server: " + server + ", database: " + database + ", username: " + username + ".");
                else if (ds.Tables[0].Rows.Count > 1)
                    throw new DataException("TCBridgeInterface.GetPartInfo: " + ds.Tables[0].Rows.Count.ToString() + " PARTINFO records found with publishID: " + publish_id.ToString() + "; Only one record allowed. server: " + server + ", database: " + database + ", username: " + username + ".");
                else
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    int PKID;
                    int PublishID;
                    int PartTranID;
                    bool PartNonStock;
                    bool PartPhantomBOM;
                    decimal PartNetWeight;
                    bool PartQtyBearing;
                    bool PlantNonStock;
                    bool PlantQtyBearing;
                    decimal MakeFromQty = 0;
                    decimal FrameDoors = 0;
                    decimal Door = 0;
                    string error = "";
                    if (!Int32.TryParse(row[0].ToString(), out PKID))
                        error = AddErrorMessage(error, "Could not parse PARTINFOPKID \"" + row[0].ToString() + "\"");
                    if (!Int32.TryParse(row[1].ToString(), out PublishID))
                        error = AddErrorMessage(error, "Could not parse PUBLISHID \"" + row[1].ToString() + "\"");
                    if (!Int32.TryParse(row[2].ToString(), out PartTranID))
                        error = AddErrorMessage(error, "Could not parse PARTTRANID \"" + row[2].ToString() + "\"");
                    if (!Boolean.TryParse(row[12].ToString(), out PartNonStock))
                        error = AddErrorMessage(error, "Could not parse PARTNONSTOCK \"" + row[12].ToString() + "\"");
                    if (!Boolean.TryParse(row[14].ToString(), out PartPhantomBOM))
                        error = AddErrorMessage(error, "Could not parse PARTPHANTOMBOM \"" + row[14].ToString() + "\"");
                    if (!Decimal.TryParse(row[15].ToString(), out PartNetWeight))
                        error = AddErrorMessage(error, "Could not parse PARTNETWEIGHT \"" + row[15].ToString() + "\"");
                    if (!Boolean.TryParse(row[16].ToString(), out PartQtyBearing))
                        error = AddErrorMessage(error, "Could not parse PARTYQTYBEARING \"" + row[16].ToString() + "\"");
                    if (!Boolean.TryParse(row[21].ToString(), out PlantNonStock))
                        error = AddErrorMessage(error, "Could not parse PLANTNONSTOCK \"" + row[21].ToString() + "\"");
                    if (!Boolean.TryParse(row[22].ToString(), out PlantQtyBearing))
                        error = AddErrorMessage(error, "Could not parse PLANTQTYBEARING \"" + row[22].ToString() + "\"");
                    if (String.IsNullOrEmpty(row[27].ToString()))
                        error = AddErrorMessage(error, "Blank RevDescription Not Allowed");
                    if (!String.IsNullOrEmpty(row[30].ToString()) && !Decimal.TryParse(row[30].ToString(), out MakeFromQty))
                        error = AddErrorMessage(error, "Could not parse MAKEFROMQTY \"" + row[30].ToString() + "\"");
                    if (!String.IsNullOrEmpty(row[33].ToString()) && !Decimal.TryParse(row[33].ToString(), out FrameDoors))
                        error = AddErrorMessage(error, "Could not parse FRAMEDOORS \"" + row[33].ToString() + "\"");
                    if (!String.IsNullOrEmpty(row[34].ToString()) && !Decimal.TryParse(row[34].ToString(), out Door))
                        error = AddErrorMessage(error, "Could not parse Door \"" + row[34].ToString() + "\"");


                    List<string> boo = new List<string>();
                    if (PartPhantomBOM)
                    {
                        Regex regex = new Regex(@"^[A-Z]+$");

                        if (regex.Matches(row[28].ToString()).Count == 0)
                            error = AddErrorMessage(error, "PhantomBOM invalid BOO format \"" + row[28].ToString() + "\".  Correct format is {code}");
                        else
                            boo.Add(row[28].ToString().Trim());
                    }
                    else
                    {
                        foreach (string str in row[28].ToString().Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            string[] split = str.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            if (split.Length != 2)
                                error = AddErrorMessage(error, "Invalid BOO format \"" + str + "\".  Correct format is {seq}-{code} with \";\" seperator");
                            else
                            {
                                int seq;
                                if (!Int32.TryParse(split[0], out seq))
                                {
                                    error = AddErrorMessage(error, "Could not parse BOO SEQ \"" + split[0] + "\" of \" " + str + "\"");
                                }
                                else
                                    boo.Add(split[1]);
                            }
                        }
                    }
                    str_error = error;
                    return new PartInfo(PKID, PublishID, PartTranID, row[3].ToString().Trim(), row[4].ToString().Trim(), row[5].ToString().Trim(), row[6].ToString().Trim(), row[7].ToString().Trim(), row[8].ToString().Trim(), row[9].ToString().Trim(),
                            row[10].ToString().Trim(), row[11].ToString().Trim(), PartNonStock, row[13].ToString().Trim(), PartPhantomBOM, PartNetWeight, PartQtyBearing, row[17].ToString().Trim(), row[18].ToString().Trim(), row[19].ToString().Trim(),
                            row[20].ToString().Trim(), PlantNonStock, PlantQtyBearing, row[23].ToString().Trim(), row[24].ToString().Trim(), row[25].ToString().Trim(), row[26].ToString().Trim(), row[27].ToString().Trim(), boo, row[29].ToString().Trim(), MakeFromQty,
                            row[31].ToString().Trim(), row[32].ToString().Trim(), FrameDoors, Door, row[35].ToString().Trim());
                }

            }
            catch (DataException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ConnectionException("TCBridgeInterface.GetPartInfo: Error reading from PARTINFO with PublishID: " + publish_id.ToString() + ".  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
            }
        }

        public void GetBOMInfo(string server, string database, string username, string password, PartInfo part)
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT BOMTRANID, PARTTRANID, CHILDCOMPANY, CHILDPARTNUM, CHILDREVNUM, BOMLINESEQNUM, PARENTPARTNUM, CHILDQTY, CHILDRELOPERN, case CHILDPULLASASM when 0 then 'false' when 1 then 'true' else convert(varchar, CHILDPULLASASM) end, case CHILDVIEWASASM when 0 then 'false' when 1 then 'true' else convert(varchar, CHILDVIEWASASM) end FROM Teamcenter.BOMINFO where PARTTRANID = @PartTranID ORDER BY BOMLINESEQNUM ASC");
                command.Parameters.AddWithValue("PartTranID", part.PartTranID);

                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, command);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int BomTranID = -1;
                    int PartTranID;
                    int BomLineSeqNum;
                    decimal ChildQty;
                    int ChildRelOpern;
                    bool ChildPullAsAsm;
                    bool ChildViewAsAsm;

                    string error = "";
                    if (!Int32.TryParse(row[0].ToString(), out BomTranID))
                        error = AddErrorMessage(error, "Could not parse BOMTRANID \"" + row[0].ToString() + "\"");
                    if (!Int32.TryParse(row[1].ToString(), out PartTranID))
                        error = AddErrorMessage(error, "Could not parse PARTTRANID \"" + row[1].ToString() + "\"");
                    if (!Int32.TryParse(row[5].ToString(), out BomLineSeqNum))
                        error = AddErrorMessage(error, "Could not parse BOMLINESEQNUM \"" + row[5].ToString() + "\"");
                    if (!Decimal.TryParse(row[7].ToString(), out ChildQty))
                        error = AddErrorMessage(error, "Could not parse CHILDQTY \"" + row[7].ToString() + "\"");
                    if (!Int32.TryParse(row[8].ToString(), out ChildRelOpern))
                    {
                        error = AddErrorMessage(error, "Could not parse CHILDRELOPERN \"" + row[8].ToString() + "\"");
                    }
                    else
                    {
                        if (part.PhantomBOM && ChildRelOpern != 0)
                            error = AddErrorMessage(error, "Invalid CHILDRELOPERN \"" + ChildRelOpern.ToString() + "\" for mtl \"" + row[3].ToString() + "\" of Phantom BOM part.  CHILDRELOPERN must be 0.");
                    }
                    if (!Boolean.TryParse(row[9].ToString(), out ChildPullAsAsm))
                        error = AddErrorMessage(error, "Could not parse CHILDPULLASASM \"" + row[9].ToString() + "\"");
                    if (!Boolean.TryParse(row[10].ToString(), out ChildViewAsAsm))
                        error = AddErrorMessage(error, "Could not parse CHILDVIEWASASM \"" + row[10].ToString() + "\"");

                    if (error.Length > 0)
                        throw new DataException("Error parsing BOMINFO record with BOMTRANID \"" + row[0].ToString() + "\": " + error);
                    else
                        part.AddBOM(new BomInfo(BomTranID, PartTranID, row[2].ToString(), row[3].ToString(), row[4].ToString(), BomLineSeqNum, row[6].ToString(), ChildQty, ChildRelOpern, ChildPullAsAsm, ChildViewAsAsm));
                }

            }
            catch (DataException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ConnectionException("TCBridgeInterface.GetBOMInfo: Error reading from BOMINFO with PartTranID: " + part.PartTranID.ToString() + ".  server: " + server + ", database: " + database + ", username: " + username + ".  Inner exception: " + ex.Message);
            }
        }

        #endregion
    }
}
