using System;
using System.Threading.Tasks;

namespace RDI.Domain.ApplicationCore.Interfaces
{
    public interface ITokenService
    {
        Task<long> CreateToken(long cardNumber, int cvv, DateTime registrationDate, bool validationProcess);
        Task<bool> ValidateToken(DateTime registrationDate, long token, int cvv);
    }
}
