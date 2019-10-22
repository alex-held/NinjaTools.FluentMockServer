using System;

using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Models.HttpEntities;
using HardCoded.MockServer.Contracts.Models.ValueTypes;

using Newtonsoft.Json;


namespace HardCoded.MockServer.Contracts.Models
{
    /// <summary>
    /// Model used to describe what to verify.
    /// </summary>
    public class Verify : BuildableBase, IEquatable<Verify>
    {
        /// <summary>
        /// The to be matched <see cref="HttpRequest"/>.
        /// </summary>
        public HttpRequest HttpRequest { get; set; }
       
        /// <summary>
        /// How many <see cref="Times"/> the request is expected to have occured.
        /// </summary>
        public Times Times { get; set; }
    }
}