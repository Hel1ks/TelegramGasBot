using AutoMapper;
using GasApi.Data.Entities;
using GasApi.Dtos;

namespace GasApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDataEntity, GetPersonalAccountResponse>()
                .AfterMap((src, dest) => dest.ResponseCode = ResponseCodeEnum.Success.ToString());
            CreateMap<UserDataEntity, GetReadingsResponse>()
                .AfterMap((src, dest) => dest.ResponseCode = ResponseCodeEnum.Success.ToString());
            CreateMap<UserDataEntity, GetPaymentsResponse>()
                .AfterMap((src, dest) => dest.ResponseCode = ResponseCodeEnum.Success.ToString());
            CreateMap<PaymentEntity, PaymentDto>().ReverseMap();
            CreateMap<ReadingEntity, ReadingDto>().ReverseMap();
        }
    }
}
