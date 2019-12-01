using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class TimesEntityConfiguration : IEntityTypeConfiguration<Times>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Times> builder)
        {
            builder.HasKey(request => request.Id);

            builder.ToTable("times")
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

            builder.Property(e => e.RemainingTimes)
                .IsRequired();

            builder.Property(e => e.Unlimited)
                .IsRequired();
        }
    }
}
