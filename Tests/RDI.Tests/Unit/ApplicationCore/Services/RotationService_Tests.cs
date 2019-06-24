using RDI.ApplicationCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RDI.Tests.Unit.ApplicationCore.Services
{
    public class RotationService_Tests
    {
        RotationService rotationService = new RotationService();

        public class RotationServiceExtension_Tests : RotationService_Tests
        {
            [Fact]
            public async void Given_SampleParameters()
            {
                int[] input = new int[] { 3, 4, 5 };
                int[] expectedResult = new int[] { 4, 5, 3 };
                int rotations = 2;

                Assert.True(expectedResult.SequenceEqual(rotationService.Rotate(input, rotations)));

                input = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                expectedResult = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0};
                rotations = 9;

                Assert.True(expectedResult.SequenceEqual(rotationService.Rotate(input, rotations)));

                input = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                expectedResult = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                rotations = 20;

                Assert.True(expectedResult.SequenceEqual(rotationService.Rotate(input, rotations)));

            }

            [Fact]
            public async void Given_InvalidParameters_Expect_Exception()
            {
                int[] input = null;

                Action result = () => rotationService.Rotate(input, 3);
                Assert.Throws<ArgumentNullException>(result);

                input = new int[] { };
                result = () => rotationService.Rotate(input, 3);

                Assert.Throws<ArgumentOutOfRangeException>(result);

                input = new int[] { 1, 2 };
                result = () => rotationService.Rotate(input, -3);

                Assert.Throws<ArgumentOutOfRangeException>(result);
            }
        }
    }
}

