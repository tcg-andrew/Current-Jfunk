#region Usings

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ObjectLibrary;
using System.Configuration;

#endregion

namespace ServiceLibrary
{
    #region ServiceContract

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public interface IPartService
    {
        [OperationContract]
        partgetallresult getallparts(int limit);

        [OperationContract]
        operationgetallresult getalloperations();

        [OperationContract]
        partgetneedingupdateresult getpartsneedingupdate(string plant, string partclass);

        [OperationContract]
        partupdateresult updatepartsminqty(partsminqtyupdaterequest request);

        [OperationContract]
        partgetstringresult getplants();

        [OperationContract]
        partgetclassresult getpartclasses();

        [OperationContract]
        partgetmiscchargeclassresult getmiscchargeclasses();

        [OperationContract]
        partgetpricelistresult getpartpricelist();
    }

    #endregion

    #region DataContract

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class partgetmiscchargeclassresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<misccharge> epicor { get; set; }

        public partgetmiscchargeclassresult()
        {
            epicor = new List<misccharge>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class partgetallresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<part> epicor { get; set; }

        public partgetallresult()
        {
            epicor = new List<part>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class operationgetallresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<operation> epicor { get; set; }

        public operationgetallresult()
        {
            epicor = new List<operation>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class partgetstringresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<string> epicor {get;set;}

        public partgetstringresult()
        {
            epicor = new List<string>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class partgetclassresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<partclass> epicor { get; set; }

        public partgetclassresult()
        {
            epicor = new List<partclass>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class partgetneedingupdateresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<NeedingUpdate.part> epicor { get; set; }

        public partgetneedingupdateresult()
        {
            epicor = new List<NeedingUpdate.part>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class partsminqtyupdaterequest
    {
        [DataMember(Order = 0)]
        public string username { get; set; }

        [DataMember(Order = 1)]
        public string password { get; set; }

        [DataMember(Order = 2)]
        public List<NeedingUpdate.part> epicor { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class partupdateresult
    {
        [DataMember]
        public string exception { get; set; }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/partservice")]
    public class partgetpricelistresult
    {
        [DataMember(Order = 0)]
        public string exception {get;set;}

        [DataMember(Order = 1)]
        public List<partpricelist> epicor { get; set; }

        public partgetpricelistresult()
        {
            epicor = new List<partpricelist>();
        }
    }

    #endregion

    #region Data Formats

    namespace NeedingUpdate
    {
        public class part
        {
            #region Properties

            public string partnum { get; set; }
            public string desc { get; set; }
            public string plant { get; set; }
            public decimal monthlyusage { get; set; }
            public decimal minimumqty { get; set; }
            public decimal percentdiff { get; set; }
            public decimal avgcost { get; set; }

            #endregion

            #region Constructors

            public part()
            {
                partnum = "";
                desc = "";
                plant = "";
                monthlyusage = 0;
                minimumqty = 0;
                percentdiff = 0;
                avgcost = 0;
            }

            public part(Part part)
            {
                partnum = part.PartNum;
                desc = part.Desc;
                plant = part.Plant;
                monthlyusage = part.MonthlyUsage;
                minimumqty = part.MinimumQty;
                percentdiff = part.PercentDiff;
                avgcost = part.AvgCost;
            }

            #endregion
        }
    }

    public class part
    {
        #region Properties

        public string partnum { get; set; }
        public string desc { get; set; }
        public string type { get; set; }
        public bool nonstock { get; set; }
        public string group { get; set; }
        public string partclass { get; set; }
        public string unit { get; set; }
        public decimal price { get; set; }
        public decimal freightclass { get; set; }
        public decimal weight { get; set; }

        #endregion

        #region Constructors

        public part()
        {
            partnum = "";
            desc = "";
            type = "";
            nonstock = false;
            group = "";
            partclass = "";
            unit = "";
            price = 0;
            freightclass = 0;
            weight = 0;
        }

        public part(ObjectLibrary.Part part)
        {
            partnum = part.PartNum;
            desc = part.Desc;
            type = part.Type;
            nonstock = part.NonStock;
            group = part.Group;
            partclass = part.Class;
            unit = part.Unit;
            price = part.Price;
            freightclass = part.FreightClass;
            weight = part.Weight;
        }

        #endregion
    }

    public class operation
    {
        #region Properties

        public string opcode { get; set; }
        public string desc { get; set; }

        #endregion

        #region Constructors

        public operation()
        {
            opcode = "";
            desc = "";
        }

        public operation(ObjectLibrary.Operation operation)
        {
            opcode = operation.OpCode;
            desc = operation.Desc;
        }
        #endregion
    }

    public class partclass
    {
        #region Properties

        public string id { get; set; }
        public string description { get; set; }

        #endregion

        #region Constructors

        public partclass()
        {
            id = "";
            description = "";
        }

        public partclass(PartClass p)
        {
            id = p.ID;
            description = p.Description;
        }

        #endregion
    }

    public class partpricelist
    {
        #region Properties

        public string custnum { get; set; }
        public string custid { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string partnum { get; set; }
        public decimal price { get; set; }

        #endregion

        #region Constructors

        public partpricelist()
        {
            custnum = "";
            custid = "";
            startdate = "";
            enddate = "";
            partnum = "";
            price = 0.0m;
        }

        public partpricelist(PartPriceList p)
        {
            custnum = p.CustNum;
            custid = p.CustID;
            startdate = p.StartDate;
            enddate = p.EndDate;
            partnum = p.PartNum;
            price = p.Price;
        }

        #endregion
    }

    public class misccharge
    {
        #region Properties
        public string code { get; set; }
        public string desc { get; set; }
        #endregion

        #region Constructors

        public misccharge()
        {
            code = "";
            desc = "";
        }

        public misccharge(MiscCharge charge)
        {
            code = charge.Code;
            desc = charge.Desc;
        }

        #endregion
    }
    #endregion
}
