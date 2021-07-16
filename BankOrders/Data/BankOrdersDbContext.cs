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

    public class BankOrdersDbContext : IdentityDbContext
    {
        public BankOrdersDbContext(DbContextOptions<BankOrdersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; init; }

        public DbSet<Template> Templates { get; init; }

        public DbSet<OrderDetail> OrderDetails { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
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
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
