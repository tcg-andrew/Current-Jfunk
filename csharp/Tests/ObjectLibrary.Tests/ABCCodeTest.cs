// <copyright file="ABCCodeTest.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectLibrary;

namespace ObjectLibrary
{
    [TestClass]
    [PexClass(typeof(ABCCode))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ABCCodeTest
    {
        [PexMethod]
        public ABCCode Constructor01(
            string code,
            decimal minvol,
            decimal mincost,
            int freq
        )
        {
            ABCCode target = new ABCCode(code, minvol, mincost, freq);
            return target;
        }
        [PexMethod]
        public ABCCode Constructor()
        {
            ABCCode target = new ABCCode();
            return target;
        }
    }
}
