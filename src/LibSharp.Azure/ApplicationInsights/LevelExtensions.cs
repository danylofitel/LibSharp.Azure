// Copyright (c) LibSharp. All rights reserved.

using Microsoft.ApplicationInsights.DataContracts;

namespace LibSharp.Azure.ApplicationInsights
{
    /// <summary>
    /// Level extension methods.
    /// </summary>
    public static class LevelExtensions
    {
        /// <summary>
        /// Converts level to SeverityLevel from Application Insights.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <returns>Severity level.</returns>
        public static SeverityLevel ToSeverityLevel(this Level level)
        {
            return level switch
            {
                Level.Verbose => SeverityLevel.Verbose,
                Level.Information => SeverityLevel.Information,
                Level.Warning => SeverityLevel.Warning,
                Level.Error => SeverityLevel.Error,
                Level.Critical => SeverityLevel.Critical,
                _ => SeverityLevel.Warning,
            };
        }
    }
}
