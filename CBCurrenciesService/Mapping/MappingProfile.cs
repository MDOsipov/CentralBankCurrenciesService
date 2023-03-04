using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects;
using Contracts;
using HttpService;

namespace CBCurrenciesService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SingleCurrencyData, SingleCurrencyDto>();
        }
    }
}
