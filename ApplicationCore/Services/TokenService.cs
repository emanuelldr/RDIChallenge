using RDI.Domain.ApplicationCore.Interfaces;
using RDI.Domain.Token.Entities;
using RDI.Domain.Token.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RDI.ApplicationCore.Services
{
    public class TokenService : ITokenService
    {
        private IAbsoluteDifferenceService _adService;
        private IRotationService _rotationService;
        private ICreditCardRepository _repository;

        public TokenService(IAbsoluteDifferenceService absoluteDifferenceService, IRotationService rotationService, ICreditCardRepository repository)
        {
            _adService = absoluteDifferenceService;
            _rotationService = rotationService;
            _repository = repository;
        }

        /// <summary>
        /// Responsible for business lógic of token generation
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="cvv"></param>
        /// <param name="registrationDate"></param>
        /// <param name="validationProcess"></param>
        /// <returns>Token</returns>
        public async Task<long> CreateToken(long cardNumber, int cvv, DateTime registrationDate, bool validationProcess)
        {
            //validations
            validateCardNumber(cardNumber);
            validateCvv(cvv);
            validateRegistrationDate(registrationDate);

            //Generate initial token based on concatenation of Card Number and registration Date
            var token = cardNumber.ToString() + registrationDate.ToString("yyyyMMddHHmm");


            //Find new array by absolute difference
            token = FindArrayByAbsoluteDifference(token);

            //Find new array by applying right circular rotations considering k = cvv
            token = ApplyRotations(token, cvv);


            //case a validation process return without saving in db
            if (validationProcess)
                return Convert.ToInt64(token);


            var card = new CreditCard
            {
                CardNumber = cardNumber,
                RegistrationDate = registrationDate
            };

            //Save Card Number on database
            await _repository.Insert(card);

            //return token
            return Convert.ToInt64(token);
        }


        /// <summary>
        /// Responsible for business lógic of token validation
        /// </summary>
        /// <param name="registrationDate"></param>
        /// <param name="token"></param>
        /// <param name="cvv"></param>
        /// <returns>boolean wich indicate if token is valid or not</returns>
        public async Task<bool> ValidateToken(DateTime registrationDate, long token, int cvv)
        {
            //pre validations
            validateCvv(cvv);
            validateRegistrationDate(registrationDate);

            if (token <= default(int) || token.ToString().Length == default(int))
                return false;

            //if registration date toke toked more than 15 minutes
            if(registrationDate < DateTime.UtcNow.AddMinutes(-15))
                return false;

            var dbCard = await _repository.Find(registrationDate);

            //haven't found Card so is an invalid token 
            if (dbCard == null)
                return false;

            //Create a new token
            var dbCardToken = await CreateToken(dbCard.CardNumber, cvv, registrationDate, true);

            //check if created token is the same of provided one and return
            return (token == dbCardToken);
        }


        //Auxiliar method for finding a new array using absolute difference algorithm
        private string FindArrayByAbsoluteDifference(string token)
        {
            var intArray = token.Select(v => (int) Char.GetNumericValue(v)).ToArray();

            var newArray = _adService.Find(intArray, 5);

            return string.Join("", newArray); 
        }


        //Auxiliar method for applying rotation using right circular rotation algorithm
        private string ApplyRotations(string token, int cvv)
        {
            var intArray = token.Select(v => (int)Char.GetNumericValue(v)).ToArray();

            var newArray = _rotationService.Rotate(intArray, cvv);

            return string.Join("", newArray);
        }



        //some validations
        #region Validations

        private void validateCardNumber(long cardNumber)
        {
            if (cardNumber <= default(int) || cardNumber.ToString().Length > 16)
                throw new ArgumentOutOfRangeException("Card Number must have between 1 and 5 digits");
        }

        private void validateCvv(int cvv)
        {
            if (cvv <= default(int) || cvv.ToString().Length > 5)
                throw new ArgumentOutOfRangeException("CVV must have between 1 and 5 digits");
        }

        private void validateRegistrationDate(DateTime registrationDate)
        {
            if (registrationDate == DateTime.MinValue)
                throw new ArgumentOutOfRangeException("Registration Date must be a valid date");

            if (registrationDate < DateTime.UtcNow.AddMinutes(-15))
                throw new ArgumentOutOfRangeException("Registration date have already took 15 minutes.");

            if (registrationDate > DateTime.UtcNow)
                throw new ArgumentOutOfRangeException("Registration date can't be a future date");
        }

        #endregion

    }
}
