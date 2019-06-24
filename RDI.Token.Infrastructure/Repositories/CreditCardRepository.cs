using Microsoft.EntityFrameworkCore;
using RDI.Domain.DataContext;
using RDI.Domain.Token.Entities;
using RDI.Domain.Token.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RDI.Token.Infrastructure.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {

        private readonly TokenContext _context;

        public CreditCardRepository(TokenContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Insert(CreditCard card)
        {
            await _context.CreditCards.AddAsync(card);
            _context.SaveChanges();
        }

        public async Task<CreditCard> Get(long cardNumber)
        {
            return await _context.CreditCards
                                .Where(c => c.CardNumber == cardNumber)
                                .FirstOrDefaultAsync();
        }

        public async Task<CreditCard> Find(DateTime registrationDate)
        {
            var result = await _context.CreditCards
                                .Where(c => c.RegistrationDate == registrationDate)
                                .FirstOrDefaultAsync();

            return result;

        }

    }
}
