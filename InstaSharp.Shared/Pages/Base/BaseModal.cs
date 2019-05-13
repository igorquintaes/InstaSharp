using InstaSharp.Shared.Extensions;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages.Base
{
    public abstract class BaseModal
    {
        protected readonly IWebDriver driver;
        protected abstract string ModalXPath { get; }

        public BaseModal(IWebDriver driver) => 
            this.driver = driver;

        public void WaitForModal() => 
            driver.Wait()
                  .Until(x => x.FindElement(By.XPath(ModalXPath)).Displayed);
    }
}
