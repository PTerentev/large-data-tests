using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeDataTests.Domain.Entities
{
    public class TestDataWithoutSerialization
    {
        public int Id { get; set; }

        public ICollection<TestDataValues> TestDataValues { get; set; }
    }
}
