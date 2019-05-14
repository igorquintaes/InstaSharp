using Bogus;
using FakeItEasy;
using FluentAssertions;
using InstaSharp.Shared.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InstaSharp.UnitTests.Specs.Pages
{
    public class UserPageTest
    {
        protected IWebDriver driver;
        protected UserPage userPage;
        protected Faker faker;

        [SetUp]
        public void ClassSetUp()
        {
            driver = A.Fake<IWebDriver>();
            userPage = new UserPage(driver, null);
            faker = new Faker();
        }

        public class PostQuantityTest : UserPageTest
        {
            protected IWebElement postElement;

            [SetUp]
            public void SetUp()
            {
                postElement = A.Fake<IWebElement>();
                A.CallTo(() => driver.FindElement(A<By>._)).Returns(postElement);
            }

            [TestCase("5", 5)]
            [TestCase("0", 0)]
            [TestCase("100.200", 100200)]
            [TestCase("100,200", 100200)]
            public void ShouldReturnExpectedQuantityOfPosts(string textElement, int expectedQuantity)
            {
                A.CallTo(() => postElement.Text).Returns(textElement);
                userPage.PostQuantity.Should().Be(expectedQuantity);
            }
        }

        public class FollowersTest : UserPageTest
        {
            protected IWebElement postElement;

            [SetUp]
            public void SetUp()
            {
                postElement = A.Fake<IWebElement>();
                A.CallTo(() => driver.FindElement(A<By>._)).Returns(postElement);
            }

            [TestCase("5", 5)]
            [TestCase("0", 0)]
            [TestCase("1,500,520", 1500520)]
            public void ShouldReturnExpectedQuantityOfFollowers(string textElement, int expectedQuantity)
            {
                A.CallTo(() => postElement.GetAttribute("title")).Returns(textElement);
                userPage.FollowersQuantity.Should().Be(expectedQuantity);
            }
        }

        public class FollowingTest : UserPageTest
        {
            protected IWebElement postElement;

            [SetUp]
            public void SetUp()
            {
                postElement = A.Fake<IWebElement>();
                A.CallTo(() => driver.FindElement(A<By>._)).Returns(postElement);
            }

            [TestCase("5", 5)]
            [TestCase("0", 0)]
            [TestCase("1,500,520", 1500520)]
            public void ShouldReturnExpectedQuantityOfFollowedProfiles(string textElement, int expectedQuantity)
            {
                A.CallTo(() => postElement.Text).Returns(textElement);
                userPage.FollowingQuantity.Should().Be(expectedQuantity);
            }
        }

        public class NameTest : UserPageTest
        {

            private IWebElement nameElement;

            [SetUp]
            public void SetUp()
            {
                nameElement = A.Fake<IWebElement>();
                A.CallTo(() => driver.FindElement(A<By>._)).Returns(nameElement);
            }

            [Test]
            public void ShoulReturnName()
            {
                var name = faker.Person.FirstName;
                A.CallTo(() => nameElement.Text).Returns(name);
                userPage.Name.Should().Be(name);
            }
        }

        public class DescriptionTest: UserPageTest
        {
            private IWebElement descriptionElement;

            [SetUp]
            public void SetUp() => descriptionElement = A.Fake<IWebElement>();


            [Test]
            public void ShouldReturnProfileDescription()
            {
                A.CallTo(() =>
                    driver.FindElements(A<By>._))
                          .Returns(new ReadOnlyCollection<IWebElement>(new[] { descriptionElement }));

                var description = faker.Lorem.Lines(3);

                A.CallTo(() => descriptionElement.Text).Returns(description);
                userPage.Description.Should().Be(description);
            }

            [Test]
            public void ShouldReturnNullWhenNoDescription()
            {
                A.CallTo(() => driver.FindElements(A<By>._))
                    .Returns(new ReadOnlyCollection<IWebElement>(new IWebElement [] { }));

                userPage.Description.Should().BeNull();
            } 
        }
    }
}
