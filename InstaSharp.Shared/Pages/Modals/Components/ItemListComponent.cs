using HtmlAgilityPack;
using InstaSharp.Shared.Enum;
using InstaSharp.Shared.Extensions;
using InstaSharp.Shared.PageObjects;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace InstaSharp.Shared.Pages.Modals.Components
{
    public class ItemListComponent
    {
        private readonly IWebDriver driver;

        public ItemListComponent(IWebDriver driver) => 
            this.driver = driver;

        public IEnumerable<ItemList> Obtain(int maxQuantity)
        {
            if (maxQuantity > 1000 || maxQuantity <= 0)
                throw new ArgumentException("Quantity is not a valid value, or higher than we support.");

            var document = new HtmlDocument();
            var itemNodes = default(HtmlNodeCollection);
            var foundNodesQuantity = 0;
            do
            {
                var documentHtml = By.XPath(
                    "//*[@role='dialog']//ul | " +
                    "//*[@role='dialog']/nav/following-sibling::div");

                document.LoadHtml(driver.FindElement(documentHtml).GetAttribute("innerHTML"));
                if (document.DocumentNode.SelectSingleNode("//li") != null)
                {
                    itemNodes = document.DocumentNode.SelectNodes("//li");
                    if (itemNodes.Count >= maxQuantity ||
                        itemNodes.Count == foundNodesQuantity)
                        break;

                    foundNodesQuantity = itemNodes.Count;
                    driver.FindElement(By.XPath($"(//li)[{foundNodesQuantity}]"))
                          .ScrollElement(driver);
                }
                else
                {
                    itemNodes = document.DocumentNode.SelectNodes("div");
                    break;
                }                
            } while (true);

            foreach(var itemNode in itemNodes)
            {
                var nodeDocument = new HtmlDocument();
                nodeDocument.LoadHtml(itemNode.InnerHtml);

                var title = nodeDocument.DocumentNode
                    .SelectSingleNode("(//a)[2] | //a")
                    .InnerText;

                var description = nodeDocument.DocumentNode
                    .SelectSingleNode($"//div[./div/a[@title='{title}']]/div[2] |" +
                                      $"//div[.//a[text()='{title}']]//span/span")
                    .InnerText;

                var imageUrl = nodeDocument.DocumentNode
                    .SelectSingleNode("//img")
                    .GetAttributeValue("src", string.Empty);

                var status = System.Enum.Parse<FollowerStatus>(
                    nodeDocument.DocumentNode.SelectSingleNode("//button").InnerText);

                yield return new ItemList(title, description, imageUrl, status);
            }
        }

        public void Unfollow(ItemList itemList) =>
            Unfollow(itemList.Title);

        public void Unfollow(string user丨Hashtag) =>
            throw new NotImplementedException();

        public void Follow(ItemList itemList) =>
            Follow(itemList.Title);

        public void Follow(string user丨Hashtag)
        {
            var itemXPath = By.XPath(
                $"//*[@role='dialog']//li[.//a[@title='{user丨Hashtag}']] | " +
                $"//*[@role='dialog']/nav/following-sibling::div[.//a[@title='{user丨Hashtag}']]");

            var buttonXPath = 
                $"//*[@role='dialog']//li[.//a[@title='{user丨Hashtag}']]//button |" +
                $"//*[@role='dialog']/nav/following-sibling::div[.//a[text()='{user丨Hashtag}']]//button";

            var document = new HtmlDocument();
            document.LoadHtml(driver.FindElement(itemXPath).GetAttribute("innerHTML"));
            var status = System.Enum.Parse<FollowerStatus>(
                        document.DocumentNode.SelectSingleNode("//button").InnerText);

            if (status != FollowerStatus.Follow)
                return;

            driver.FindElement(By.XPath(buttonXPath)).Click();
            driver.Wait().Until(x =>
            {
                var seleniumText = x.FindElement(By.XPath(buttonXPath)).Text;
                return !string.IsNullOrWhiteSpace(seleniumText) &&
                        FollowerStatus.Follow.ToString() != seleniumText;
            });
        }

    }
}
