// Copyright (c) LibSharp. All rights reserved.

using System;
using System.Collections.Generic;

namespace LibSharp.Azure.ApplicationInsights
{
    /// <summary>
    /// Telemetry client interface.
    /// </summary>
    public interface ITelemetryClient
    {
        /// <summary>
        /// Logs an activity result along with duration.
        /// </summary>
        /// <param name="name">Activity name.</param>
        /// <returns>Activity object.</returns>
        IActivity TrackActivity(string name);

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="name">Event name.</param>
        /// <param name="properties">Event properties.</param>
        /// <param name="metrics">Event metrics.</param>
        void TrackEvent(string name, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        /// <param name="properties">Exception properties.</param>
        /// <param name="metrics">Exception metrics.</param>
        void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        /// <param name="message">Trace message.</param>
        /// <param name="level">Trace level.</param>
        /// <param name="properties">Trace properties.</param>
        void TrackTrace(string message, Level level, IDictionary<string, string> properties = null);
    }
}
