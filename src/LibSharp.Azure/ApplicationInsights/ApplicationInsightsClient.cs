// Copyright (c) LibSharp. All rights reserved.

using System;
using System.Collections.Generic;
using LibSharp.Common;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace LibSharp.Azure.ApplicationInsights
{
    /// <summary>
    /// Application Insights telemetry client.
    /// </summary>
    public class ApplicationInsightsClient : ITelemetryClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsClient"/> class.
        /// </summary>
        /// <param name="client">Telemetry client.</param>
        public ApplicationInsightsClient(TelemetryClient client)
        {
            Argument.NotNull(client, nameof(client));

            m_client = client;
        }

        /// <inheritdoc/>
        public IActivity TrackActivity(string name)
        {
            Argument.NotNullOrWhiteSpace(name, nameof(name));

            return new ApplicationInsightsActivity(this, name);
        }

        /// <inheritdoc/>
        public void TrackEvent(string name, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Argument.NotNullOrWhiteSpace(name, nameof(name));

            m_client.TrackEvent(name, properties, metrics);
        }

        /// <inheritdoc/>
        public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            Argument.NotNull(exception, nameof(exception));

            m_client.TrackException(exception, properties, metrics);
        }

        /// <inheritdoc/>
        public void TrackTrace(string message, Level level, IDictionary<string, string> properties = null)
        {
            Argument.NotNullOrWhiteSpace(message, nameof(message));

            SeverityLevel severityLevel = level.ToSeverityLevel();
            m_client.TrackTrace(message, severityLevel, properties);
        }

        private readonly TelemetryClient m_client;
    }
}
