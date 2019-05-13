using OpenQA.Selenium;
using System;
using System.Threading;

namespace InstaSharp.Shared.Extensions
{
    public static class IWebElementExtensions
    {
        public static IWebElement AdjustElementExibition(this IWebElement element, IWebDriver driver)
        {
            var jsDriver = ((IJavaScriptExecutor)driver);
            var windowHeight = Convert.ToInt32(jsDriver.ExecuteScript("return window.innerHeight;"));
            var scrollTo = element.Location.Y - windowHeight / 2;

            jsDriver.ExecuteScript($"window.scrollTo(0, {scrollTo})");
            Thread.Sleep(80);
            return element;
        }

        public static void ScrollElement(this IWebElement element, IWebDriver driver) => 
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].scrollIntoView(true);", element);
    }
}
