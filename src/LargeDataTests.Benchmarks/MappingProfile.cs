using AutoMapper;
using LargeDataTests.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeDataTests.Benchmarks
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TestValuesDto, TestDataValues>().ReverseMap();
            CreateMap<TestValuesDto, TestDataValuesItem>().ReverseMap();

            CreateMap<TestDataDto, TestDataWithJsonSerialization>().ReverseMap();
            CreateMap<TestDataDto, TestDataWithoutSerialization>().ReverseMap();
        }
    }
}
