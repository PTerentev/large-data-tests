using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeDataTests.Domain.Entities
{
    public class BaseTestDataValues
    {
        public double Time { get; set; }
        public IReadOnlyCollection<double> Values { get; set; }
    }
}
