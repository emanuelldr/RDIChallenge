using RDI.Domain.Token.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RDI.Tests.Base;
using RDI.Token.Infrastructure.Repositories;
using Xunit;
using System;
using System.Threading.Tasks;

namespace RDI.Tests.Integration.Infrastructures.Token
{
    public class CreditCardRepository_Tests : BaseTest
    {
        private readonly ICreditCardRepository repository;

        public CreditCardRepository_Tests() : base()
        {
            services.AddTransient<ICreditCardRepository, CreditCardRepository>();
            var provider = services.BuildServiceProvider();

            repository = provider.GetService<ICreditCardRepository>();
        }

        public class CreditCardRepository_Tests_Extension : CreditCardRepository_Tests
        {

            [Fact]
            public async void Given_ValidParameters()
            {
                var creditCard = new Domain.Token.Entities.CreditCard
                {
                    CardNumber = 99999999999,
                    RegistrationDate = DateTime.UtcNow
                };

                var exception = await Record.ExceptionAsync(() => repository.Insert(creditCard));
                Assert.IsNotType<Exception>(exception);

                var result = await repository.Get(creditCard.CardNumber);
                Assert.True(result.CardNumber == creditCard.CardNumber);

                result = await repository.Find(creditCard.RegistrationDate);
                Assert.True(result.CardNumber == creditCard.CardNumber);
                
            }

        }
    }
}
