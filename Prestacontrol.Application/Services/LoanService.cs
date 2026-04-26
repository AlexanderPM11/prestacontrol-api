using AutoMapper;
using Prestacontrol.Application.DTOs;
using Prestacontrol.Application.Interfaces;
using Prestacontrol.Domain.Entities;
using Prestacontrol.Domain.Enums;
using Prestacontrol.Domain.Interfaces;

namespace Prestacontrol.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoanDto> CreateLoanAsync(CreateLoanRequest request, int userId)
        {
            decimal totalInterest = request.Amount * (request.InterestRate / 100);
            decimal totalToPay = request.Amount + totalInterest;
            decimal installmentAmount = totalToPay / request.InstallmentsCount;
            decimal principalPerInstallment = request.Amount / request.InstallmentsCount;
            decimal interestPerInstallment = totalInterest / request.InstallmentsCount;

            var loan = new Loan
            {
                ClientId = request.ClientId,
                UserId = userId,
                Amount = request.Amount,
                InterestRate = request.InterestRate,
                Frequency = request.Frequency,
                InstallmentsCount = request.InstallmentsCount,
                StartDate = request.StartDate,
                TotalToPay = totalToPay,
                BalanceDue = totalToPay,
                Status = LoanStatus.Active,
                EndDate = CalculateEndDate(request.StartDate, request.Frequency, request.InstallmentsCount)
            };

            // Generate Installments
            for (int i = 1; i <= request.InstallmentsCount; i++)
            {
                loan.Installments.Add(new Installment
                {
                    InstallmentNumber = i,
                    DueDate = CalculateDueDate(request.StartDate, request.Frequency, i),
                    Amount = installmentAmount,
                    PrincipalAmount = principalPerInstallment,
                    InterestAmount = interestPerInstallment,
                    Status = InstallmentStatus.Pending
                });
            }

            await _unitOfWork.Loans.AddAsync(loan);
            
            // Record Disbursement in CashFlow
            await _unitOfWork.CashFlows.AddAsync(new CashFlow
            {
                Amount = request.Amount,
                Type = CashFlowType.Outcome,
                Category = "Préstamo",
                Description = $"Desembolso préstamo a cliente ID: {request.ClientId}",
                UserId = userId,
                Date = DateTime.Now
            });

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LoanDto>(loan);
        }

        public async Task<IEnumerable<LoanDto>> GetClientLoansAsync(int clientId)
        {
            var loans = await _unitOfWork.Loans.FindAsync(l => l.ClientId == clientId);
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<LoanDto?> GetLoanDetailsAsync(int loanId)
        {
            var loan = await _unitOfWork.Loans.GetByIdAsync(loanId);
            return _mapper.Map<LoanDto>(loan);
        }

        private DateTime CalculateDueDate(DateTime start, LoanFrequency freq, int installmentNumber)
        {
            return freq switch
            {
                LoanFrequency.Diario => start.AddDays(installmentNumber),
                LoanFrequency.Semanal => start.AddDays(installmentNumber * 7),
                LoanFrequency.Quincenal => start.AddDays(installmentNumber * 15),
                LoanFrequency.Mensual => start.AddMonths(installmentNumber),
                _ => start.AddDays(installmentNumber)
            };
        }

        private DateTime CalculateEndDate(DateTime start, LoanFrequency freq, int count)
        {
            return CalculateDueDate(start, freq, count);
        }
    }
}
