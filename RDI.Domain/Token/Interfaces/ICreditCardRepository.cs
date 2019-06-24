using RDI.Domain.Token.Entities;
using System;
using System.Threading.Tasks;

namespace RDI.Domain.Token.Interfaces
{
    public interface ICreditCardRepository
    {
        Task Insert(CreditCard card);
        Task<CreditCard> Get(long cardNumber);
        Task<CreditCard> Find(DateTime registrationDate);
    }
}
