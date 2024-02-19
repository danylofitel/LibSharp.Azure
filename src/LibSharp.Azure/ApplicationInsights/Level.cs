// Copyright (c) LibSharp. All rights reserved.

namespace LibSharp.Azure.ApplicationInsights
{
    /// <summary>
    /// Logging levels.
    /// </summary>
    public enum Level
    {
        /// <summary>
        /// Verbose level.
        /// Disabled in Production environment by default.
        /// </summary>
        Verbose,

        /// <summary>
        /// Information level.
        /// </summary>
        Information,

        /// <summary>
        /// Warning level.
        /// </summary>
        Warning,

        /// <summary>
        /// Error level.
        /// </summary>
        Error,

        /// <summary>
        /// Critical error level.
        /// </summary>
        Critical,
    }
}
