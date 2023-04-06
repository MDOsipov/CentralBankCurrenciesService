using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects;
using Contracts;
using HttpService;
using System.Dynamic;

namespace CBCurrenciesService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SingleCurrencyData, SingleCurrencyDto>();
			CreateMap<ExpandoObject, SingleCurrencyDto>();
		}
	}
}
