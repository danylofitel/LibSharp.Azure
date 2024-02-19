// Copyright (c) LibSharp. All rights reserved.

using LibSharp.Azure.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibSharp.Azure.UnitTests.ApplicationInsights
{
    [TestClass]
    public class LevelExtensionsUnitTests
    {
        [TestMethod]
        public void ToSeverityLevelTest()
        {
            // Assert
            Assert.AreEqual(SeverityLevel.Verbose, Level.Verbose.ToSeverityLevel());
            Assert.AreEqual(SeverityLevel.Information, Level.Information.ToSeverityLevel());
            Assert.AreEqual(SeverityLevel.Warning, Level.Warning.ToSeverityLevel());
            Assert.AreEqual(SeverityLevel.Error, Level.Error.ToSeverityLevel());
            Assert.AreEqual(SeverityLevel.Critical, Level.Critical.ToSeverityLevel());
            Assert.AreEqual(SeverityLevel.Warning, ((Level)5).ToSeverityLevel());
        }
    }
}
