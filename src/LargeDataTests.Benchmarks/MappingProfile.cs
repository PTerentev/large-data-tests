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
        public static IMapper Mapper { get; } = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();

        public MappingProfile()
        {
            //CreateMap<TestValuesDto, TestDataValues>().ReverseMap();
            CreateMap<TestValuesDto, TestDataValuesItem>()
                .ForMember(d => d.Values, opt => opt.MapFrom(s => s.Values)).ReverseMap();

            CreateMap<TestDataDto, TestDataWithJsonSerialization>().ReverseMap();
            CreateMap<TestDataDto, TestDataWithoutSerialization>().ReverseMap();
            CreateMap<TestDataDto, SystemJson_TestDataWithJsonSerialization>().ReverseMap();
            CreateMap<TestDataDto, SystemJsonCompressed_TestDataWithJsonSerialization>().ReverseMap();

            CreateMap<TestDataDto, ByteArray_SystemJson_TestDataWithJsonSerialization>().ReverseMap();
        }
    }
}
