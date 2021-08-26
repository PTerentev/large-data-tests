using System;
using System.Collections.Generic;
using System.Text;

namespace LargeDataTests.Domain.Entities
{
    public class SystemJson_TestDataWithJsonSerialization
    {
        public int Id { get; set; }

        public ICollection<TestDataValuesItem> TestDataValues { get; set; }
    }
}
