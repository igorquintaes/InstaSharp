using FluentAssertions;
using InstaSharp.Shared.Extensions;
using NUnit.Framework;

namespace InstaSharp.UnitTests.Specs.Extensions
{
    public class StringExtesionsTest
    {
        public class OnlyNumberTests : StringExtesionsTest
        {
            [TestCase("0", 0)]
            [TestCase("250,200", 250200)]
            [TestCase("250.200", 250200)]
            [TestCase("7.000 posts", 7000)]
            [TestCase("7 and 5", 75)]
            public void ShouldExtractAndReturnOnlyNumbersOnText(string text, int expectedResult) =>
                text.OnlyNumbers().Should().Be(expectedResult);
        }
    }
}
