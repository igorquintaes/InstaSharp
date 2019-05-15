using FakeItEasy;
using FluentAssertions;
using InstaSharp.Shared.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace InstaSharp.UnitTests.Specs.Pages
{
    public class UserPageTest : BaseTest
    {
        protected UserPage userPage;

        [SetUp]
        public void ClassSetUp() => 
            userPage = new UserPage(fakeDriver, null);

        public class PostQuantityTest : UserPageTest
        {
            [SetUp]
            public void SetUp() => 
                A.CallTo(() => fakeDriver.FindElement(A<By>._)).Returns(fakeElement);

            [TestCase("5", 5)]
            [TestCase("0", 0)]
            [TestCase("100.200", 100200)]
            [TestCase("100,200", 100200)]
            public void ShouldReturnExpectedQuantityOfPosts(string textElement, int expectedQuantity)
            {
                A.CallTo(() => fakeElement.Text).Returns(textElement);
                userPage.PostQuantity.Should().Be(expectedQuantity);
            }
        }

        public class FollowersTest : UserPageTest
        {
            [SetUp]
            public void SetUp() => 
                A.CallTo(() => fakeDriver.FindElement(A<By>._)).Returns(fakeElement);

            [TestCase("5", 5)]
            [TestCase("0", 0)]
            [TestCase("1,500,520", 1500520)]
            public void ShouldReturnExpectedQuantityOfFollowers(string textElement, int expectedQuantity)
            {
                A.CallTo(() => fakeElement.GetAttribute("title")).Returns(textElement);
                userPage.FollowersQuantity.Should().Be(expectedQuantity);
            }
        }

        public class FollowingTest : UserPageTest
        {
            [SetUp]
            public void SetUp() => 
                A.CallTo(() => fakeDriver.FindElement(A<By>._)).Returns(fakeElement);

            [TestCase("5", 5)]
            [TestCase("0", 0)]
            [TestCase("1,500,520", 1500520)]
            public void ShouldReturnExpectedQuantityOfFollowedProfiles(string textElement, int expectedQuantity)
            {
                A.CallTo(() => fakeElement.Text).Returns(textElement);
                userPage.FollowingQuantity.Should().Be(expectedQuantity);
            }
        }

        public class NameTest : UserPageTest
        {
            [SetUp]
            public void SetUp() => 
                A.CallTo(() => fakeDriver.FindElement(A<By>._)).Returns(fakeElement);

            [Test]
            public void ShoulReturnName()
            {
                var name = faker.Person.FirstName;
                A.CallTo(() => fakeElement.Text).Returns(name);
                userPage.Name.Should().Be(name);
            }
        }

        public class DescriptionTest: UserPageTest
        {
            [Test]
            public void ShouldReturnProfileDescription()
            {
                A.CallTo(() => fakeDriver.FindElements(A<By>._))
                    .Returns(new ReadOnlyCollection<IWebElement>(new[] { fakeElement }));

                var description = faker.Lorem.Lines(3);

                A.CallTo(() => fakeElement.Text).Returns(description);
                userPage.Description.Should().Be(description);
            }

            [Test]
            public void ShouldReturnNullWhenNoDescription()
            {
                A.CallTo(() => fakeDriver.FindElements(A<By>._))
                    .Returns(new ReadOnlyCollection<IWebElement>(new IWebElement [] { }));

                userPage.Description.Should().BeNull();
            } 
        }
    }
}
