using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
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
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [CategoriesColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 1, targetCount: 2, id: "FastAndDirtyJob")]
    public class TestDataBenchmark
    {
        private const string SaveTestDataName = "Save Test Data";
        private const string GetTestDataName = "Get Test Data";

        private AppDbContext saveWithoutSerializationDbContext;
        private AppDbContext saveWithSerializationDbContext;

        private AppDbContext dbContextWithLoadedDataWithJson;
        private AppDbContext dbContextWithLoadedDataWithoutJson;

        private static IMapper mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();

        private ICollection<TestValuesDto> testDataValuesDtos;
        private ICollection<TestDataValues> testDataValues;
        private ICollection<TestDataValuesItem> testDataValuesItems;

        [Params(5000, 20000, 100000)]
        public int TestValuesCount { get; set; }

        [GlobalSetup(Targets = new[] { nameof(SaveTestDataWithJsonSerializationBenchmark), nameof(SaveTestDataWithoutSerializationBenchmark) })]
        public void SaveGlobalSetup()
        {
            testDataValuesDtos = TestDataSeedHelper.GetTestDataValues(TestValuesCount);
            testDataValues = mapper.Map<ICollection<TestDataValues>>(testDataValuesDtos);
            testDataValuesItems = mapper.Map<ICollection<TestDataValuesItem>>(testDataValuesDtos);
        }

        [GlobalSetup(Targets = new[] { nameof(GetTestDataWithJsonSerializationBenchmark) })]
        public async Task GetWithJsonGlobalSetup()
        {
            dbContextWithLoadedDataWithJson = await TestDataSeedHelper.GetLoadedDatabaseWithJson(TestValuesCount);
        }

        [GlobalSetup(Targets = new[] { nameof(GetTestDataWithoutSerializationBenchmark) })]
        public async Task GetWithoutJsonGlobalSetup()
        {
            dbContextWithLoadedDataWithoutJson = await TestDataSeedHelper.GetLoadedDatabaseWithoutJson(TestValuesCount);
        }

        [IterationSetup(Targets = new[] { nameof(SaveTestDataWithJsonSerializationBenchmark) })]
        public void SetupTestDataWithSerialization()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            saveWithSerializationDbContext = new AppDbContext(options);
        }

        [IterationCleanup(Targets = new[] { nameof(SaveTestDataWithJsonSerializationBenchmark) })]
        public void CleanupTestDataWithSerialization()
        {
            saveWithSerializationDbContext?.Dispose();
        }

        [IterationSetup(Targets = new[] { nameof(SaveTestDataWithoutSerializationBenchmark) })]
        public void SetupTestDataWithoutSerialization()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            saveWithoutSerializationDbContext = new AppDbContext(options);
        }

        [IterationCleanup(Targets = new[] { nameof(SaveTestDataWithoutSerializationBenchmark) })]
        public void CleanupTestDataWithoutSerialization()
        {
            saveWithoutSerializationDbContext?.Dispose();
        }

        [BenchmarkCategory(SaveTestDataName), Benchmark(Baseline = true)]
        public async Task SaveTestDataWithJsonSerializationBenchmark()
        {
            var data = new TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            saveWithSerializationDbContext.DataWithJsonSerializations.Add(data);
            await saveWithSerializationDbContext.SaveChangesAsync();
        }

        [BenchmarkCategory(SaveTestDataName), Benchmark]
        public async Task SaveTestDataWithoutSerializationBenchmark()
        {
            var data = new TestDataWithoutSerialization()
            {
                TestDataValues = testDataValues
            };

            saveWithoutSerializationDbContext.DataWithoutSerializations.Add(data);
            await saveWithoutSerializationDbContext.SaveChangesAsync();
        }

        [BenchmarkCategory(GetTestDataName), Benchmark(Baseline = true)]
        public async Task<List<TestDataDto>> GetTestDataWithJsonSerializationBenchmark()
        {
            var dataWithJson = await dbContextWithLoadedDataWithJson.DataWithJsonSerializations.ToListAsync();

            var data = mapper.Map<List<TestDataDto>>(dataWithJson);
            return data;
        }

        [BenchmarkCategory(GetTestDataName), Benchmark]
        public async Task<List<TestDataDto>> GetTestDataWithoutSerializationBenchmark()
        {
            var dataWithoutJson = await dbContextWithLoadedDataWithoutJson.DataWithoutSerializations.ToListAsync();

            var data = mapper.Map<List<TestDataDto>>(dataWithoutJson);
            return data;
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            testDataValuesDtos = null;
            testDataValues = null;

            dbContextWithLoadedDataWithJson?.Dispose();
            dbContextWithLoadedDataWithoutJson?.Dispose();
            saveWithSerializationDbContext?.Dispose();
            saveWithoutSerializationDbContext?.Dispose();
        }
    }
}
