using AutoMapper;
using Crayon.API.Models.Domain;
using Crayon.API.Models.Dto;

namespace Crayon.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDto>();       
        }
    }
}
