using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class VerifyEntityConfiguration : IEntityTypeConfiguration<Verify>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Verify> builder)
        {
            builder.HasKey(request => request.Id);

            builder.ToTable("verify")
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

            builder.HasOne(e => e.HttpRequest);

            builder.Property(e => e.Times)
                .IsRequired(false)
                .HasConversion(
                    d => JObject.FromObject(d).ToString(Formatting.Indented),
                    str => JObject.Parse(str).ToObject<VerificationTimes>())
                .ValueGeneratedNever();
        }
    }
}
