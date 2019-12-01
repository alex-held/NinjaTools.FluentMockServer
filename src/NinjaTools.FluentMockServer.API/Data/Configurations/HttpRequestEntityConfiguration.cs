using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class HttpRequestEntityConfiguration : IEntityTypeConfiguration<HttpRequest>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<HttpRequest> builder)
        {
            builder.HasKey(request => request.Id);

            builder.ToTable("httpRequest").HasIndex(nameof(HttpRequest.Path),
                    nameof(HttpRequest.Id),
                    nameof(HttpRequest.Method),
                    nameof(HttpRequest.CreatedOn))
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

            builder.Property(e => e.Path)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(e => e.Path);

            builder.Property(e => e.Method)
                .IsRequired(false)
                .HasMaxLength(20);

      

            builder.Property(e => e.Secure)
                .IsRequired(false)
                .ValueGeneratedNever();

            builder.Property(e => e.KeepAlive)
                .IsRequired(false)
                .ValueGeneratedNever();


            builder.Ignore(e => e.Headers);
            builder.Ignore(e => e.Cookies);

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
