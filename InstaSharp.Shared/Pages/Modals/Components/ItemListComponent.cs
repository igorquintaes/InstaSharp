using HtmlAgilityPack;
using InstaSharp.Shared.Enum;
using InstaSharp.Shared.Extensions;
using InstaSharp.Shared.PageObjects;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace InstaSharp.Shared.Pages.Modals.Components
{
    public class ItemListComponent
    {
        private readonly IWebDriver driver;

        public ItemListComponent(IWebDriver driver) =>
            this.driver = driver;

        public List<ItemList> Obtain(int maxQuantity)
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
                    driver.FindElement(By.XPath($"(//*[@role='dialog']//li)[{foundNodesQuantity}]"))
                          .ScrollElement(driver);

                    try
                    {
                        driver.Wait().Until(x => x.FindElements(By.XPath($"//*[@role='dialog']//li"))
                              .Count > foundNodesQuantity);

                        Thread.Sleep(300);
                    }
                    catch { }
                }
                else
                {
                    itemNodes = document.DocumentNode.SelectNodes("div");
                    break;
                }
            } while (true);

            var itemList = new List<ItemList>();
            foreach (var itemNode in itemNodes)
            {
                var nodeDocument = new HtmlDocument();
                nodeDocument.LoadHtml(itemNode.InnerHtml);

                var title = nodeDocument.DocumentNode
                    .SelectNodes("(//a)[2] | //a")
                    .FirstOrDefault(x => !string.IsNullOrEmpty(x.InnerText))
                    .InnerText;

                var description = nodeDocument.DocumentNode
                    .SelectSingleNode($"//div[./div/a[@title='{title}']]/div[2] |" +
                                      $"//div[.//a[text()='{title}']]//span/span")
                    ?.InnerText;

                var imageUrl = nodeDocument.DocumentNode
                    .SelectSingleNode("//img")
                    .GetAttributeValue("src", string.Empty);

                var status = System.Enum.Parse<FollowerStatus>(
                    nodeDocument.DocumentNode.SelectSingleNode("//button").InnerText);

                itemList.Add(new ItemList(title, description, imageUrl, status));
            }

            return itemList;
        }

        public void Unfollow(ItemList itemList) =>
            Unfollow(itemList.Title);

        public void Unfollow(string user丨Hashtag)
        {
            var buttonXPath =
                $"//*[@role='dialog']//li[.//a[@title='{user丨Hashtag}']]//button |" +
                $"//*[@role='dialog']/nav/following-sibling::div[./div//a[text()='{user丨Hashtag}']]//button";

            var button = driver
                .FindElements(By.XPath(buttonXPath))
                .FirstOrDefault();

            if (button == null || 
                button.GetCssValue("color") == "#fff" ||
                button.GetCssValue("color") == "rgba(255, 255, 255, 1)")
                return;

            var c = button.GetCssValue("color");

            button.Click();
            driver.Wait().Until(x => x
                .FindElement(By.XPath("(//*[@role='dialog'])[2]//button[1]")))
                .Click();

            driver.Wait().Until(x => 
                x.FindElement(By.XPath(buttonXPath))
                 .GetCssValue("color") == "rgba(255, 255, 255, 1)");
        }

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
