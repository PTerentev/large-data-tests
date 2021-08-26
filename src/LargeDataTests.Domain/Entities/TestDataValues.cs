using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeDataTests.Domain.Entities
{
    public class TestDataValues
    {
        public int Id { get; set; }

        public TestDataWithoutSerialization TestData { get; set; }

        public int TestDataId { get; set; }
    }
}
