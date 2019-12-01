using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.API.Data.Configurations
{
    public class ExpectationEntityConfiguration : IEntityTypeConfiguration<Expectation>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Expectation> builder)
        {
            builder.ToTable("Expectations").HasKey(e => e.Id);
            builder.HasIndex("Id",
                "CreatedOn",
                "Timestamp");

            builder.Property(r => r.Timestamp)
                .IsRequired()
                .IsRowVersion();

            builder.Property(e => e.CreatedOn)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()")
                .HasColumnType("datetime")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ModifiedOn)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()")
                .HasColumnType("datetime")
                .ValueGeneratedOnAdd();

            // -- HttpRequest -- //
            builder.OwnsOne(e => e.HttpRequest, req =>
            {
                req.HasIndex(r => r.Method).IsUnique(false);
                req.Property(r => r.Method).IsRequired(false).HasMaxLength(20);
                req.HasIndex(r => r.Path).IsUnique(false);
                req.Property(r => r.Path).IsRequired(false).HasMaxLength(100);
                req.Property(r => r.Secure).IsRequired(false);
                req.Property(r => r.KeepAlive).IsRequired(false);
                req.HasIndex(r => r.Body).IsUnique(false);
                req.Property(r => r.Body).HasConversion(new JTokenValueConverter()).IsRequired(false);
                req.Property(r => r.Headers).HasConversion(new NestedDictionaryConverter()).IsRequired(false);
                req.Property(r => r.Cookies).HasConversion(new DictionaryConverter()).IsRequired(false);
            });

            // -- HttpResponse -- //
            builder.OwnsOne(e => e.HttpResponse, res =>
            {
                res.Property(r => r.StatusCode).IsRequired(false);
                res.OwnsOne(r => r.ConnectionOptions, opt =>
                {
                    opt.Property(o => o.CloseSocket).IsRequired(false);
                    opt.Property(o => o.KeepAliveOverride).IsRequired(false);
                    opt.Property(o => o.SuppressConnectionHeader).IsRequired(false);
                    opt.Property(o => o.SuppressContentLengthHeader).IsRequired(false);
                    opt.Property(o => o.ContentLengthHeaderOverride).IsRequired(false);
                });
                res.Property(r => r.Headers).IsRequired(false).HasConversion(new NestedDictionaryConverter());
                res.Property(r => r.Body).HasConversion(new JTokenValueConverter()).IsRequired(false);
                res.Property(r => r.Delay).IsRequired(false).HasConversion(new DelayValueConverter());
            });

            // -- HttpError -- //
            builder.OwnsOne(e => e.HttpError, err =>
            {
                err.Property(r => r.Delay).HasConversion(new DelayValueConverter()).IsRequired(false);
                err.Property(r => r.DropConnection).IsRequired(false);
                err.Property(r => r.ResponseBytes).IsRequired(false).HasColumnName("Base64Response");
            });

            // -- HttpForward -- //
            builder.OwnsOne(e => e.HttpForward, fwd =>
            {
                fwd.Property(r => r.Host).IsRequired(false).HasMaxLength(100);
                fwd.Property(r => r.Port).IsRequired(false);
            });

            // -- HttpForward -- //
            builder.OwnsOne(e => e.Times, time =>
            {
                time.Property(r => r.Unlimited).IsRequired();
                time.Property(r => r.RemainingTimes).IsRequired();
            });


            // -- LiveTime -- //
            builder.OwnsOne(e => e.TimeToLive, ttl =>
            {
                ttl.Property(r => r.Unlimited).IsRequired(false);
                ttl.Property(r => r.TimeUnit).IsRequired(false).HasConversion(new TimeUnitConverter());
                ttl.Property(r => r.TimeToLive).IsRequired(false);
            });

            // -- .HttpForwardTemplate -- //
            builder.OwnsOne(e => e.HttpForwardTemplate, fwd =>
            {
                fwd.Property(f => f.Delay).IsRequired(false).HasConversion(new DelayValueConverter());
                fwd.Property(f => f.Template).IsRequired(false);
            });

            // -- HttpResponseTemplate -- //
            builder.OwnsOne(e => e.HttpResponseTemplate, fwd =>
            {
                fwd.Property(f => f.Delay).IsRequired(false).HasConversion(new DelayValueConverter());
                fwd.Property(f => f.Template).IsRequired(false);
            });
        }
    }


    public class NestedDictionaryConverter : ValueConverter<Dictionary<string, string[]>, string>
    {
        public NestedDictionaryConverter() 
        : base(dict => JObject.FromObject(dict).ToString(),
            str => JObject.Parse(str).ToObject<Dictionary<string, string[]>>()) { }
    }
    
    public class DictionaryConverter : ValueConverter<Dictionary<string, string>, string>
    {
        public DictionaryConverter() 
            : base(dict => JObject.FromObject(dict).ToString(),
                str => JObject.Parse(str).ToObject<Dictionary<string, string>>()) { }
    }

    public class TimeUnitConverter : ValueConverter<TimeUnit?, int?>
    {
        public TimeUnitConverter() : base(
            // ReSharper disable once HeapView.BoxingAllocation
            timeUnit => (int?) timeUnit,
            // ReSharper disable once HeapView.BoxingAllocation
            @int => @int.HasValue ? (TimeUnit?) @int.Value : null)
        {
        }
    }


    public class DelayValueConverter : ValueConverter<Delay, string>
    {
        /// <inheritdoc />
        public DelayValueConverter() : base(
            d => JToken.FromObject(d).ToString(),
            s => JObject.Parse(s).ToObject<Delay>())
        {
        }
    }


    public class JTokenValueConverter : ValueConverter<JToken, string>
    {
        /// <inheritdoc />
        public JTokenValueConverter()
            : base(jt => jt.ToString(),
                s => JToken.Parse(s))
        {
        }
    }
}
