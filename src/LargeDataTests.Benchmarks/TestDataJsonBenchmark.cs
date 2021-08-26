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
    [MemoryDiagnoser(false)]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [CategoriesColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [MeanColumn]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 1, targetCount: 1, id: "FastAndDirtyJob")]
    public class TestDataJsonBenchmark
    {
        private const string SaveTestDataName = "Save Test Data";
        private const string GetTestDataName = "Get Test Data";

        private AppDbContext saveWithSerializationDbContext;

        private AppDbContext dbContextWithLoadedDataWithJson;

        private static IMapper mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();

        private ICollection<TestValuesDto> testDataValuesDtos;
        private ICollection<TestDataValuesItem> testDataValuesItems;

        [Params(10_000, 100_000)]
        public int TestValuesCount { get; set; }

        [GlobalSetup]
        public void SaveGlobalSetup()
        {
            testDataValuesDtos = TestDataSeedHelper.GetTestDataValues(TestValuesCount);
            testDataValuesItems = mapper.Map<ICollection<TestDataValuesItem>>(testDataValuesDtos);
        }

        #region Newtonsoft

        [GlobalSetup(Targets = new[] { nameof(Newtonsoft_GetTestDataWithJsonSerializationBenchmark) })]
        public async Task Newtonsoft_GetWithJsonGlobalSetup()
        {
            dbContextWithLoadedDataWithJson = await TestDataSeedHelper.GetLoadedDatabaseWithJson(TestValuesCount);
        }

        [IterationSetup(Targets = new[] { nameof(Newtonsoft_SaveTestDataWithJsonSerializationBenchmark) })]
        public void Newtonsoft_SetupTestDataWithSerialization()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            saveWithSerializationDbContext = new AppDbContext(options);
        }

        [IterationCleanup(Targets = new[] { nameof(Newtonsoft_SaveTestDataWithJsonSerializationBenchmark) })]
        public void Newtonsoft_CleanupTestDataWithSerialization()
        {
            saveWithSerializationDbContext?.Dispose();
        }

        [BenchmarkCategory(SaveTestDataName), Benchmark(Baseline = true)]
        public async Task Newtonsoft_SaveTestDataWithJsonSerializationBenchmark()
        {
            var data = new TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            saveWithSerializationDbContext.DataWithJsonSerializations.Add(data);
            await saveWithSerializationDbContext.SaveChangesAsync();
        }

        [BenchmarkCategory(GetTestDataName), Benchmark(Baseline = true)]
        public async Task<List<TestDataDto>> Newtonsoft_GetTestDataWithJsonSerializationBenchmark()
        {
            var dataWithJson = await dbContextWithLoadedDataWithJson.DataWithJsonSerializations.ToListAsync();

            var data = mapper.Map<List<TestDataDto>>(dataWithJson);
            return data;
        }

        #endregion


        #region SystemJson

        private AppDbContext systemJson_dbContextWithLoadedDataWithJson;
        private AppDbContext systemJson_saveWithSerializationDbContext;

        [GlobalSetup(Targets = new[] { nameof(SystemJson_GetTestDataWithJsonSerializationBenchmark) })]
        public async Task SystemJson_GetWithJsonGlobalSetup()
        {
            systemJson_dbContextWithLoadedDataWithJson = await TestDataSeedHelper.SystemJson_GetLoadedDatabaseWithJson(TestValuesCount);
        }

        [IterationSetup(Targets = new[] { nameof(SystemJson_SaveTestDataWithJsonSerializationBenchmark) })]
        public void SystemJson_SetupTestDataWithSerialization()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            systemJson_saveWithSerializationDbContext = new AppDbContext(options);
        }

        [IterationCleanup(Targets = new[] { nameof(SystemJson_SaveTestDataWithJsonSerializationBenchmark) })]
        public void SystemJson_CleanupTestDataWithSerialization()
        {
            systemJson_saveWithSerializationDbContext?.Dispose();
        }

        [BenchmarkCategory(SaveTestDataName), Benchmark]
        public async Task SystemJson_SaveTestDataWithJsonSerializationBenchmark()
        {
            var data = new SystemJson_TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            systemJson_saveWithSerializationDbContext.SystemJson_TestDataWithJsonSerializations.Add(data);
            await systemJson_saveWithSerializationDbContext.SaveChangesAsync();
        }

        [BenchmarkCategory(GetTestDataName), Benchmark]
        public async Task<List<TestDataDto>> SystemJson_GetTestDataWithJsonSerializationBenchmark()
        {
            var dataWithJson = await systemJson_dbContextWithLoadedDataWithJson.SystemJson_TestDataWithJsonSerializations.ToListAsync();

            var data = mapper.Map<List<TestDataDto>>(dataWithJson);
            return data;
        }

        #endregion


        #region ByteArray_SystemJson

        private AppDbContext byteArray_SystemJson_dbContextWithLoadedDataWithJson;
        private AppDbContext byteArray_SystemJson_saveWithSerializationDbContext;

        [GlobalSetup(Targets = new[] { nameof(ByteArray_GetTestDataWithJsonSerializationBenchmark) })]
        public async Task ByteArray_GetWithJsonGlobalSetup()
        {
            byteArray_SystemJson_dbContextWithLoadedDataWithJson = await TestDataSeedHelper.ByteArray_SystemJson_GetLoadedDatabaseWithJson(TestValuesCount);
        }

        [IterationSetup(Targets = new[] { nameof(ByteArray_SaveTestDataWithJsonSerializationBenchmark) })]
        public void ByteArray_SetupTestDataWithSerialization()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            byteArray_SystemJson_saveWithSerializationDbContext = new AppDbContext(options);
        }

        [IterationCleanup(Targets = new[] { nameof(ByteArray_SaveTestDataWithJsonSerializationBenchmark) })]
        public void ByteArray_CleanupTestDataWithSerialization()
        {
            byteArray_SystemJson_saveWithSerializationDbContext?.Dispose();
        }

        [BenchmarkCategory(SaveTestDataName), Benchmark]
        public async Task ByteArray_SaveTestDataWithJsonSerializationBenchmark()
        {
            var data = new ByteArray_SystemJson_TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            byteArray_SystemJson_saveWithSerializationDbContext.ByteArray_SystemJson_TestDataWithJsonSerializations.Add(data);
            await byteArray_SystemJson_saveWithSerializationDbContext.SaveChangesAsync();
        }

        [BenchmarkCategory(GetTestDataName), Benchmark]
        public async Task<List<TestDataDto>> ByteArray_GetTestDataWithJsonSerializationBenchmark()
        {
            var dataWithJson = await byteArray_SystemJson_dbContextWithLoadedDataWithJson.ByteArray_SystemJson_TestDataWithJsonSerializations.ToListAsync();

            var data = mapper.Map<List<TestDataDto>>(dataWithJson);
            return data;
        }

        #endregion


        #region SystemJsonCompressed

        private AppDbContext systemJsonCompressed_dbContextWithLoadedDataWithJson;
        private AppDbContext systemJsonCompressed_saveWithSerializationDbContext;

        [GlobalSetup(Targets = new[] { nameof(SystemJsonCompressed_GetTestDataWithJsonSerializationBenchmark) })]
        public async Task SystemJsonCompressed_GetWithJsonGlobalSetup()
        {
            systemJsonCompressed_dbContextWithLoadedDataWithJson = await TestDataSeedHelper.SystemJsonCompressed_GetLoadedDatabaseWithJson(TestValuesCount);
        }

        [IterationSetup(Targets = new[] { nameof(SystemJsonCompressed_SaveTestDataWithJsonSerializationBenchmark) })]
        public void SystemJsonCompressed_SetupTestDataWithSerialization()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            systemJsonCompressed_saveWithSerializationDbContext = new AppDbContext(options);
        }

        [IterationCleanup(Targets = new[] { nameof(SystemJsonCompressed_SaveTestDataWithJsonSerializationBenchmark) })]
        public void SystemJsonCompressed_CleanupTestDataWithSerialization()
        {
            systemJsonCompressed_saveWithSerializationDbContext?.Dispose();
        }

        [BenchmarkCategory(SaveTestDataName), Benchmark]
        public async Task SystemJsonCompressed_SaveTestDataWithJsonSerializationBenchmark()
        {
            var data = new SystemJsonCompressed_TestDataWithJsonSerialization()
            {
                TestDataValues = testDataValuesItems
            };

            systemJsonCompressed_saveWithSerializationDbContext.SystemJsonCompressed_TestDataWithJsonSerializations.Add(data);
            await systemJsonCompressed_saveWithSerializationDbContext.SaveChangesAsync();
        }

        [BenchmarkCategory(GetTestDataName), Benchmark]
        public async Task<List<TestDataDto>> SystemJsonCompressed_GetTestDataWithJsonSerializationBenchmark()
        {
            var dataWithJson = await systemJsonCompressed_dbContextWithLoadedDataWithJson.SystemJsonCompressed_TestDataWithJsonSerializations.ToListAsync();

            var data = mapper.Map<List<TestDataDto>>(dataWithJson);
            return data;
        }

        #endregion


        [GlobalCleanup]
        public void GlobalCleanup()
        {
            testDataValuesDtos = null;

            dbContextWithLoadedDataWithJson?.Dispose();
            systemJson_dbContextWithLoadedDataWithJson?.Dispose();
            systemJsonCompressed_dbContextWithLoadedDataWithJson?.Dispose();
        }
    }
}
