// <copyright file="ShipToAddressTest.Constructor01.g.cs">Copyright ?  2011</copyright>
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
    public partial class ShipToAddressTest
    {
[TestMethod]
[PexGeneratedBy(typeof(ShipToAddressTest))]
public void Constructor01741()
{
    ShipToAddress shipToAddress;
    shipToAddress =
      this.Constructor01((string)null, (string)null, (string)null, (string)null, 
                         (string)null, (string)null, (string)null, (string)null);
    Assert.IsNotNull((object)shipToAddress);
    Assert.AreEqual<string>((string)null, shipToAddress.ShipToNum);
    Assert.IsNotNull(shipToAddress.Address);
    Assert.AreEqual<string>((string)null, shipToAddress.Address.Address1);
    Assert.AreEqual<string>((string)null, shipToAddress.Address.Address2);
    Assert.AreEqual<string>((string)null, shipToAddress.Address.Address3);
    Assert.AreEqual<string>((string)null, shipToAddress.Address.City);
    Assert.AreEqual<string>((string)null, shipToAddress.Address.State);
    Assert.AreEqual<string>((string)null, shipToAddress.Address.Zip);
    Assert.AreEqual<string>((string)null, shipToAddress.Address.Country);
}
[TestMethod]
[PexGeneratedBy(typeof(ShipToAddressTest))]
public void Constructor731()
{
    ShipToAddress shipToAddress;
    shipToAddress = this.Constructor();
    Assert.IsNotNull((object)shipToAddress);
    Assert.AreEqual<string>("", shipToAddress.ShipToNum);
    Assert.IsNotNull(shipToAddress.Address);
    Assert.AreEqual<string>("", shipToAddress.Address.Address1);
    Assert.AreEqual<string>("", shipToAddress.Address.Address2);
    Assert.AreEqual<string>("", shipToAddress.Address.Address3);
    Assert.AreEqual<string>("", shipToAddress.Address.City);
    Assert.AreEqual<string>("", shipToAddress.Address.State);
    Assert.AreEqual<string>("", shipToAddress.Address.Zip);
    Assert.AreEqual<string>("", shipToAddress.Address.Country);
}
    }
}
