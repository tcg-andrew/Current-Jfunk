#region Usings

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ObjectLibrary;
using System;

#endregion

namespace ServiceLibrary
{
    #region ServiceContract

    [ServiceContract(Namespace="http://services.it.tcg/epicor/shiptoservice")]
    public interface IShipToService
    {
        [OperationContract]
        shiptogetresult getshiptobycustid(string custid);

        [OperationContract]
        salesrepgetresult getsalesreplist(string company);

/*        [OperationContract]
        shiptogetresult getsingleshipto(string shiptonum, int custnum);
*/    }

    #endregion

    #region DataContract

    [DataContract(Namespace = "http://services.it.tcg/epicor/shiptoservice")]
    public class shiptogetresult
    {
        [DataMember(Order=0)]
        public string exception { get; set; }

        [DataMember(Order=1)]
        public List<shipto> epicor { get; set; }

        public shiptogetresult()
        {
            epicor = new List<shipto>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/shiptoservice")]
    public class salesrepgetresult
    {
        [DataMember(Order=0)]
        public string exception { get; set; }

        [DataMember(Order=1)]
        public List<salesrep> epicor { get; set; }

        public salesrepgetresult()
        {
            epicor = new List<salesrep>();
        }
    }
    #endregion

    #region Data Formats

    public class shipto
    {
        #region Properties

        public string shiptonum { get; set; }
        public string stname { get; set; }
        public string staddress1 { get; set; }
        public string staddress2 { get; set; }
        public string staddress3 { get; set; }
        public string stcity { get; set; }
        public string ststate { get; set; }
        public string stzip { get; set; }
        public string stcountry { get; set; }
        public string stphone { get; set; }
        public string stemail { get; set; }
        public string stemailcc { get; set; }

        #endregion

        #region Constructors

        public shipto()
        {
            shiptonum = "";
            stname = "";
            staddress1 = "";
            staddress2 = "";
            staddress3 = "";
            stcity = "";
            ststate = "";
            stzip = "";
            stcountry = "";
            stphone = "";
            stemail = "";
            stemailcc = "";
        }

        public shipto(ShipToAddress shipToAddress)
        {
            shiptonum = shipToAddress.ShipToNum;
            stname = shipToAddress.Name;
            staddress1 = shipToAddress.Address.Address1;
            staddress2 = shipToAddress.Address.Address2;
            staddress3 = shipToAddress.Address.Address3;
            stcity = shipToAddress.Address.City;
            ststate = shipToAddress.Address.State;
            stzip = shipToAddress.Address.Zip;
            stcountry = shipToAddress.Address.Country;
            stphone = shipToAddress.PhoneNum;
            stemail = shipToAddress.Email;
            stemailcc = shipToAddress.EmailCC;
        }

        #endregion
    }

    public class salesrep
    {
        #region Properties

        public string name { get; set; }
        public string repcode { get; set; }

        #endregion

        #region Constructors

        public salesrep()
        {
            name = "";
            repcode = "";
        }

        public salesrep(SalesRep s)
        {
            name = s.Name;
            repcode = s.RepCode;
        }

        #endregion
    }
    #endregion
}
