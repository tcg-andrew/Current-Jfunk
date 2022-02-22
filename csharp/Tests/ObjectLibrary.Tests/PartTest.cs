// <copyright file="PartTest.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectLibrary;

namespace ObjectLibrary
{
    [TestClass]
    [PexClass(typeof(Part))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class PartTest
    {
        [PexMethod]
        public Part Constructor01(
            string partnum,
            string desc,
            string type,
            bool nonstock,
            string group,
            string cl,
            string unit,
            decimal price,
            string plant,
            decimal usage,
            decimal minqty,
            decimal pctdiff,
            decimal freightclass,
            decimal weight,
            decimal avgcost
        )
        {
            Part target
               = new Part(partnum, desc, type, nonstock, group, cl, unit, price, plant, usage, minqty, pctdiff, freightclass, weight, avgcost);
            return target;
        }
        [PexMethod]
        public Part Constructor()
        {
            Part target = new Part();
            return target;
        }
    }
}
