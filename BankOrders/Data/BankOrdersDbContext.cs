﻿using BankOrders.Data.Models;
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

        public DbSet<OrderDetail> OrderDetails { get; init; }

        public DbSet<ExchangeRate> ExchangeRates { get; init; }

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

            /*builder
                .Entity<OrderDetail>()
                .HasOne(c => c.Order)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(c => c.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<OrderDetail>()
                .HasOne(c => c.Template)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(c => c.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);*/

            base.OnModelCreating(builder);
        }
    }
}
