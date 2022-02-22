#region Usings

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ObjectLibrary;

#endregion

namespace ServiceLibrary
{
    #region ServiceContract

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public interface IProductionService
    {
        [OperationContract]
        onedaydetailgetresult getonedaydetail(string company, string lookupdate, string prior);

        [OperationContract]
        sobasedschedulegetresult getsobasedschedule(string company);

        [OperationContract]
        activelaborgetresult getactivelabor(string company);

        [OperationContract]
        indirectactivitygetresult getindirectactivity(string company);

        [OperationContract]
        mesinfogetresult getmesinfo(string company, string jobnum);

        [OperationContract]
        mesdrawinggetresult getmesdrawing(string company, string mesnum);

        [OperationContract]
        padatagetresult getpadata(string company, string jobnum, int assemblyseq);
    }

    #endregion

    #region DataContract

    [DataContract(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public class onedaydetailgetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<onedaydetail> epicor { get; set; }

        public onedaydetailgetresult()
        {
            epicor = new List<onedaydetail>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public class sobasedschedulegetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<sobasedschedule> epicor { get; set; }

        public sobasedschedulegetresult()
        {
            epicor = new List<sobasedschedule>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public class activelaborgetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<activelabor> epicor { get; set; }

        public activelaborgetresult()
        {
            epicor = new List<activelabor>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public class indirectactivitygetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<indirectactivity> epicor { get; set; }

        public indirectactivitygetresult()
        {
            epicor = new List<indirectactivity>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public class mesinfogetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<mesinfo> epicor { get; set; }

        public mesinfogetresult()
        {
            epicor = new List<mesinfo>();
        }

    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public class mesdrawinggetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<mesdrawing> epicor { get; set; }

        public mesdrawinggetresult()
        {
            epicor = new List<mesdrawing>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/productionservice")]
    public class padatagetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<padata> epicor { get; set; }

        public padatagetresult()
        {
            epicor = new List<padata>();
        }
    }

    #endregion

    #region Data Formats

    public class onedaydetail
    {
        #region properties

        public string jobnum { get; set; }
        public string assembly { get; set; }
        public string doors { get; set; }
        public string frames { get; set; }
        public string configpart { get; set; }
        public string openings { get; set; }
        public string part { get; set; }
        public string description { get; set; }
        public string imageurl { get; set; }
        public string crddate { get; set; }
        public string specialinstructions { get; set; }

        #endregion

        #region Constructors

        public onedaydetail()
        {
            jobnum = "";
            assembly = "";
            doors = "";
            frames = "";
            configpart = "";
            openings = "";
            part = "";
            description = "";
            imageurl = "";
            crddate = "";
            specialinstructions = "";
        }

        public onedaydetail(OneDayDetail o)
        {
            jobnum = o.Jobnum;
            assembly = o.Assembly;
            doors = o.Doors;
            frames = o.Frames;
            configpart = o.ConfigPart;
            openings = o.Openings;
            part = o.Part;
            description = o.Description;
            imageurl = o.ImageUrl;
            crddate = o.CRDDate;
            specialinstructions = o.SpecialInstructions;
        }

        #endregion
    }

    public class sobasedschedule
    {
        #region Properties

        public string ponum { get; set; }
        public string name { get; set; }
        public string custid { get; set; }
        public string linedesc { get; set; }
        public string needbydate { get; set; }
        public string openrelease { get; set; }
        public string orderline { get; set; }
        public string ordernum { get; set; }
        public string orderrelnum { get; set; }
        public string partnum { get; set; }
        public string reqdate { get; set; }
        public string revisionnum { get; set; }
        public string character05 { get; set; }
        public string ourreqqty { get; set; }
        public string assemblyseq { get; set; }
        public string character01 { get; set; }
        public string opcode { get; set; }
        public string oprseq { get; set; }
        public string qtycompleted { get; set; }
        public string runqty { get; set; }
        public string number01 { get; set; }
        public string ourjobshippedqty { get; set; }
        public string ourstockshippedqty { get; set; }
        public string jobasmcommenttext { get; set; }
        public string shortchar01 { get; set; }
        public string jobnum { get; set; }
        public string jobheadcommenttext { get; set; }
        public string character07 { get; set; }
        public string drawnum { get; set; }
        public string shipviacode { get; set; }
        public string crddate { get; set; }
        public string outstandingqty { get; set; }
        public string picurl { get; set; }
        public List<string> activeemployees { get; set; }

        #endregion

        #region Constructors

        public sobasedschedule()
        {
            ponum = "";
            name = "";
            custid = "";
            linedesc = "";
            needbydate = "";
            openrelease = "";
            orderline = "";
            ordernum = "";
            orderrelnum = "";
            partnum = "";
            reqdate = "";
            revisionnum = "";
            character05 = "";
            ourreqqty = "";
            assemblyseq = "";
            character01 = "";
            opcode = "";
            oprseq = "";
            qtycompleted = "";
            runqty = "";
            number01 = "";
            ourjobshippedqty = "";
            ourstockshippedqty = "";
            jobasmcommenttext = "";
            shortchar01 = "";
            jobnum = "";
            jobheadcommenttext = "";
            character07 = "";
            drawnum = "";
            shipviacode = "";
            crddate = "";
            outstandingqty = "";
            picurl = "";
            activeemployees = new List<string>();
        }

        public sobasedschedule(SOBasedSchedule o)
        {
            ponum = o.PONum;
            name = o.Name;
            custid = o.CustID;
            linedesc = o.LineDesc;
            needbydate = o.NeedByDate;
            openrelease = o.OpenRelease;
            orderline = o.OrderLine;
            ordernum = o.OrderNum;
            orderrelnum = o.OrderRelNum;
            partnum = o.PartNum;
            reqdate = o.ReqDate;
            revisionnum = o.RevisionNum;
            character05 = o.Character05;
            ourreqqty = o.OurReqQty;
            assemblyseq = o.AssemblySeq;
            character01 = o.Character01;
            opcode = o.OpCode;
            oprseq = o.OprSeq;
            qtycompleted = o.QtyCompleted;
            runqty = o.RunQty;
            number01 = o.Number01;
            ourjobshippedqty = o.OurJobShippedQty;
            ourstockshippedqty = o.OurStockShippedQty;
            jobasmcommenttext = o.JobAsmCommentText;
            shortchar01 = o.ShortChar01;
            jobnum = o.JobNum;
            jobheadcommenttext = o.JobHeadCommentText;
            character07 = o.Character07;
            drawnum = o.DrawNum;
            shipviacode = o.ShipViaCode;
            crddate = o.CRDDate;
            outstandingqty = o.OutstandingQty;
            picurl = o.PicUrl;
            activeemployees = o.ActiveEmployees;
        }

        #endregion
    }

    public class activelabor
    {
        #region Properties

        public string employeeid { get; set; }
        public string jobnum { get; set; }
        public string assemblyseq { get; set; }
        public string oprseq { get; set; }
        public string oprcode { get; set; }
        public string employeename { get; set; }
        public string revision { get; set; }

        #endregion

        #region Constructors

        public activelabor()
        {
            employeeid = "";
            jobnum = "";
            assemblyseq = "";
            oprseq = "";
            oprcode = "";
            employeename = "";
            revision = "";
        }

        public activelabor(ActiveLabor al)
        {
            employeeid = al.EmployeeID;
            jobnum = al.JobNum;
            assemblyseq = al.AssemblySeq;
            oprseq = al.OprSeq;
            oprcode = al.OprCode;
            employeename = al.EmployeeName;
            revision = al.Revision;
        }

        #endregion
    }

    public class indirectactivity
    {
        #region Properties

        public string employeeid { get; set; }
        public string employeename { get; set; }
        public string indirectcode { get; set; }
        public string resourcegrpid { get; set; }

        #endregion

        #region Constructors

        public indirectactivity()
        {
            employeeid = "";
            employeename = "";
            indirectcode = "";
            resourcegrpid = "";
        }

        public indirectactivity(IndirectActivity ia)
        {
            employeeid = ia.EmployeeID;
            employeename = ia.EmployeeName;
            indirectcode = ia.IndirectCode;
            resourcegrpid = ia.ResourceGrpId;
        }

        #endregion

    }
    public class mesinfo
    {
        #region Properties

        public string mesnum { get; set; }
        public string description { get; set; }
        public string hasdrawings { get; set; }

        #endregion

        #region Constructors

        public mesinfo()
        {
            mesnum = "";
            description = "";
            hasdrawings = "";
        }

        public mesinfo(MESInfo mes)
        {
            mesnum = mes.MESNum;
            description = mes.Description;
            hasdrawings = mes.HasDrawings.ToString();
        }

        #endregion
    }

    public class mesdrawing
    {
        #region Properties

        public string name { get; set; }
        public string file { get; set; }
        public byte[] filedata { get; set; }

        #endregion

        #region Constructors

        public mesdrawing()
        {
            name = "";
            file = "";
            filedata = null;
        }

        public mesdrawing(MESDrawing md)
        {
            name = md.Name;
            file = md.File;
            filedata = md.FileData;
        }

        #endregion
    }

    public class padata
    {
        #region Properties

        public bool isdoor { get; set; }
        public bool isframe { get; set; }
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

        public padata()
        {
            isdoor = false;
            isframe = false;
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

        public padata(PAData pa)
        {
            isframe = pa.IsFrame;
            isdoor = pa.IsDoor;
            drfrwire = pa.drfrwire;
            drfrohlo = pa.drfrohlo;
            drfrohhi = pa.drfrohhi;
            drglohlo = pa.drglohlo;
            drglohhi = pa.drglohhi;
            drdrohlo = pa.drdrohlo;
            drdrohhi = pa.drdrohhi;
            drdramlo = pa.drdramlo;
            drdramhi = pa.drdramhi;
            frw1wire = pa.frw1wire;
            frw1ohlo = pa.frw1ohlo;
            frw1ohhi = pa.frw1ohhi;
            frw2wire = pa.frw2wire;
            frw2ohlo = pa.frw2ohlo;
            frw2ohhi = pa.frw2ohhi;
            frfwohlo = pa.frfwohlo;
            frfwohhi = pa.frfwohhi;
            frmuwire = pa.frmuwire;
            frmuohlo = pa.frmuohlo;
            frmuohhi = pa.frmuohhi;
            frstwire = pa.frstwire;
            frstohlo = pa.frstohlo;
            frstohhi = pa.frstohhi;
            frtmohlo = pa.frtmohlo;
            frtmohhi = pa.frtmohhi;
            frtsohlo = pa.frtsohlo;
            frtsohhi = pa.frtsohhi;
            frtfohlo = pa.frtfohlo;
            frtfohhi = pa.frtfohhi;
            frtfamlo = pa.frtfamlo;
            frtfamhi = pa.frtfamhi;
            ltampslo = pa.ltampslo;
            ltampshi = pa.ltampshi;
            suflamlo = pa.suflamlo;
            suflamhi = pa.suflamhi;
            sudfamlo = pa.sudfamlo;
            sudfamhi = pa.sudfamhi;
            sudlamlo = pa.sudlamlo;
            sudlamhi = pa.sudlamhi;
            sumxamhe = pa.sumxamhe;
            sumxamlt = pa.sumxamlt;
            sumxamto = pa.sumxamto;
            surtamhe = pa.surtamhe;
            surtamlt = pa.surtamlt;
        }

        #endregion
    }

    #endregion
}
