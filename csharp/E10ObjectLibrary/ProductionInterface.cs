#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

#endregion

namespace ObjectLibrary
{
    #region Class OneDayDetail

    public class OneDayDetail
    {
        #region properties

        public string Jobnum { get; set; }
        public string Assembly { get; set; }
        public string Doors { get; set; }
        public string Frames { get; set; }
        public string ConfigPart { get; set; }
        public string Openings { get; set; }
        public string Part { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CRDDate { get; set; }
        public string SpecialInstructions { get; set; }

        #endregion

        #region Constructors

        public OneDayDetail()
        {
            Jobnum = "";
            Assembly = "";
            Doors = "";
            Frames = "";
            ConfigPart = "";
            Openings = "";
            Part = "";
            Description = "";
            ImageUrl = "";
            CRDDate = "";
            SpecialInstructions = "";
        }

        public OneDayDetail(string jobnum, string assembly, string doors, string frames, string configpart, string openings, string part, string description, string imageurl, string crddate, string specinst)
        {
            Jobnum = jobnum;
            Assembly = assembly;
            Doors = doors;
            Frames = frames;
            ConfigPart = configpart;
            Openings = openings;
            Part = part;
            Description = description;
            ImageUrl = imageurl;
            CRDDate = crddate;
            SpecialInstructions = specinst;
        }

        #endregion
    }

    #endregion

    #region Class SOBasedSchedule

    public class SOBasedSchedule
    {
        #region Properties

        public string PONum { get; set; }
        public string Name { get; set; }
        public string CustID { get; set; }
        public string LineDesc { get; set; }
        public string NeedByDate { get; set; }
        public string OpenRelease { get; set; }
        public string OrderLine { get; set; }
        public string OrderNum { get; set; }
        public string OrderRelNum { get; set; }
        public string PartNum { get; set; }
        public string ReqDate { get; set; }
        public string RevisionNum { get; set; }
        public string Character05 { get; set; }
        public string OurReqQty { get; set; }
        public string AssemblySeq { get; set; }
        public string Character01 { get; set; }
        public string OpCode { get; set; }
        public string OprSeq { get; set; }
        public string QtyCompleted { get; set; }
        public string RunQty { get; set; }
        public string Number01 { get; set; }
        public string OurJobShippedQty { get; set; }
        public string OurStockShippedQty { get; set; }
        public string JobAsmCommentText { get; set; }
        public string ShortChar01 { get; set; }
        public string JobNum { get; set; }
        public string JobHeadCommentText { get; set; }
        public string Character07 { get; set; }
        public string DrawNum { get; set; }
        public string ShipViaCode { get; set; }
        public string CRDDate { get; set; }
        public string OutstandingQty { get; set; }
        public string PicUrl { get; set; }
        public string Plant { get; set; }
        public string Progress_recid { get; set; }
        public string Drawing { get; set; }
        public string CSR { get; set; }
        public List<string> ActiveEmployees { get; set; }

        #endregion

        #region Constructors

        public SOBasedSchedule()
        {
            PONum = "";
            Name = "";
            CustID = "";
            LineDesc = "";
            NeedByDate = "";
            OpenRelease = "";
            OrderLine = "";
            OrderNum = "";
            OrderRelNum = "";
            PartNum = "";
            ReqDate = "";
            RevisionNum = "";
            Character05 = "";
            OurReqQty = "";
            AssemblySeq = "";
            Character01 = "";
            OpCode = "";
            OprSeq = "";
            QtyCompleted = "";
            RunQty = "";
            Number01 = "";
            OurJobShippedQty = "";
            OurStockShippedQty = "";
            JobAsmCommentText = "";
            ShortChar01 = "";
            JobNum = "";
            JobHeadCommentText = "";
            Character07 = "";
            DrawNum = "";
            ShipViaCode = "";
            CRDDate = "";
            OutstandingQty = "";
            PicUrl = "";
            Plant = "";
            Progress_recid = "";
            Drawing = "";
            CSR = "";
            ActiveEmployees = new List<string>();
        }

        public SOBasedSchedule(string ponum, string name, string custid, string linedesc, string needbydate, string openrelease, string orderline, string ordernum, string orderrelnum, string partnum, string reqdate, string revisionnum, string character05,
            string ourreqqty, string assemblyseq, string character01, string opcode, string oprseq, string qtycompleted, string runqty, string number01, string ourjobshippedqty, string ourstockshippedqty, string jobasmcommentext, string shortchar01, string jobnum,
            string jobheadcommenttext, string character07, string drawnum, string shipviacode, string crddate, string outstandingqty, string picurl, string plant, string progress_recid, string drawing, string csr)
        {
            PONum = ponum;
            Name = name;
            CustID = custid;
            LineDesc = linedesc;
            NeedByDate = needbydate;
            OpenRelease = openrelease;
            OrderLine = orderline;
            OrderNum = ordernum;
            OrderRelNum = orderrelnum;
            PartNum = partnum;
            ReqDate = reqdate;
            RevisionNum = revisionnum;
            Character05 = character05;
            OurReqQty = ourreqqty;
            AssemblySeq = assemblyseq;
            Character01 = character01;
            OpCode = opcode;
            OprSeq = oprseq;
            QtyCompleted = qtycompleted;
            RunQty = runqty;
            Number01 = number01;
            OurJobShippedQty = ourjobshippedqty;
            OurStockShippedQty = ourstockshippedqty;
            JobAsmCommentText = jobasmcommentext;
            ShortChar01 = shortchar01;
            JobNum = jobnum;
            JobHeadCommentText = jobheadcommenttext;
            Character07 = character07;
            DrawNum = drawnum;
            ShipViaCode = shipviacode;
            CRDDate = crddate;
            OutstandingQty = outstandingqty;
            PicUrl = picurl;
            Plant = plant;
            Progress_recid = progress_recid;
            Drawing = drawing;
            CSR = csr;
            ActiveEmployees = new List<string>();
        }

        #endregion
    }

    #endregion

    #region Class ActiveLabor

    public class ActiveLabor
    {
        #region Properties

        public string EmployeeID { get; set; }
        public string JobNum { get; set; }
        public string AssemblySeq { get; set; }
        public string OprSeq { get; set; }
        public string OprCode { get; set; }
        public string EmployeeName { get; set; }
        public string Revision { get; set; }

        #endregion

        #region Constructors

        public ActiveLabor()
        {
            EmployeeID = "";
            JobNum = "";
            AssemblySeq = "";
            OprSeq = "";
            OprCode = "";
            EmployeeName = "";
            Revision = "";
        }

        public ActiveLabor(string employeeid, string jobnum, string assemblyseq, string oprseq, string oprcode, string employeename, string revision)
        {
            EmployeeID = employeeid;
            JobNum = jobnum;
            AssemblySeq = assemblyseq;
            OprSeq = oprseq;
            OprCode = oprcode;
            EmployeeName = employeename;
            Revision = revision;
        }

        #endregion
    }

    #endregion

    #region Class IndirectActivity

    public class IndirectActivity
    {
        #region Properties

        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string IndirectCode { get; set; }
        public string ResourceGrpId { get; set; }

        #endregion

        #region Constructors

        public IndirectActivity()
        {
            EmployeeID = "";
            EmployeeName = "";
            IndirectCode = "";
            ResourceGrpId = "";
        }

        public IndirectActivity(string employeeid, string employeename, string indirectcode, string resourcegrpid)
        {
            EmployeeID = employeeid;
            EmployeeName = employeename;
            IndirectCode = indirectcode;
            ResourceGrpId = resourcegrpid;
        }

        #endregion

    }
    #endregion

    #region Class MESInfo

    public class MESInfo
    {
        #region Properties

        public string MESNum { get; set; }
        public string Description { get; set; }
        public bool HasDrawings { get; set; }

        #endregion

        #region Constructors

        public MESInfo()
        {
            MESNum = "";
            Description = "";
            HasDrawings = false;
        }

        public MESInfo(string mesnum, string description, bool hasdrawings)
        {
            MESNum = mesnum;
            Description = description;
            HasDrawings = hasdrawings;
        }

        #endregion
    }

    #endregion

    #region Class MESDrawing

    public class MESDrawing
    {
        #region Properties

        public string Name { get; set; }
        public string File { get; set; }
        public byte[] FileData { get; set; }

        #endregion

        #region Constructors

        public MESDrawing()
        {
            Name = "";
            File = "";
            FileData = null;
        }

        public MESDrawing(string name, string file, byte[] data)
        {
            Name = name;
            File = file;
            FileData = data;
        }

        #endregion
    }

    #endregion

    #region Class PAData

    public class PAData
    {
        #region Properties

        public bool IsFrame { get; set; }
        public bool IsDoor { get; set; }
        public string drfrwire { get; set; }
        public string drfrohlo { get; set; }
        public string drfrohhi { get; set; }
        public string drglohlo { get; set; }
        public string drglohhi { get; set; }
        public string drdrohlo { get; set; }
        public string drdrohhi { get; set; }
        public string drdramlo { get; set; }
        public string drdramhi { get; set; }
        public string frw1wire { get; set; }
        public string frw1ohlo { get; set; }
        public string frw1ohhi { get; set; }
        public string frw2wire { get; set; }
        public string frw2ohlo { get; set; }
        public string frw2ohhi { get; set; }
        public string frfwohlo { get; set; }
        public string frfwohhi { get; set; }
        public string frmuwire { get; set; }
        public string frmuohlo { get; set; }
        public string frmuohhi { get; set; }
        public string frstwire { get; set; }
        public string frstohlo { get; set; }
        public string frstohhi { get; set; }
        public string frtmohlo { get; set; }
        public string frtmohhi { get; set; }
        public string frtsohlo { get; set; }
        public string frtsohhi { get; set; }
        public string frtfohlo { get; set; }
        public string frtfohhi { get; set; }
        public string frtfamlo { get; set; }
        public string frtfamhi { get; set; }
        public string ltampslo { get; set; }
        public string ltampshi { get; set; }
        public string suflamlo { get; set; }
        public string suflamhi { get; set; }
        public string sudfamlo { get; set; }
        public string sudfamhi { get; set; }
        public string sudlamlo { get; set; }
        public string sudlamhi { get; set; }
        public string sumxamhe { get; set; }
        public string sumxamlt { get; set; }
        public string sumxamto { get; set; }
        public string surtamhe { get; set; }
        public string surtamlt { get; set; }

        #endregion

        #region Constructors

        public PAData()
        {
            IsFrame = false;
            IsDoor = false;
            drfrwire = "";
            drfrohlo = "";
            drfrohhi = "";
            drfrohhi = "";
            drglohlo = "";
            drglohhi = "";
            drdrohlo = "";
            drdrohhi = "";
            drdramlo = "";
            drdramhi = "";
            frw1wire = "";
            frw1ohlo = "";
            frw1ohhi = "";
            frw2wire = "";
            frw2ohlo = "";
            frw2ohhi = "";
            frfwohlo = "";
            frfwohhi = "";
            frmuwire = "";
            frmuohlo = "";
            frmuohhi = "";
            frstwire = "";
            frstohlo = "";
            frstohhi = "";
            frtmohlo = "";
            frtmohhi = "";
            frtsohlo = "";
            frtsohhi = "";
            frtfohlo = "";
            frtfohhi = "";
            frtfamlo = "";
            frtfamhi = "";
            ltampslo = "";
            ltampshi = "";
            suflamlo = "";
            suflamhi = "";
            sudfamlo = "";
            sudfamhi = "";
            sudlamlo = "";
            sudlamhi = "";
            sumxamhe = "";
            sumxamlt = "";
            sumxamto = "";
            surtamhe = "";
            surtamlt = "";
        }

        public PAData(bool isframe, bool isdoor, string _drfrwire, string _drfrohlo, string _drfrohhi, string _drglohlo, string _drglohhi, string _drdrohlo, string _drdrohhi, string _drdramlo, string _drdramhi, string _frw1wire, string _frw1ohlo, 
            string _frw1ohhi, string _frw2wire, string _frw2ohlo, string _frw2ohhi, string _frfwohlo, string _frfwohhi, string _frmuwire, string _frmuohlo, string _frmuohhi, string _frstwire, string _frstohlo, string _frstohhi, string _frtmohlo, 
            string _frtmohhi, string _frtsohlo, string _frtsohhi, string _frtfohlo, string _frtfohhi, string _frtfamlo, string _frtfamhi, string _ltampslo, string _ltampshi, string _suflamlo, string _suflamhi, string _sudfamlo, string _sudfamhi, 
            string _sudlamlo, string _sudlamhi, string _sumxamhe, string _sumxamlt, string _sumxamto, string _surtamhe, string _surtamlt)
        {
            IsFrame = isframe;
            IsDoor = isdoor;
            drfrwire = _drfrwire;
            drfrohlo = _drfrohlo;
            drfrohhi = _drfrohhi;
            drglohlo = _drglohlo;
            drglohhi = _drglohhi;
            drdrohlo = _drdrohlo;
            drdrohhi = _drdrohhi;
            drdramlo = _drdramlo;
            drdramhi = _drdramhi;
            frw1wire = _frw1wire;
            frw1ohlo = _frw1ohlo;
            frw1ohhi = _frw1ohhi;
            frw2wire = _frw2wire;
            frw2ohlo = _frw2ohlo;
            frw2ohhi = _frw2ohhi;
            frfwohlo = _frfwohlo;
            frfwohhi = _frfwohhi;
            frmuwire = _frmuwire;
            frmuohlo = _frmuohlo;
            frmuohhi = _frmuohhi;
            frstwire = _frstwire;
            frstohlo = _frstohlo;
            frstohhi = _frstohhi;
            frtmohlo = _frtmohlo;
            frtmohhi = _frtmohhi;
            frtsohlo = _frtsohlo;
            frtsohhi = _frtsohhi;
            frtfohlo = _frtfohlo;
            frtfohhi = _frtfohhi;
            frtfamlo = _frtfamlo;
            frtfamhi = _frtfamhi;
            ltampslo = _ltampslo;
            ltampshi = _ltampshi;
            suflamlo = _suflamlo;
            suflamhi = _suflamhi;
            sudfamlo = _sudfamlo;
            sudfamhi = _sudfamhi;
            sudlamlo = _sudlamlo;
            sudlamhi = _sudlamhi;
            sumxamhe = _sumxamhe;
            sumxamlt = _sumxamlt;
            sumxamto = _sumxamto;
            surtamhe = _surtamhe;
            surtamlt = _surtamlt;
        }

        #endregion
    }

    #endregion

    public class ProductionInterface
    {
        #region Public Methods

        #region Retrieve Methods

        public List<OneDayDetail> GetOneDayDetail(string server, string database, string username, string password, string company, string lookupdate, string prior)
        {
            List<OneDayDetail> result = new List<OneDayDetail>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_JobAsmbl_FrameandDoor_Counts_OneDay_Detail @Company, @LookupDate, @Prior");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("LookupDate", lookupdate);
            sqlCommand.Parameters.AddWithValue("Prior", prior);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new OneDayDetail(row["Job Number"].ToString(), row["Assembly"].ToString(), row["Doors"].ToString(), row["Frames"].ToString(), row["Config Part"].ToString(), row["Frame-Door Openings"].ToString(), row["Part"].ToString(), row["Description"].ToString(), row["Picture URL"].ToString(), row["CRD Date"].ToString(), row["Special Instr"].ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<SOBasedSchedule> GetSOBasedSchedule(string server, string database, string username, string password, string company)
        {
            List<SOBasedSchedule> result = new List<SOBasedSchedule>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_SOBasedScheduleOuterJoin @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.CommandTimeout = 30000;
            List<string> recids = new List<string>();

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                SOBasedSchedule currRow = new SOBasedSchedule();

                int index = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SOBasedSchedule nextRow = new SOBasedSchedule(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString(), row[9].ToString(), row[10].ToString(),
                        row[11].ToString(), row[12].ToString(), row[13].ToString(), row[14].ToString(), row[15].ToString(), row[16].ToString(), row[17].ToString(), row[18].ToString(), row[19].ToString(), row[20].ToString(), row[21].ToString(), row[22].ToString(),
                        row[23].ToString(), row[24].ToString(), row[25].ToString(), row[26].ToString(), row[27].ToString(), row[28].ToString(), row[29].ToString(), row[30].ToString(), row[31].ToString(), row[32].ToString(), row[34].ToString(), row[35].ToString(),
                        row[36].ToString(), row[37].ToString());

                    if (nextRow.JobNum != currRow.JobNum || nextRow.AssemblySeq != currRow.AssemblySeq || nextRow.OprSeq != currRow.OprSeq)
                    {
                        if (!recids.Contains(nextRow.Progress_recid))
                        {
                            recids.Add(nextRow.Progress_recid);
//                            SqlCommand noticeCommand = new SqlCommand("exec [dbo].sp_EmailJustin @Message");
  //                          noticeCommand.Parameters.AddWithValue("Message", "Company = " + company + ", Inserting progress rec id" + nextRow.Progress_recid + " for job " + nextRow.JobNum + " asm " + nextRow.AssemblySeq + " opr " + nextRow.OprSeq + ". Index : " + index.ToString());
    //                        SQLAccess.NonQuery(server, database, username, password, noticeCommand);
                        }
                        else
                        {
                            try
                            {
                                SqlCommand noticeCommand = new SqlCommand("exec [dbo].sp_EmailJustin @Message");
                                noticeCommand.Parameters.AddWithValue("Message", "Company = " + company + ", Yo, the SO sync is trying to insert duplicate progress ID " + nextRow.Progress_recid + " for job " + nextRow.JobNum + " asm " + nextRow.AssemblySeq + " opr " + nextRow.OprSeq + " at index: " + index.ToString() + ". Curr Row: " + currRow.JobNum + ", " + currRow.AssemblySeq + ", " + currRow.OprSeq + "," + currRow.Progress_recid + ". Next Row: " + nextRow.JobNum + ", " + nextRow.AssemblySeq + ", " + nextRow.OprSeq + ", " + nextRow.Progress_recid);
                                SQLAccess.NonQuery(server, database, username, password, noticeCommand);
                            }
                            catch
                            {
                            }
                        }
                        currRow = nextRow;
                        result.Add(currRow);
                    }

                    if (!String.IsNullOrEmpty(row[33].ToString()))
                        currRow.ActiveEmployees.Add(row[33].ToString());

                    index++;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetSOBasedSchedule - " + ex.Message);
            }

            return result;
        }

        public List<SOBasedSchedule> GetSOBasedScheduleDetail(string server, string database, string username, string password, string company, string jobnum, int asmseq, int oprseq)
        {
            List<SOBasedSchedule> result = new List<SOBasedSchedule>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_SOBasedScheduleOuterJoin @Company, @Jobnum, @AssemblySeq, @OprSeq");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("Jobnum", jobnum);
            sqlCommand.Parameters.AddWithValue("AssemblySeq", asmseq);
            sqlCommand.Parameters.AddWithValue("OprSeq", oprseq);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                SOBasedSchedule currRow = new SOBasedSchedule();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SOBasedSchedule nextRow = new SOBasedSchedule(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString(), row[9].ToString(), row[10].ToString(),
                        row[11].ToString(), row[12].ToString(), row[13].ToString(), row[14].ToString(), row[15].ToString(), row[16].ToString(), row[17].ToString(), row[18].ToString(), row[19].ToString(), row[20].ToString(), row[21].ToString(), row[22].ToString(),
                        row[23].ToString(), row[24].ToString(), row[25].ToString(), row[26].ToString(), row[27].ToString(), row[28].ToString(), row[29].ToString(), row[30].ToString(), row[31].ToString(), row[32].ToString(), row[34].ToString(), row[35].ToString(),
                        row[36].ToString(), row[37].ToString());

                    if (nextRow.JobNum != currRow.JobNum || nextRow.AssemblySeq != currRow.AssemblySeq || nextRow.OprSeq != currRow.OprSeq)
                    {
                        currRow = nextRow;
                        result.Add(currRow);
                    }

                    if (!String.IsNullOrEmpty(row[33].ToString()))
                        currRow.ActiveEmployees.Add(row[33].ToString());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<ActiveLabor> GetActiveLabor(string server, string database, string username, string password, string company)
        {
            List<ActiveLabor> result = new List<ActiveLabor>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetActiveLabor @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.Add(new ActiveLabor(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<IndirectActivity> GetIndirectActivity(string server, string database, string username, string password, string company)
        {
            List<IndirectActivity> result = new List<IndirectActivity>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetIndirectActivity @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.Add(new IndirectActivity(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public DataSet GetActiveLaborRaw(string server, string database, string username, string password, string company)
        {
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetActiveLabor @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MESInfo> GetMESInfoForJob(string server, string database, string username, string password, string company, string jobnum)
        {
            List<MESInfo> result = new List<MESInfo>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_UD03_ShopViewAllMES @Company, @VCLink");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("VCLink", jobnum);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.Add(new MESInfo(row[0].ToString(), row[1].ToString(), !String.IsNullOrEmpty(row[2].ToString())));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<MESDrawing> GetMESDrawings(string server, string database, string username, string password, string company, string mesnum)
        {
            List<MESDrawing> result = new List<MESDrawing>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetMESDrawingLocation @Company, @MESNum");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("MESNum", mesnum);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string directory = row[0].ToString();
                    DirectoryInfo di = new DirectoryInfo(directory);
                    FileInfo[] files = di.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        result.Add(new MESDrawing(file.Name, file.FullName, File.ReadAllBytes(file.FullName)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public PAData GetPADataForJob(string server, string database, string username, string password, string company, string jobnum, int assemblyseq)
        {
            PAData result = new PAData();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPAData @Company, @JobNum, @AssemblySeq");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("JobNum", jobnum);
            sqlCommand.Parameters.AddWithValue("AssemblySeq", assemblyseq);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                DataRow row = ds.Tables[0].Rows[0];
                result = new PAData(row[0].ToString() == "1", row[1].ToString() == "1", row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString(), row[9].ToString(), row[10].ToString(), row[11].ToString(),
                    row[12].ToString(), row[13].ToString(), row[14].ToString(), row[15].ToString(), row[16].ToString(), row[17].ToString(), row[18].ToString(), row[19].ToString(), row[20].ToString(), row[21].ToString(), row[22].ToString(), row[23].ToString(),
                    row[24].ToString(), row[25].ToString(), row[26].ToString(), row[27].ToString(), row[28].ToString(), row[29].ToString(), row[30].ToString(), row[31].ToString(), row[32].ToString(), row[33].ToString(), row[34].ToString(), row[35].ToString(),
                    row[36].ToString(), row[37].ToString(), row[38].ToString(), row[39].ToString(), row[40].ToString(), row[41].ToString(), row[42].ToString(), row[43].ToString(), row[44].ToString(), row[45].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }

        #endregion

        #endregion
    }
}
