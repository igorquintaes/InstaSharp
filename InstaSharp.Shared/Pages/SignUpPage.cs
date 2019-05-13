using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;
using System;

namespace InstaSharp.Shared.Pages
{
    public class SignUpPage : LoggedOutBasePage
    {
        public SignUpPage(IWebDriver driver) : base(driver)
        {
        }

        protected override string UrlPath => throw new NotImplementedException();
    }
}
