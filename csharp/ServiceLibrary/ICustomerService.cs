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

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/customerservice")]
    public interface ICustomerService
    {
        [OperationContract]
        customergetallresult getallcustomers();

        [OperationContract]
        customergetsingleresult getcustomerbycustid(string custid);

        [OperationContract]
        customergetspecialinstructionresult getspecialinstruction(string custid);
    }

    #endregion

    #region DataContract

    [DataContract(Namespace = "http://services.it.tcg/epicor/customerservice")]
    public class customergetsingleresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<customer> epicor { get; set; }

        public customergetsingleresult()
        {
            epicor = new List<customer>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/customerservice")]
    public class customergetallresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<GetAllCustomers.customer> epicor { get; set; }

        public customergetallresult()
        {
            epicor = new List<GetAllCustomers.customer>();
        }
    }

    [DataContract(Namespace = "http://services.it.tcg/epicor/customerservice")]
    public class customergetspecialinstructionresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<string> epicor { get; set; }

        public customergetspecialinstructionresult()
        {
            epicor = new List<string>();
        }
    }

    #endregion

    #region Data Formats

    namespace GetAllCustomers
    {
        public class customer
        {
            #region Properties

            public string custid { get; set; }
            public string custnum { get; set; }
            public string name { get; set; }
            public string phonenum { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }

            #endregion

            #region Constructors

            public customer()
            {
                custid = "";
                custnum = "";
                name = "";
                phonenum = "";
                city = "";
                state = "";
                zip = "";
            }

            public customer(Customer customer)
            {
                custid = customer.CustID;
                custnum = customer.CustNum;
                name = customer.Name;
                phonenum = customer.PhoneNumber;
                city = customer.Address.City;
                state = customer.Address.State;
                zip = customer.Address.Zip;
            }

            #endregion
        }
    }

    public class customer
    {
        #region Properties

        public string custid { get; set; }
        public string custnum { get; set; }
        public string name { get; set; }
        public string phonenum { get; set; }
        public string faxnum { get; set; }
        public string discountpercent { get; set; }
        public string emailaddress { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string shiptonum { get; set; }
        public string stname { get; set; }
        public string staddress1 { get; set; }
        public string staddress2 { get; set; }
        public string staddress3 { get; set; }
        public string stcity { get; set; }
        public string ststate { get; set; }
        public string stzip { get; set; }
        public string stcountry { get; set; }
        public string specialinstructions { get; set; }
        public string taxexempt { get; set; }
        public string isepackage { get; set; }
        public string salesrepcode { get; set; }
        public string stemail { get; set; }

        #endregion

        #region Constructors

        public customer()
        {
            custid = "";
            custnum = "";
            name = "";
            phonenum = "";
            faxnum = "";
            discountpercent = "";
            emailaddress = "";
            address1 = "";
            address2 = "";
            address3 = "";
            city = "";
            state = "";
            zip = "";
            country = "";
            shiptonum = "";
            stname = "";
            staddress1 = "";
            staddress2 = "";
            staddress3 = "";
            stcity = "";
            ststate = "";
            stzip = "";
            stcountry = "";
            specialinstructions = "";
            taxexempt = "false";
            isepackage = "false";
            salesrepcode = "";
            stemail = "";
        }

        public customer(ObjectLibrary.Customer customer)
        {
            custid = customer.CustID;
            custnum = customer.CustNum;
            name = customer.Name;
            phonenum = customer.PhoneNumber;
            faxnum = customer.FaxNumber;
            discountpercent = customer.DiscountPercent;
            emailaddress = customer.Email;
            address1 = customer.Address.Address1;
            address2 = customer.Address.Address2;
            address3 = customer.Address.Address3;
            city = customer.Address.City;
            state = customer.Address.State;
            zip = customer.Address.Zip;
            country = customer.Address.Country;
            shiptonum = customer.ShipToNum;
            stname = customer.ShipToName;
            staddress1 = customer.ShipToAddress.Address1;
            staddress2 = customer.ShipToAddress.Address2;
            staddress3 = customer.ShipToAddress.Address3;
            stcity = customer.ShipToAddress.City;
            stcountry = customer.ShipToAddress.Country;
            ststate = customer.ShipToAddress.State;
            stzip = customer.ShipToAddress.Zip;
            specialinstructions = customer.SpecialInstructions;
            taxexempt = customer.TaxExempt ? "true" : "false";
            isepackage = customer.ISEPackage ? "true" : "false";
            salesrepcode = customer.SalesRepCode;
            stemail = customer.ShipToEmail;
        }

        #endregion
    }

    #endregion
}
