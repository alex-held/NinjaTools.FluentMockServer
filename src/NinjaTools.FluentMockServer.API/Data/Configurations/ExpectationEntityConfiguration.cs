using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NinjaTools.FluentMockServer.Domain.Models;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class ExpectationEntityConfiguration : IEntityTypeConfiguration<Expectation>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Expectation> builder)
        {
            builder.ToTable("expectation");
            builder
                .HasKey(exp => exp.Id)
                .HasName($"Id");

            builder.Property(e => e.CreatedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(e => e.ModifiedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(e => e.Timestamp)
                .IsRequired()
                .HasColumnType("TimeStamp");

            builder.HasOne(e => e.HttpResponse);
            builder.HasOne(e => e.HttpRequest);
            builder.HasOne(e => e.HttpError);
            builder.HasOne(e => e.HttpForward);
            builder.HasOne(e => e.Times);
            builder.HasOne(e => e.TimeToLive);
            builder.Ignore(e => e.HttpForwardTemplate);
            builder.Ignore(e => e.HttpResponseTemplate);
        }
    }
}
