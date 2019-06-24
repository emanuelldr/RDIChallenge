using RDI.ApplicationCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RDI.Tests.Unit.ApplicationCore.Services
{
    public class AbsoluteDifferenceService_Tests
    {
        AbsoluteDifferenceService adService = new AbsoluteDifferenceService();

        public class AbsoluteDifferenceService_Tests_Extension : AbsoluteDifferenceService_Tests
        {
            [Fact]
            public async void Given_SampleParameters()
            {
                int[] input = new int[] { 1, 9, 1, 3, 4, 4, 5, 6, 5 };
                int[] expectedResult = new int[] { 1, 1, 3, 4, 4, 5, 5 };
                int inputRef = 4;
                
                Assert.True(expectedResult.SequenceEqual(adService.Find(input, inputRef)));

                input = new int[] { 4, 6, 5, 3, 3, 1 };
                expectedResult = new int[] { 4, 5, 3, 3, 1 };

                Assert.True(expectedResult.SequenceEqual(adService.Find(input, inputRef)));


                input = new int[] { 0, 0, 0, 0, 0 };
                expectedResult = new int[] { 0, 0, 0, 0, 0 };
                Assert.True(expectedResult.SequenceEqual(adService.Find(input, inputRef)));

                input = new int[] { 4, 6, 5, 3, 3, -1 };
                expectedResult = new int[] { 3, 3, -1 };

                Assert.True(expectedResult.SequenceEqual(adService.Find(input, inputRef)));

            }

            [Fact]
            public async void Given_InvalidParameters_Expect_Exception()
            {
                int[] input = null;

                Action result = () => adService.Find(input);
                Assert.Throws<ArgumentNullException>(result);

                input = new int[] { };
                result = () => adService.Find(input);

                Assert.Throws<ArgumentOutOfRangeException>(result);

                input = new int[] {1, 2};
                result = () => adService.Find(input, -1);

                Assert.Throws<ArgumentOutOfRangeException>(result);
            }
        }
    }
}


