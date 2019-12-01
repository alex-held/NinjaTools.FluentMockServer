using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class HttpResponseEntityConfiguration : IEntityTypeConfiguration<HttpResponse>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<HttpResponse> builder)
        {
            builder.HasKey(request => request.Id);

            builder.ToTable("httpResponse")
                .HasIndex(
                    nameof(HttpResponse.Id),
                    nameof(HttpResponse.Timestamp),
                    nameof(HttpResponse.CreatedOn),
                    nameof(HttpResponse.ModifiedOn),
                    nameof(HttpResponse.StatusCode))
                .IsUnique(false);
            
            builder.Property(e => e.CreatedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(e => e.ModifiedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(e => e.Timestamp)
                .IsRequired()
                .HasColumnType("TimeStamp");

            builder.Property(e => e.Delay)
                .IsRequired(false)
                .HasConversion(
                    d => JObject.FromObject(d).ToString(Formatting.Indented),
                    str => JObject.Parse(str).ToObject<Delay>());
            
            builder.Ignore(e => e.Headers);
            builder.Ignore(e => e.ConnectionOptions);

            builder.Property(exp => exp.Body)
                .IsRequired(false)
                .HasConversion(
                    jo => jo.ToString(Formatting.Indented),
                    str => JObject.Parse(str))
                .ValueGeneratedNever()
                .HasColumnType("NVARCHAR");
            
            
        }
    }
}
