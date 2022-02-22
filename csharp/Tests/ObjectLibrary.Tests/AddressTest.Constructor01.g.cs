// <copyright file="AddressTest.Constructor01.g.cs">Copyright �  2011</copyright>
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace ObjectLibrary
{
    public partial class AddressTest
    {
[TestMethod]
[PexGeneratedBy(typeof(AddressTest))]
public void Constructor01242()
{
    Address address;
    address = this.Constructor01((string)null, (string)null, (string)null, 
                                 (string)null, "", (string)null, (string)null);
    Assert.IsNotNull((object)address);
    Assert.AreEqual<string>((string)null, address.Address1);
    Assert.AreEqual<string>((string)null, address.Address2);
    Assert.AreEqual<string>((string)null, address.Address3);
    Assert.AreEqual<string>((string)null, address.City);
    Assert.AreEqual<string>("", address.State);
    Assert.AreEqual<string>((string)null, address.Zip);
    Assert.AreEqual<string>((string)null, address.Country);
}
[TestMethod]
[PexGeneratedBy(typeof(AddressTest))]
public void Constructor570()
{
    Address address;
    address = this.Constructor();
    Assert.IsNotNull((object)address);
    Assert.AreEqual<string>("", address.Address1);
    Assert.AreEqual<string>("", address.Address2);
    Assert.AreEqual<string>("", address.Address3);
    Assert.AreEqual<string>("", address.City);
    Assert.AreEqual<string>("", address.State);
    Assert.AreEqual<string>("", address.Zip);
    Assert.AreEqual<string>("", address.Country);
}
    }
}