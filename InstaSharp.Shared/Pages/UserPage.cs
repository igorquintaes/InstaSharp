using HtmlAgilityPack;
using InstaSharp.Shared.Enum;
using InstaSharp.Shared.Extensions;
using InstaSharp.Shared.Pages.Base;
using InstaSharp.Shared.Pages.Modals;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InstaSharp.Shared.Pages
{
    public class UserPage : LoggedInBasePage
    {
        private readonly UserFollowersModal userFollowersModal;
        private readonly UserFollowingModal userFollowingModal;

        protected override string UrlPath => "/{0}/";

        public UserPage(
            IWebDriver driver,
            UserFollowersModal userFollowersModal,
            UserFollowingModal userFollowingModal)
        : base(driver)
        {
            this.userFollowersModal = userFollowersModal;
            this.userFollowingModal = userFollowingModal;
        }

        public int PostQuantity => Convert.ToInt32(
            driver.FindElement(By.XPath("//*[@role='main']//header//ul/li[1]/span/span"))
                  .Text.OnlyNumbers());

        public int FollowersQuantity => Convert.ToInt32(
            driver.FindElement(By.XPath("(//section//li)[2]/a/span"))
                  .GetAttribute("title")
                  .OnlyNumbers());

        public int FollowingQuantity => Convert.ToInt32(
            driver.FindElement(By.XPath("(//section//li)[3]/a/span"))
                  .Text.OnlyNumbers());

        public string Name => driver
            .FindElement(By.XPath("(//*[@role='main']//header//h1)[2]"))
            .Text;

        public string Description => driver
            .FindElements(By.XPath("//*[@role='main']//header//h1/following-sibling::span"))
            .FirstOrDefault()
            ?.Text;

        public UserFollowersModal FollowersButton()
        {
            var buttonXPath = "(//section//li)[2]/a";
            driver
                .FindElement(By.XPath(buttonXPath))
                .AdjustElementExibition(driver)
                .Click();

            userFollowersModal.WaitForModal();
            return userFollowersModal;
        }

        public UserFollowingModal FollowingButton()
        {
            var buttonXPath = "(//section//li)[3]/a";
            driver
                .FindElement(By.XPath(buttonXPath))
                .AdjustElementExibition(driver)
                .Click();

            userFollowingModal.WaitForModal();
            return userFollowingModal;
        }

        public IEnumerable<string> GetPostsLinks(int quantity)
        {
            var postQuantity = PostQuantity;

            if (quantity > postQuantity)
                quantity = postQuantity;

            LoadPosts();
            var document = new HtmlDocument();
            document.LoadHtml(driver.PageSource);
            
            foreach(var postLink in document.DocumentNode.SelectNodes("//article//a"))
                yield return postLink.GetAttributeValue("href", string.Empty);

            void LoadPosts()
            {
                var elements = driver.FindElements(By.XPath("//article//a"));
                if (elements.Count < quantity)
                {
                    elements.Last().ScrollElement(driver);
                    driver.Wait().Until(x => x.FindElements(By.XPath("//article//a")).Count > elements.Count);
                    LoadPosts();
                }

                return;
            }
        }
    }
}
