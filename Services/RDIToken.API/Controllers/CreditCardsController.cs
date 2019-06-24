using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RDI.Domain.ApplicationCore.Interfaces;
using RDI.Token.API.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RDI.Token.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardsController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public CreditCardsController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        /// <summary>
        ///  Receive credit card data and generate a token based on data provided and return it back in the response.
        /// </summary>
        /// <param name="creditCard">Card Number and Cvv(Card Verification Value)</param>
        /// <returns>Response containing Token and Registragion Date </returns>
        [Route("", Name = "CreateToken")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreditCardResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateToken([FromBody] CreditCardRequest creditCard)
        {
            try
            {
                var registrationDate = DateTime.UtcNow;

                var token = await _tokenService.CreateToken(creditCard.CardNumber, creditCard.CVV, registrationDate, false);

                var response = new CreditCardResponse { RegistrationDate = registrationDate, Token = token };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Validate a provided token.
        /// </summary>
        /// <param name="registrationDate">Registration Date</param>
        /// <param name="token">Token</param>
        /// <param name="cvv">CVV - Card Verification Value</param>
        /// <returns>Boolean wich indicate if token is valid or not</returns>
        [Route("Token/{registrationDate}/{token}/{cvv}/Check", Name = "CheckToken")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<bool> CheckToken(DateTime registrationDate, long token, int cvv)
        {
            try
            {
                return await _tokenService.ValidateToken(registrationDate.ToUniversalTime(), token, cvv);
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
