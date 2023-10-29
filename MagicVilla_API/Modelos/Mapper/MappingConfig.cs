using AutoMapper;
using MagicVilla_API.Modelos.Dtos;

namespace MagicVilla_API.Modelos.Mapper
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();
            //una manera u otra.
            CreateMap<Villa, VillaCrearDto>().ReverseMap();  
        }
    }
}
