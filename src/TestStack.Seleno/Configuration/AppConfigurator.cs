using System;
using System.Reflection;
using Funq;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.Infrastructure.Logging;
using TestStack.Seleno.Infrastructure.Logging.Loggers;

using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Configuration
{
    public class AppConfigurator : IAppConfigurator
    {
        protected WebApplication _webApplication;
        protected IWebServer _webServer;
        protected Func<Container, ICamera> _camera = c => new NullCamera();
        protected Func<IWebDriver> _webDriver = BrowserFactory.FireFox;
        protected ILogFactory _logFactory = new ConsoleLogFactory();
        protected Assembly[] _pageObjectAssemblies;

        public ISelenoApplication CreateApplication()
        {
            _logFactory
                .GetLogger(GetType())
                .InfoFormat("Seleno v{0}, .NET Framework v{1}",
                    typeof(SelenoApplicationRunner).Assembly.GetName().Version, Environment.Version);

            var container = BuildContainer();
            var app = new SelenoApplication(container);

            return app;
        }

        private Container BuildContainer()
        {
            var container = new Container();
            container.Register(c => _webServer ?? new IisExpressWebServer(_webApplication));
            container.Register(c => _webDriver.Invoke());
            container.Register(_camera);

            container.Register<IElementFinder>(c => new ElementFinder(c.Resolve<IWebDriver>()));
            container.Register<IScriptExecutor>(
                c => new ScriptExecutor(c.Resolve<IWebDriver>(), c.Resolve<IWebDriver>().GetJavaScriptExecutor(),
                                        c.Resolve<IElementFinder>(), c.Resolve<ICamera>()));
            container.Register<IPageNavigator>(
                c => new PageNavigator(c.Resolve<IWebDriver>(), c.Resolve<IScriptExecutor>(),
                                       c.Resolve<IWebServer>(), c.Resolve<IComponentFactory>()));
            container.Register<IComponentFactory>(
                c => new ComponentFactory(c));

            return container;
        }

        public AppConfigurator ProjectToTest(WebApplication webApplication)
        {
            _webApplication = webApplication;
            return this;
        }

        public AppConfigurator WithWebServer(IWebServer webServer)
        {
            _webServer = webServer;
            return this;
        }

        public AppConfigurator WithWebDriver(Func<IWebDriver> webDriver)
        {
            _webDriver = webDriver;
            return this;
        }

        public AppConfigurator UsingCamera(ICamera camera)
        {
            _camera = c => camera;
            return this;
        }

        public AppConfigurator UsingCamera(string screenShotPath)
        {
            _camera = c => new FileCamera(c.Resolve<IWebDriver>(), screenShotPath);
            return this;
        }

        public AppConfigurator UsingLogger(ILogFactory logFactory)
        {
            _logFactory = logFactory;
            return this;
        }

        public AppConfigurator WithPageObjectsFrom(Assembly[] assemblies)
        {
            _pageObjectAssemblies = assemblies;
            return this;
        }
    }

    public class PageObjectScanner : IFunqlet
    {
        readonly Assembly[] _assemblies;

        public PageObjectScanner(Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public void Configure(Container container)
        {
            // scan assemblies for classes implementing UiComponent
            // register them with container
        }
    }
}