using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.Models
{
    public interface IIdentifiable<T> : IEquatable<T>
    {
        [JsonIgnore, Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; }
        
        [JsonIgnore, Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        DateTime CreatedOn { get; set; }
        
        [JsonIgnore, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        DateTime ModifiedOn { get; set; }

        [JsonIgnore, Timestamp, CanBeNull]
        byte[] Timestamp { get; set; }


    }
}
