// Copyright (c) LibSharp. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net;

namespace LibSharp.Azure.ApplicationInsights
{
    /// <summary>
    /// An interface used to capture activity telemetry.
    /// </summary>
    public interface IActivity : IDisposable
    {
        /// <summary>
        /// Gets or sets the activity result.
        /// </summary>
        HttpStatusCode Result { get; set; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        IDictionary<string, string> Properties { get; }

        /// <summary>
        /// Gets the metrics.
        /// </summary>
        IDictionary<string, double> Metrics { get; }
    }
}
