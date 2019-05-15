using InstaSharp.Shared.Extensions;
using InstaSharp.Shared.Pages.Base;
using InstaSharp.Shared.Pages.Modals;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace InstaSharp.Shared.Pages
{
    public class UserPage : LoggedInBasePage
    {
        private readonly UserFollowersModal userFollowersModal;

        protected override string UrlPath => "/{0}/";

        public UserPage(
            IWebDriver driver,
            UserFollowersModal userFollowersModal)
        : base(driver) => 
            this.userFollowersModal = userFollowersModal;

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

        public void FollowingButton() => 
            throw new NotImplementedException();
    }
}
