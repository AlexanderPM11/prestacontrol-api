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
            CreateMap<Loan, LoanDto>();
            CreateMap<Installment, InstallmentDto>();
            
            CreateMap<CreateLoanRequest, Loan>();
            CreateMap<FinancialTransaction, TransactionDto>();
        }
    }
}
