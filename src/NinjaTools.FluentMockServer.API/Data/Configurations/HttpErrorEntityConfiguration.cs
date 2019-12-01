using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class HttpErrorEntityConfiguration : IEntityTypeConfiguration<HttpError>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<HttpError> builder)
        {
            builder.HasKey(request => request.Id);

            builder.ToTable("httpError")
                .HasIndex(
                    nameof(HttpResponse.Id),
                    nameof(HttpResponse.Timestamp),
                    nameof(HttpResponse.CreatedOn),
                    nameof(HttpResponse.ModifiedOn)
                    ).IsUnique(false);
            
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

            builder.Property(e => e.DropConnection)
                .IsRequired(false)
                .ValueGeneratedNever();
            
            
            builder.Property(e => e.ResponseBytes)
                .IsRequired(false)
                .ValueGeneratedNever();
        }
    }
}
