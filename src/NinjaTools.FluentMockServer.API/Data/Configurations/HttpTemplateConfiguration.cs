using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class HttpTemplateConfiguration : IEntityTypeConfiguration<HttpTemplate>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<HttpTemplate> builder)
        {
            builder.HasKey(request => request.Id);

            builder.ToTable("httpTemplate")
                .HasIndex(nameof(HttpResponse.Id),
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

            builder.Property(e => e.Template);
            
            builder.Property(e => e.Delay)
                .HasConversion(
                    d => JObject.FromObject(d).ToString(Formatting.Indented),
                    str => JObject.Parse(str).ToObject<Delay>());
                
        }
    }
    
    
    public class LifeTimeConfiguration : IEntityTypeConfiguration<LifeTime>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<LifeTime> builder)
        {
            builder.HasKey(request => request.Id);

            builder.ToTable("lifeTime")
                .HasIndex(nameof(HttpResponse.Id),
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

            builder.Property(e => e.Unlimited)
                .IsRequired(false)
                .ValueGeneratedNever();

            builder.Property(e => e.TimeUnit)
                .IsRequired(false)
                .HasConversion(typeof(ushort?));

            builder.Property(e => e.TimeToLive)
                .IsRequired(false);
        }
    }
}
