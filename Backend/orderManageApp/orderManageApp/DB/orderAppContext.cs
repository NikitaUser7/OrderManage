using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orderManageApp.DB
{
    public class orderAppContext : DbContext
    {
        public static readonly string RowVersion = nameof(RowVersion);

        public orderAppContext(DbContextOptions<orderAppContext> options) : base(options)
        {
        }
        public DbSet<order> Orders { get; set; }
        public DbSet<order_position> Order_positions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<order>()
                .HasKey(c => c.ID);
        }

    }
}
