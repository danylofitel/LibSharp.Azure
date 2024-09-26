// Copyright (c) LibSharp. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using LibSharp.Azure.ApplicationInsights;
using LibSharp.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace LibSharp.Azure.UnitTests.ApplicationInsights
{
    [TestClass]
    public class ApplicationInsightsActivityUnitTests
    {
        [TestMethod]
        public void DefaultProperties_Dispose_TracksActivityAsEvent()
        {
            // Arrange
            string name = "Do_Something";
            IDictionary<string, string> properties = null;
            IDictionary<string, double> metrics = null;

            ITelemetryClient telemetryClient = Substitute.For<ITelemetryClient>();

            telemetryClient
                .When(x => x.TrackEvent(name, Arg.Is<IDictionary<string, string>>(_ => _ != null), Arg.Is<IDictionary<string, double>>(_ => _ != null)))
                .Do(args =>
                {
                    properties = (IDictionary<string, string>)args[1];
                    metrics = (IDictionary<string, double>)args[2];
                });

            // Act
            using (ApplicationInsightsActivity activity = new ApplicationInsightsActivity(telemetryClient, name))
            {
                telemetryClient.Received(1).TrackEvent($"{name}_Started", Arg.Is<IDictionary<string, string>>(_ => _ != null), null);
                Assert.AreEqual(1, telemetryClient.ReceivedCalls().Count());
            }

            // Assert
            telemetryClient.Received(1).TrackEvent(name, Arg.Is<IDictionary<string, string>>(_ => _ != null), Arg.Is<IDictionary<string, double>>(_ => _ != null));
            Assert.AreEqual(2, telemetryClient.ReceivedCalls().Count());

            Assert.IsTrue(properties.ContainsKey("Id"));
            Assert.IsTrue(properties.ContainsKey("StartTime"));
            Assert.IsTrue(properties.ContainsKey("EndTime"));
            Assert.AreEqual("InternalServerError", properties["Result"]);
            Assert.AreEqual("500", properties["HttpStatusCode"]);

            Assert.IsTrue(metrics.ContainsKey("Do_Something_DurationMilliseconds"));
        }

        [TestMethod]
        public void CustomProperties_Dispose_TracksActivityAsEvent()
        {
            // Arrange
            string name = "Do_Something";
            HttpStatusCode customResult = HttpStatusCode.OK;
            IDictionary<string, string> customProperties = new Dictionary<string, string>
            {
                ["property1"] = "value1",
                ["property2"] = "value2",
            };
            IDictionary<string, double> customMetrics = new Dictionary<string, double>
            {
                ["property1"] = 1.0,
                ["property2"] = 2.0,
            };

            IDictionary<string, string> properties = null;
            IDictionary<string, double> metrics = null;

            ITelemetryClient telemetryClient = Substitute.For<ITelemetryClient>();

            telemetryClient
                .When(x => x.TrackEvent(name, Arg.Is<IDictionary<string, string>>(_ => _ != null), Arg.Is<IDictionary<string, double>>(_ => _ != null)))
                .Do(args =>
                {
                    properties = (IDictionary<string, string>)args[1];
                    metrics = (IDictionary<string, double>)args[2];
                });

            // Act
            using (ApplicationInsightsActivity activity = new ApplicationInsightsActivity(telemetryClient, name))
            {
                telemetryClient.Received(1).TrackEvent($"{name}_Started", Arg.Is<IDictionary<string, string>>(_ => _ != null), null);
                Assert.AreEqual(1, telemetryClient.ReceivedCalls().Count());

                activity.Result = customResult;
                _ = customProperties.CopyTo(activity.Properties);
                _ = customMetrics.CopyTo(activity.Metrics);
            }

            // Assert
            telemetryClient.Received(1).TrackEvent(name, Arg.Is<IDictionary<string, string>>(_ => _ != null), Arg.Is<IDictionary<string, double>>(_ => _ != null));
            Assert.AreEqual(2, telemetryClient.ReceivedCalls().Count());

            Assert.IsTrue(properties.ContainsKey("Id"));
            Assert.IsTrue(properties.ContainsKey("StartTime"));
            Assert.IsTrue(properties.ContainsKey("EndTime"));
            Assert.AreEqual("OK", properties["Result"]);
            Assert.AreEqual("200", properties["HttpStatusCode"]);

            foreach (KeyValuePair<string, string> pair in customProperties)
            {
                Assert.AreEqual(pair.Value, properties[pair.Key]);
            }

            Assert.IsTrue(metrics.ContainsKey("Do_Something_DurationMilliseconds"));

            foreach (KeyValuePair<string, double> pair in customMetrics)
            {
                Assert.AreEqual(pair.Value, metrics[pair.Key]);
            }
        }
    }
}
