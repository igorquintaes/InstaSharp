using HtmlAgilityPack;
using InstaSharp.Shared.Enum;
using InstaSharp.Shared.Extensions;
using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstaSharp.Shared.Pages.Modals
{
    public class UserFollowersModal : BaseModal
    {
        protected override string ModalXPath =>
            "//*[@role='dialog']//ul";

        public UserFollowersModal(IWebDriver driver)
            : base(driver)
        { }
        
        public IDictionary<string, FollowerStatus> GetFollowers(int maxQuantity)
        {
            if (maxQuantity > 1000 || maxQuantity <= 0)
                throw new ArgumentException("Quantity is not a valid value, or higher than we support.");

            var document = new HtmlDocument();
            var followersNodes = default(HtmlNodeCollection);
            var foundNodesQuantity = 0;
            do
            {
                document.LoadHtml(driver.FindElement(By.XPath(ModalXPath)).GetAttribute("innerHTML"));
                followersNodes = document.DocumentNode.SelectNodes("//li");
                if (followersNodes.Count >= maxQuantity ||
                    followersNodes.Count == foundNodesQuantity)
                    break;

                foundNodesQuantity = followersNodes.Count;
                driver.FindElement(By.XPath($"(//li)[{foundNodesQuantity}]"))
                      .ScrollElement(driver);
            } while (true);

            return new Dictionary<string, FollowerStatus>(
                followersNodes.Select(x =>
                {
                    var nodeDocument = new HtmlDocument();
                    nodeDocument.LoadHtml(x.InnerHtml);

                    var userName = nodeDocument.DocumentNode.SelectSingleNode("(//a)[2]")?.InnerText
                                ?? nodeDocument.DocumentNode.SelectSingleNode("//a")?.InnerText;

                    var status = System.Enum.Parse<FollowerStatus>(
                        nodeDocument.DocumentNode.SelectSingleNode("//button").InnerText);

                    return new KeyValuePair<string, FollowerStatus>(userName, status);
                }));
        }

        public void Unfollow(string username) => 
            throw new NotImplementedException();

        public void Follow(string username)
        {
            var userXpath = $"{ModalXPath}//li[.//a[@title='{username}' and text()='{username}']]//button";

            var document = new HtmlDocument();
            document.LoadHtml(driver.FindElement(By.XPath(
                $"{ModalXPath}//li[.//a[@title='{username}' and text()='{username}']]")).GetAttribute("innerHTML"));

            var status = System.Enum.Parse<FollowerStatus>(
                        document.DocumentNode.SelectSingleNode("//button").InnerText);

            if (status != FollowerStatus.Follow)
                return;

            driver.FindElement(By.XPath(userXpath)).Click();
            driver.Wait().Until(x =>
            {
                var seleniumText = x.FindElement(By.XPath(userXpath)).Text;
                return !string.IsNullOrWhiteSpace(seleniumText) &&
                        FollowerStatus.Follow.ToString() != seleniumText;
            });
        }
    }
}
