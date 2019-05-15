using FakeItEasy;
using FluentAssertions;
using InstaSharp.Shared.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Drawing;

namespace InstaSharp.UnitTests.Specs.Extensions
{
    public class IWebElementExtensionsTest : BaseTest
    {
        public class AdjustElementExibitionTest : IWebElementExtensionsTest
        {
            [Test]
            public void ShouldReturnTheSameWebElementPassedByParameter()
            {
                A.CallTo(() => fakeDriver
                    .As<IJavaScriptExecutor>()
                    .ExecuteScript(A<string>.That.Contains("innerHeight")))
                .Returns(A.Dummy<int>());

                A.CallTo(() => fakeElement.Location)
                    .Returns(new Point(A.Dummy<int>(), A.Dummy<int>()));

                var returnedElement = fakeElement.AdjustElementExibition(fakeDriver);
                returnedElement.Should().Be(fakeElement);
            }

            [Test]
            public void ShouldScrollAsExpected()
            {
                A.CallTo(() => fakeDriver
                    .As<IJavaScriptExecutor>()
                    .ExecuteScript(A<string>.That.Contains("innerHeight")))
                .Returns(100);

                A.CallTo(() => fakeElement.Location)
                    .Returns(new Point(A.Dummy<int>(), 20));

                const int expectedScroll = -30;

                fakeElement.AdjustElementExibition(fakeDriver);

                A.CallTo(() => fakeDriver
                    .As<IJavaScriptExecutor>()
                    .ExecuteScript($"window.scrollTo(0, {expectedScroll})"))
                .MustHaveHappenedOnceExactly();
            }
        }

        public class ScrollElementTest : IWebElementExtensionsTest
        {
            [Test]
            public void ShouldExecuteScriptOnce()
            {
                fakeElement.ScrollElement(fakeDriver);
                A.CallTo(() => fakeDriver
                    .As<IJavaScriptExecutor>()
                    .ExecuteScript(A<string>._, A<IWebElement>._))
                .MustHaveHappenedOnceExactly();
            }
        }
    }
}
