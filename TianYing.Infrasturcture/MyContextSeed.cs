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
