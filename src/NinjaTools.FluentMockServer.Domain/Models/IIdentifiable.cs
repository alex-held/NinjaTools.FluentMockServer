using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.Domain.Models
{
    public interface IIdentifiable<T, TEntity> :  IIdentifiable<T>  where TEntity : class, T where T : class,  IEquatable<TEntity>
    {
    }

    public interface IIdentifiable<T> : IEquatable<T>
    {
        [JsonIgnore, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; }
        
        [JsonIgnore, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        DateTime CreatedOn { get; set; }
        
        [JsonIgnore, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        DateTime ModifiedOn { get; set; }

        [JsonIgnore, Timestamp, CanBeNull]
        byte[] Timestamp { get; set; }


    }
}
