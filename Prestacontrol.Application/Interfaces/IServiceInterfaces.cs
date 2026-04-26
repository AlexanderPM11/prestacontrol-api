using Prestacontrol.Application.DTOs;

namespace Prestacontrol.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<UserDto> RegisterAsync(UserDto userDto, string password);
    }

    public interface ILoanService
    {
        Task<LoanDto> CreateLoanAsync(CreateLoanRequest request, int userId);
        Task<IEnumerable<LoanDto>> GetClientLoansAsync(int clientId);
        Task<LoanDto?> GetLoanDetailsAsync(int loanId);
    }

    public interface IJwtService
    {
        string GenerateToken(Domain.Entities.User user);
    }
}
