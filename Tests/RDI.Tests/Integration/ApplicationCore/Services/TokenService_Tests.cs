using Microsoft.Extensions.DependencyInjection;
using RDI.ApplicationCore.Services;
using RDI.Domain.ApplicationCore.Interfaces;
using RDI.Domain.Token.Interfaces;
using RDI.Tests.Base;
using RDI.Token.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RDI.Tests.Integration.ApplicationCore.Services
{
    public class TokenService_Tests : BaseTest
    {
        private readonly ITokenService _tokenService;
        public TokenService_Tests() : base()
        {
            services.AddTransient<IAbsoluteDifferenceService, AbsoluteDifferenceService>();
            services.AddTransient<IRotationService, RotationService>();
            services.AddTransient<ICreditCardRepository, CreditCardRepository>();
            services.AddTransient<ITokenService, TokenService>();

            var provider = services.BuildServiceProvider();

            _tokenService = provider.GetService<ITokenService>();
        }


        public class TokenService_Tests_Extension : TokenService_Tests
        {
            //Some unit test on integration test layer
            [Fact]
            public async void Given_InvalidParameters_ExpectException()
            {

                //Card Number Invalid
                Func<Task> result = () => _tokenService.CreateToken(0, 234, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);

                result = () => _tokenService.CreateToken(-1, 234, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);

                result = () => _tokenService.CreateToken(999999999999999999, 234, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);


                //Cvv Invalid
                result = () => _tokenService.CreateToken(9999999999999999, 0, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);

                result = () => _tokenService.CreateToken(9999999999999999, -1, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);

                result = () => _tokenService.CreateToken(9999999999999999, 999999, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);


                //Cvv Invalid
                result = () => _tokenService.CreateToken(9999999999999999, 0, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);

                result = () => _tokenService.CreateToken(9999999999999999, -1, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);

                result = () => _tokenService.CreateToken(9999999999999999, 999999, DateTime.UtcNow, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);


                //Registration Date Invalid
                result = () => _tokenService.CreateToken(9999999999999999, 999, DateTime.MinValue, false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);

                result = () => _tokenService.CreateToken(9999999999999999, 999, DateTime.UtcNow.AddMinutes(-16), false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);

                result = () => _tokenService.CreateToken(9999999999999999, 999, DateTime.UtcNow.AddMinutes(1), false);
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);
            }

            [Fact]
            public async void Given_ValidParameters()
            {
                var registrationDate = DateTime.UtcNow;
                var token = await _tokenService.CreateToken(1234567899876543, 99999, registrationDate, false);

                Assert.True(await _tokenService.ValidateToken(registrationDate, token, 99999));

                Assert.False(await _tokenService.ValidateToken(DateTime.UtcNow, token, 99999));

                Assert.False(await _tokenService.ValidateToken(registrationDate, 123456789123456, 99999));

                Assert.False(await _tokenService.ValidateToken(registrationDate, token, 12345));

            }
        }
    }
}
