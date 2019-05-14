using Bogus;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Drawing;

namespace InstaSharp.UnitTests.Specs
{
    public class BaseTest
    {
        protected IWebDriver fakeDriver;
        protected IWebElement fakeElement;
        protected Faker faker;

        [SetUp]
        public void BaseSetUp()
        {
            fakeDriver = A.Fake<IWebDriver>(builder =>
                builder.Implements<IJavaScriptExecutor>());

            fakeElement = A.Fake<IWebElement>();
            faker = new Faker();
        }

        protected void MockAdjustElementExibition(IWebDriver driver, IWebElement element)
        {
            A.CallTo(() => driver
                .As<IJavaScriptExecutor>()
                .ExecuteScript(A<string>._, A<IWebElement>._))
            .Returns(A.Dummy<int>());

            A.CallTo(() => element.Location)
                .Returns(new Point(A.Dummy<int>(), A.Dummy<int>()));
        }

        protected void MockScrollElement(IWebDriver driver) =>
            A.CallTo(() => driver
                .As<IJavaScriptExecutor>()
                .ExecuteScript(A<string>._, A<IWebElement>._));

    }
}
