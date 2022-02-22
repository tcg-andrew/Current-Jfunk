// <copyright file="PexAssemblyInfo.cs">Copyright ©  2011</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Moles;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("ObjectLibrary")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.BO.Configuration")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.IF.IQuote")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.IF.IPart")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.IF.IQuoteAsm")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.BO.QuoteAsm")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.IF.IConfiguration")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.Core.BLConnectionPool")]
[assembly: PexInstrumentAssembly("System.Data")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.BO.Part")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.Core.Session")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.BO.Quote")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.IF.IAbcCode")]
[assembly: PexInstrumentAssembly("Epicor.Mfg.BO.AbcCode")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.BO.Configuration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.IF.IQuote")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.IF.IPart")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.IF.IQuoteAsm")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.BO.QuoteAsm")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.IF.IConfiguration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.Core.BLConnectionPool")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Data")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.BO.Part")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.Core.Session")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.BO.Quote")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.IF.IAbcCode")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Epicor.Mfg.BO.AbcCode")]

// Microsoft.Pex.Framework.Moles
[assembly: PexAssumeContractEnsuresFailureAtBehavedSurface]
[assembly: PexChooseAsBehavedCurrentBehavior]

