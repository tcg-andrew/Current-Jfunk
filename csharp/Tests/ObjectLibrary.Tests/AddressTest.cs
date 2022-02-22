// <copyright file="AddressTest.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectLibrary;

namespace ObjectLibrary
{
    [TestClass]
    [PexClass(typeof(Address))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class AddressTest
    {
        [PexMethod]
        public Address Constructor01(
            string address1,
            string address2,
            string address3,
            string city,
            string state,
            string zip,
            string country
        )
        {
            Address target = new Address(address1, address2, address3, city, state, zip, country);
            return target;
        }
        [PexMethod]
        public Address Constructor()
        {
            Address target = new Address();
            return target;
        }
    }
}
