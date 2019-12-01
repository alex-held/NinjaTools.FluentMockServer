using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class HttpForwardConfiguration : IEntityTypeConfiguration<HttpForward>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<HttpForward> builder)
        {
            builder.HasKey(request => request.Id);

            builder.ToTable("httpForward")
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

            builder.Property(e => e.Host)
                .IsRequired(false)
                .HasMaxLength(100);
            
                
            builder.Property(e => e.Port).IsRequired(false);
        }
    }
}
