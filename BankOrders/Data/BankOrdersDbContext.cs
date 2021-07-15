using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankOrders.Data
{
    public class BankOrdersDbContext : IdentityDbContext
    {
        public BankOrdersDbContext(DbContextOptions<BankOrdersDbContext> options)
            : base(options)
        {
        }
    }
}
