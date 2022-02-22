// <copyright file="CustomerTest.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectLibrary;

namespace ObjectLibrary
{
    [TestClass]
    [PexClass(typeof(Customer))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class CustomerTest
    {
        [PexMethod]
        public Customer Constructor01(
            string custid,
            string custnum,
            string name,
            string phonenumber,
            string faxnumber,
            string discountpercent,
            string email,
            string contactname,
            string address1,
            string address2,
            string address3,
            string city,
            string state,
            string zip,
            string country,
            string staddress1,
            string staddress2,
            string staddress3,
            string stcity,
            string ststate,
            string stzip,
            string stcountry,
            string specialinstructions
        )
        {
            Customer target = new Customer(custid, custnum, name, phonenumber, faxnumber,
                                           discountpercent, email, contactname, address1, address2, address3,
                                           city, state, zip, country, staddress1, staddress2,
                                           staddress3, stcity, ststate, stzip, stcountry, specialinstructions);
            return target;
        }
        [PexMethod]
        public Customer Constructor()
        {
            Customer target = new Customer();
            return target;
        }
    }
}
