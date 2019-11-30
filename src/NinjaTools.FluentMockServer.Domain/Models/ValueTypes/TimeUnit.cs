using System;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Domain.Serialization;

namespace NinjaTools.FluentMockServer.Domain.Models.ValueTypes
{
    /// <summary>
    ///     Enumerate the available time unit.
    /// </summary>
    [JsonConverter(typeof(ContractResolver.UpperCaseEnumConverter))]
    [Serializable]
    public enum TimeUnit
    {
        /// <summary>
        ///     The nanoseconds.
        /// </summary>
        Nanoseconds,

        /// <summary>
        ///     The Microseconds.
        /// </summary>
        Microseconds,

        /// <summary>
        ///     The Milliseconds.
        /// </summary>
        Milliseconds,

        /// <summary>
        ///     The seconds.
        /// </summary>
        Seconds,

        /// <summary>
        ///     The minutes.
        /// </summary>
        Minutes,

        /// <summary>
        ///     The hours.
        /// </summary>
        Hours,

        /// <summary>
        ///     The days.
        /// </summary>
        Days
    }
}
