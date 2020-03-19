using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYing.Core.Entities;
using TianYing.Infrasturcture.Databases;

namespace TianYing.Infrasturcture
{
    public class MyContextSeed
    {
        public static async Task SeedAsync(MyContext myContext,
                         ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                // TODO: Only run this if using a real database
                // myContext.Database.Migrate();

                if (!myContext.Companies.Any())
                {
                    myContext.Companies.AddRange(
                        new List<Company>{
                             new Company
                {
                    Id = Guid.Parse("22ac7044-7600-478c-98b7-b6198dd7354b"),
                    Name = "Microsoft",
                    Introduction = "Create Compay"
                },
                     new Company
                     {
                         Id = Guid.Parse("b8f047a6-8976-441a-aa9d-17f00a073816"),
                         Name = "Microsoft",
                         Introduction = "Create Compay"
                     },
                          new Company
                          {
                              Id = Guid.Parse("4110f7bc-7b35-4011-918b-3402b4e69bab"),
                              Name = "Microsoft",
                              Introduction = "Create Compay"
                          }
                    //new Company
                    //{
                    //    Id = Guid.Parse("4110f7bc-7b35-4011-918b-3402b4e69bab"),
                    //    Name = "Microsoft",
                    //    Introduction = "Create Compay",
                    //    Employees = new List<Employee>
                    //    {
                    //        new Employee
                    //        {
                    //            DateofBirth = new DateTime(1999,12,12),
                    //            EmployeeNo="f1",
                    //            FirstName="aa",
                    //            LastName="bb",
                    //            Gender=Gender.女
                    //        },
                    //        new Employee
                    //        {
                    //            DateofBirth = new DateTime(1999,12,12),
                    //            EmployeeNo="f1",
                    //            FirstName="aa",
                    //            LastName="bb",
                    //            Gender=Gender.女
                    //        }
                    //    }
                    //}   //  不允许这样
                        }
                    );
                    await myContext.SaveChangesAsync();                            
                }

                if (!myContext.Employees.Any())
                {
                    myContext.Employees.AddRange(
                        new List<Employee>
                        {
                                new Employee
                                {
                                    Id=Guid.Parse("a565c6ec-98b5-4411-9a1b-2a4762437a83"),
                                    CompanyId=Guid.Parse("22ac7044-7600-478c-98b7-b6198dd7354b"),
                                    DateofBirth=new DateTime(1999,12,12),
                                    EmployeeNo="G001",
                                    FirstName="a",
                                    LastName="b",
                                    Gender=Gender.女
                                },
                                   new Employee
                                {
                                       Id=Guid.Parse("17245fa4-601e-4f0c-9130-4d70925d2d4e"),
                                    CompanyId=Guid.Parse("22ac7044-7600-478c-98b7-b6198dd7354b"),
                                    DateofBirth=new DateTime(1999,12,12),
                                    EmployeeNo="G002",
                                    FirstName="a",
                                    LastName="c",
                                    Gender=Gender.女
                                },
                                      new Employee
                                {
                                         Id=Guid.Parse("97979813-d8d0-48a3-8ee8-dca1c1ebafd6"),
                                    CompanyId=Guid.Parse("b8f047a6-8976-441a-aa9d-17f00a073816"),
                                    DateofBirth=new DateTime(1999,12,12),
                                    EmployeeNo="G003",
                                    FirstName="a",
                                    LastName="d",
                                    Gender=Gender.女
                                },
                                         new Employee
                                {
                                             Id=Guid.Parse("8b4484e3-a888-4615-8d6a-dccc39ef526c"),
                                    CompanyId=Guid.Parse("b8f047a6-8976-441a-aa9d-17f00a073816"),
                                    DateofBirth=new DateTime(1999,12,12),
                                    EmployeeNo="G004",
                                    FirstName="a",
                                    LastName="e",
                                    Gender=Gender.女
                                },
                                   new Employee
                                {
                                       Id=Guid.Parse("12af62f3-cac8-4492-8235-8bb89985a702"),
                                    CompanyId=Guid.Parse("b8f047a6-8976-441a-aa9d-17f00a073816"),
                                    DateofBirth=new DateTime(1999,12,12),
                                    EmployeeNo="G005",
                                    FirstName="a",
                                    LastName="f",
                                    Gender=Gender.女
                                },
                                      new Employee
                                {
                                        Id=Guid.Parse("8acc171b-6742-4b2c-a0fa-ba2745026d96"),
                                    CompanyId=Guid.Parse("b8f047a6-8976-441a-aa9d-17f00a073816"),
                                    DateofBirth=new DateTime(1999,12,12),
                                    EmployeeNo="G006",
                                    FirstName="a",
                                    LastName="d",
                                    Gender=Gender.女
                                },
                        }
                        );
                    await myContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<MyContextSeed>();
                    logger.LogError(ex.Message);
                    await SeedAsync(myContext, loggerFactory, retryForAvailability);
                }
            }
        }
    }
}
