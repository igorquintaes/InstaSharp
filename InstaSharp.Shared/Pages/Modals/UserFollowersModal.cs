using InstaSharp.Shared.Pages.Base;
using InstaSharp.Shared.Pages.Modals.Components;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages.Modals
{
    public class UserFollowersModal : BaseModal
    {
        public ItemListComponent ItemListComponent { get; }
        protected override string ModalXPath => "//*[@role='dialog']//ul";

        public UserFollowersModal(IWebDriver driver, ItemListComponent itemListComponent)
            : base(driver) => ItemListComponent = itemListComponent;
    }
}
