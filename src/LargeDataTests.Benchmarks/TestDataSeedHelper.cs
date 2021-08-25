using AutoMapper;
using LargeDataTests.Domain.Entities;
using LargerDataTests.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeDataTests.Benchmarks
{
    static class TestDataSeedHelper
    {
        private static IMapper mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();

        public static async Task<AppDbContext> GetLoadedDatabaseWithJson(int count)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new AppDbContext(options);

            var testDataValuesDtos = GetTestDataValues(count);
            var testDataValuesItems = mapper.Map<ICollection<TestDataValuesItem>>(testDataValuesDtos);

            var dataWithJson = new TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            dbContext.DataWithJsonSerializations.Add(dataWithJson);

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        public static async Task<AppDbContext> GetLoadedDatabaseWithoutJson(int count)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new AppDbContext(options);

            var testDataValuesDtos = GetTestDataValues(count);
            var testDataValues = mapper.Map<ICollection<TestDataValues>>(testDataValuesDtos);

            var dataWithoutJson = new TestDataWithoutSerialization()
            {
                TestDataValues = testDataValues
            };

            dbContext.DataWithoutSerializations.Add(dataWithoutJson);

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        public static ICollection<TestValuesDto> GetTestDataValues(int count)
        {
            var testValues = new List<TestValuesDto>(count);

            var randomazier = new Random();

            for (int i = 0; i < count; i++)
            {
                testValues.Add(new TestValuesDto()
                {
                    Time = DateTime.Now.TimeOfDay.TotalSeconds,
                    Value1 = randomazier.NextDouble(),
                    Value2 = randomazier.NextDouble(),
                    Value3 = randomazier.NextDouble(),
                    Value4 = randomazier.NextDouble(),
                    Value5 = randomazier.NextDouble(),
                    Value6 = randomazier.NextDouble(),
                    Value7 = randomazier.NextDouble(),
                    Value8 = randomazier.NextDouble(),
                    Value9 = randomazier.NextDouble(),
                    Value10 = randomazier.NextDouble(),
                    Value11 = randomazier.NextDouble(),
                    Value12 = randomazier.NextDouble(),
                    Value13 = randomazier.NextDouble(),
                    Value14 = randomazier.NextDouble(),
                    Value15 = randomazier.NextDouble(),
                    Value16 = randomazier.NextDouble(),
                });
            }

            return testValues;
        }
    }
}
