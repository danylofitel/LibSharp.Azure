// Copyright (c) LibSharp. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using LibSharp.Common;

namespace LibSharp.Azure.ApplicationInsights
{
    /// <summary>
    /// Activity that can be logged by telemetry.
    /// </summary>
    public class ApplicationInsightsActivity : IActivity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsActivity"/> class.
        /// </summary>
        /// <param name="telemetryClient">Telemetry client.</param>
        /// <param name="name">Activity name.</param>
        public ApplicationInsightsActivity(ITelemetryClient telemetryClient, string name)
        {
            Argument.NotNull(telemetryClient, nameof(telemetryClient));
            Argument.NotNullOrWhiteSpace(name, nameof(name));

            m_telemetryClient = telemetryClient;
            m_name = name;
            m_id = Guid.NewGuid();
            m_watch = Stopwatch.StartNew();
            m_startTime = DateTime.UtcNow;
            m_disposed = false;

            Result = HttpStatusCode.InternalServerError;
            Properties = new Dictionary<string, string>();
            Metrics = new Dictionary<string, double>();

            m_telemetryClient.TrackEvent(
                $"{m_name}_Started",
                new Dictionary<string, string>
                {
                    ["Id"] = m_id.ToString(),
                    ["StartTime"] = m_startTime.ToString(DateTimeFormat, s_dateTimeFormatProvider),
                });
        }

        /// <inheritdoc/>
        public HttpStatusCode Result { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> Properties { get; }

        /// <inheritdoc/>
        public IDictionary<string, double> Metrics { get; }

        private void Send()
        {
            m_watch.Stop();

            Properties["Id"] = m_id.ToString();
            Properties["StartTime"] = m_startTime.ToString(DateTimeFormat, s_dateTimeFormatProvider);
            Properties["EndTime"] = m_startTime.Add(m_watch.Elapsed).ToString(DateTimeFormat, s_dateTimeFormatProvider);
            Properties["Result"] = Result.ToString();
            Properties["HttpStatusCode"] = ((int)Result).ToString(CultureInfo.InvariantCulture);

            Metrics[$"{m_name}_DurationMilliseconds"] = m_watch.Elapsed.TotalMilliseconds;

            m_telemetryClient.TrackEvent(m_name, Properties, Metrics);
        }

        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffff";
        private static readonly IFormatProvider s_dateTimeFormatProvider = new DateTimeFormatInfo();

        private readonly ITelemetryClient m_telemetryClient;
        private readonly string m_name;
        private readonly Guid m_id;
        private readonly Stopwatch m_watch;
        private readonly DateTime m_startTime;
        private bool m_disposed;

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the activity.
        /// </summary>
        /// <param name="disposing">True if called by Dispose(), false if called by the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                Send();
                m_disposed = true;
            }
        }
    }
}
