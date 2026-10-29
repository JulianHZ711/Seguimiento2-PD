using Microsoft.EntityFrameworkCore;
using Access.Domain.Entities.AccessLogs;
using Access.Domain.Entities.AccessPoints;
using Access.Domain.Entities.Areas;
using Access.Domain.Entities.Credentials;
using Access.Domain.Entities.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<AccessPoint> AccessPoints { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
