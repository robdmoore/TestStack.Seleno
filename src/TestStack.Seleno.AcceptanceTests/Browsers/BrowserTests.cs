using NUnit.Framework;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.AcceptanceTests.PageObjects;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.Browsers
{
    abstract class BrowserTests
    {
        protected abstract RemoteWebDriver WebDriver { get; }

        [Explicit]
        [Test]
        public void RunGoogleTest()
        {
            using (var host = new SelenoHost())
            {
                host.Run(x =>x.WithRemoteWebDriver(() => WebDriver)
                    .WithWebServer(new InternetWebServer("http://www.google.com/"))
                );

                var title = host.NavigateToInitialPage<Page>().Title;

                Assert.That(title, Is.EqualTo("Google"));
            }
        }

        [Explicit]
        [Test]
        public void RunDelayedJQueryTest()
        {
            using (var host = new SelenoHost())
            {
                host.Run("TestStack.Seleno.AcceptanceTests.Web", 12340, x => x.WithRemoteWebDriver(() => WebDriver));
                
                var value = host.NavigateToInitialPage<HomePage>()
                    .GoToJQueryPage()
                    .GetListItemValue();

                Assert.That(value, Is.EqualTo("hello"));
            }
        }
    }

    class SafariTests : BrowserTests
    {
        protected override RemoteWebDriver WebDriver
        {
            get { return BrowserFactory.Safari(); }
        }
    }

    class PhantomJSTests : BrowserTests
    {
        protected override RemoteWebDriver WebDriver
        {
            get { return BrowserFactory.PhantomJS(); }
        }
    }

    class FirefoxTests : BrowserTests
    {
        protected override RemoteWebDriver WebDriver
        {
            get { return BrowserFactory.FireFox(); }
        }
    }

    class ChromeTests : BrowserTests
    {
        protected override RemoteWebDriver WebDriver
        {
            get { return BrowserFactory.Chrome(); }
        }
    }

    class AndroidTests : BrowserTests
    {
        protected override RemoteWebDriver WebDriver
        {
            get { return BrowserFactory.Android(); }
        }
    }

    class IETests : BrowserTests
    {
        protected override RemoteWebDriver WebDriver
        {
            get { return BrowserFactory.InternetExplorer(); }
        }
    }
}
