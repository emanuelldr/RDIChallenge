using RDI.Domain.ApplicationCore.Interfaces;
using System;

namespace RDI.ApplicationCore.Services
{
    public class RotationService : IRotationService
    {

        /// <summary>
        /// Perform a number of right circular rotations and return this array.
        /// </summary>
        /// <param name="inputArray">Array of integers, with values for rotations</param>
        /// <param name="rotations">Number of rotations, as k rotations in document</param>
        /// <returns>Return a reordered array after a number of rotations/returns>
        public int[] Rotate(int[] inputArray, int rotations)
        {

            //Validate input, throws exception if its necessary
            validateInput(inputArray, rotations);

            int length = inputArray.Length;


            //There is no need to rotate more than array length.
            if (rotations >= length)
            {
                rotations = rotations % length;
            }

            //Without rotations to peform, return the input array.
            if (rotations == 0)
                return inputArray;


            int[] resultArray = new int[length];


            for (int index = 0; index < length; index++)
            {
                int position = 0;

                //For the first part of result array, before index reaches rotation number. (Get the value from the end of input array)
                if (index < rotations)
                {

                    position = (length + index - rotations);
                    resultArray[index] = inputArray[position];


                }
                else //After index reaches rotation number. (Get the value from the begining of input array)
                {
                    position = (index - rotations);
                    resultArray[index] = inputArray[position];
                }
            }

            return resultArray;
        }


        //Validations throws exception depending on inputs
        private void validateInput(int[] inputArray, int rotations)
        {
            if (inputArray == null)
                throw new ArgumentNullException("inputArray must be not null");

            if (inputArray.Length == default(int))
                throw new ArgumentOutOfRangeException("inputArray can't be empty");

            if (rotations < default(int))
                throw new ArgumentOutOfRangeException("refValue must be greater or equal than zero");
        }
    }
}
