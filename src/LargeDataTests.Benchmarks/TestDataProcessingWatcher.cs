using LargeDataTests.Domain.Entities;
using LargerDataTests.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeDataTests.Benchmarks
{
    class TestDataProcessingWatcher
    {
        public static void Run(int valuesCount)
        {
            Console.WriteLine($"Values count: {valuesCount}");

            var stopWatcher = new Stopwatch();

            var testDataValuesDtos = TestDataSeedHelper.GetTestDataValues(valuesCount);
            var testDataValuesItems = MappingProfile.Mapper.Map<ICollection<TestDataValuesItem>>(testDataValuesDtos);

            stopWatcher.Start();

            var result = System.Text.Json.JsonSerializer.Serialize(testDataValuesItems);

            stopWatcher.Stop();

            Console.WriteLine($"Serialize Time: {stopWatcher.ElapsedMilliseconds}");

            stopWatcher.Reset();

            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new AppDbContext(options);

            var dataWithJson = new CustomTestData()
            {
                TestDataValues = result
            };

            dbContext.CustomTestDatas.Add(dataWithJson);

            stopWatcher.Start();

            dbContext.SaveChanges();

            stopWatcher.Stop();

            Console.WriteLine($"Saving to DB Time: {stopWatcher.ElapsedMilliseconds}");

            stopWatcher.Reset();

            Console.WriteLine();

            //dataWithJson = dbContext.CustomTestDatas.AsNoTracking().FirstOrDefault();

            //dataWithJson.TestDataValues
        }
    }
}
