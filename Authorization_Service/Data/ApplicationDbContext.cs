using Authorization_Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization_Service.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string ConnectionString = "data source=DESKTOP-A3F76NG; initial catalog=AuditManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(ConnectionString);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<UserModel> Users { get; set; }
    }
}
