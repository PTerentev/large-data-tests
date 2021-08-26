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

        public static async Task<AppDbContext> ByteArray_SystemJson_GetLoadedDatabaseWithJson(int count)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new AppDbContext(options);

            var testDataValuesDtos = GetTestDataValues(count);
            var testDataValuesItems = mapper.Map<ICollection<TestDataValuesItem>>(testDataValuesDtos);

            var dataWithJson = new ByteArray_SystemJson_TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            dbContext.ByteArray_SystemJson_TestDataWithJsonSerializations.Add(dataWithJson);

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        public static async Task<AppDbContext> SystemJson_GetLoadedDatabaseWithJson(int count)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new AppDbContext(options);

            var testDataValuesDtos = GetTestDataValues(count);
            var testDataValuesItems = mapper.Map<ICollection<TestDataValuesItem>>(testDataValuesDtos);

            var dataWithJson = new SystemJson_TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            dbContext.SystemJson_TestDataWithJsonSerializations.Add(dataWithJson);

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        public static async Task<AppDbContext> SystemJsonCompressed_GetLoadedDatabaseWithJson(int count)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new AppDbContext(options);

            var testDataValuesDtos = GetTestDataValues(count);
            var testDataValuesItems = mapper.Map<ICollection<TestDataValuesItem>>(testDataValuesDtos);

            var dataWithJson = new SystemJsonCompressed_TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            dbContext.SystemJsonCompressed_TestDataWithJsonSerializations.Add(dataWithJson);

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

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
                    Values = Enumerable
                        .Range(0, 16)
                        .Select(i => randomazier.NextDouble())
                        .ToArray()
                });
            }

            return testValues;
        }
    }
}
