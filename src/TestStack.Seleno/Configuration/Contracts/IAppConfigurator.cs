using System;
using System.Reflection;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Infrastructure.Logging;

namespace TestStack.Seleno.Configuration.Contracts
{
    public interface IAppConfigurator
    {
        AppConfigurator ProjectToTest(WebApplication webApplication);
        AppConfigurator WithWebServer(IWebServer webServer);
        AppConfigurator WithWebDriver(Func<IWebDriver> webDriver);
        AppConfigurator UsingCamera(ICamera camera);
        AppConfigurator UsingCamera(string screenShotPath);
        AppConfigurator UsingLogger(ILogFactory logFactory);
        AppConfigurator WithPageObjectsFrom(Assembly[] assemblies);
    }
}