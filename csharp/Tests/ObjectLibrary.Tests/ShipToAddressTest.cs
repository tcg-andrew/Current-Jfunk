// <copyright file="ShipToAddressTest.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectLibrary;

namespace ObjectLibrary
{
    [TestClass]
    [PexClass(typeof(ShipToAddress))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ShipToAddressTest
    {
        [PexMethod]
        public ShipToAddress Constructor01(
            string shiptonum,
            string address1,
            string address2,
            string address3,
            string city,
            string state,
            string zip,
            string country
        )
        {
            ShipToAddress target
               = new ShipToAddress(shiptonum, address1, address2, address3, city, state, zip, country);
            return target;
        }
        [PexMethod]
        public ShipToAddress Constructor()
        {
            ShipToAddress target = new ShipToAddress();
            return target;
        }
    }
}
