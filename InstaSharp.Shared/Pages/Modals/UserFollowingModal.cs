using InstaSharp.Shared.Enum;
using InstaSharp.Shared.Extensions;
using InstaSharp.Shared.Pages.Base;
using InstaSharp.Shared.Pages.Modals.Components;
using OpenQA.Selenium;
using System.Linq;

namespace InstaSharp.Shared.Pages.Modals
{
    public class UserFollowingModal : BaseModal
    {
        public ItemListComponent ItemListComponent { get; }
        protected override string ModalXPath => "//*[@role='dialog']";

        public UserFollowingModal(IWebDriver driver, ItemListComponent itemListComponent)
            : base(driver) => ItemListComponent = itemListComponent;

        public UserFollowingTab UserFollowingTab
        {
            get => driver
                .FindElements(By.XPath($"{ModalXPath}//nav/a[contains(@href, '/following')][@aria-current='page']"))
                .Any()
                    ? UserFollowingTab.People
                    : UserFollowingTab.HashTag;
            set
            {
                var element = driver.FindElement(By.XPath($"{ModalXPath}//nav/a[{(int)value}]"));
                if (element.GetAttribute("aria-current") != "page")
                    return;

                element.Click();

                driver.Wait().Until(x =>
                    x.FindElements(By.XPath($"{ModalXPath}//nav/following-sibling::div/svg"))
                     .FirstOrDefault(y => !y.Displayed) == null);
            }
        }
    }
}
