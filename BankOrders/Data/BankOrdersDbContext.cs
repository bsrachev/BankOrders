using BankOrders.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankOrders.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BankOrders.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class BankOrdersDbContext : IdentityDbContext<User>
    {
        public BankOrdersDbContext(DbContextOptions<BankOrdersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; init; }

        public DbSet<Template> Templates { get; init; }

        public DbSet<Detail> Details { get; init; }

        public DbSet<Currency> Currencies { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("OrderRefNumSeq")
                              .StartsAt(10000001).IncrementsBy(1);

            builder.Entity<Order>()
               .Property(x => x.RefNumber)
               .HasDefaultValueSql("NEXT VALUE FOR OrderRefNumSeq");

            builder.HasSequence<int>("TemplateOrderRefNumSeq")
                              .StartsAt(90000001).IncrementsBy(1);

            builder.Entity<Template>()
               .Property(x => x.RefNumber)
               .HasDefaultValueSql("NEXT VALUE FOR TemplateOrderRefNumSeq");

            builder
                .Entity<Detail>()
                .HasOne(c => c.Currency)
                .WithMany(c => c.Details)
                .HasForeignKey(c => c.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
