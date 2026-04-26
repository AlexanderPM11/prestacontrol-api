using AutoMapper;
using Prestacontrol.Application.DTOs;
using Prestacontrol.Domain.Entities;

namespace Prestacontrol.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Client, ClientDto>().ReverseMap();
            
            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.FullName));
            
            CreateMap<Installment, InstallmentDto>();
            
            CreateMap<CreateLoanRequest, Loan>();
        }
    }
}
