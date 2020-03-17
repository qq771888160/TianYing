using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TianYing.Core.Entities;
using TianYing.Infrasturcture.Databases.EntityConfigurations;

namespace TianYing.Infrasturcture.Databases
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);  // 可以这样进行配置
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            //modelBuilder.Entity<Company>().HasData(
            //    new Company
            //    {
            //        Id = Guid.Parse("22ac7044-7600-478c-98b7-b6198dd7354b"),
            //        Name = "Microsoft",
            //        Introduction = "Create Compay"
            //    },
            //     new Company
            //     {
            //         Id = Guid.Parse("b8f047a6-8976-441a-aa9d-17f00a073816"),
            //        Name = "Microsoft",
            //       Introduction = "Create Compay"
            //     },
            //     new Company
            //     {
            //       Id = Guid.Parse("4110f7bc-7b35-4011-918b-3402b4e69bab"),
            //       Name = "Microsoft",
            //       Introduction = "Create Compay"
            //    }
                //);

        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
