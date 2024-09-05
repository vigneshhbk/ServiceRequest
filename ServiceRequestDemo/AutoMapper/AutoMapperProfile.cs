using AutoMapper;
using ServiceRequestDemo.DatabaseAccess.TableClasses;
using ServiceRequestDemo.Models;

namespace ServiceRequestDemo.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<ServiceRequest, ServiceRequestDTO>()
                .ForMember(dest => dest.CurrentStatus,
                opt => opt.MapFrom(src => (Common.CurrentStatus)src.CurrentStatus));

            CreateMap<ServiceRequestDTO, ServiceRequest>()
                .ForMember(dest => dest.CurrentStatus,
                opt => opt.MapFrom(src => (int)src.CurrentStatus));
        }
    }
}
