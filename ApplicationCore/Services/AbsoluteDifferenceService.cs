using RDI.Domain.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RDI.ApplicationCore.Services
{
    public class AbsoluteDifferenceService : IAbsoluteDifferenceService
    {
        /// <summary>
        /// Find the array such that the absolute difference between any two of the chosen integers is less than or equal to refValue.
        /// 
        /// Pay attention:
        /// ** It's possible to generate more than one array by the document definition
        /// ** With this sample input: {4, 6, 5, 3, 3, 1} and absolute difference ≤ 4, We can generate {4, 6, 5, 3, 3} or {4, 5, 3, 3, 1}
        /// ** But, as the document says the correct return is {4, 5, 3, 3, 1}, I will return the array with the lower values.
        /// </summary>
        /// <param name="inputArray">Input array</param>
        /// <param name="refValue">absolute difference to be considered</param>
        /// <returns>Array such that the absolute difference between any two of the chosen integers is less than or equal to refValue</returns>
        public int[] Find(int[] inputArray, int refValue = 5)
        {
            //Validate input, throws exception if its necessary
            validateInput(inputArray, refValue);

            //Find the find the lowest value.
            var minValue = inputArray.Min();

            //Find the greatest value to be returned.
            int limitValue = minValue + refValue;

            List<int> result = new List<int>();
            foreach (int input in inputArray)
            {
                if (input <= limitValue)
                    result.Add(input);
            }

            return result.ToArray();
        }


        //Validations throws exception depending on inputs
        private void validateInput(int[] inputArray, int refValue)
        {
            if (inputArray == null)
                throw new ArgumentNullException("inputArray must be not null");

            if (inputArray.Length == default(int))
                throw new ArgumentOutOfRangeException("inputArray can't be empty");

            if (refValue < default(int))
                throw new ArgumentOutOfRangeException("refValue must be greater or equal than zero");
        }
    }
}
