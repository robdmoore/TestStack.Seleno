using System;

using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Infrastructure.Logging;

namespace TestStack.Seleno.Configuration
{
    public static class SelenoApplicationRunner
    {
        private static readonly ILog Log = LogManager.GetLogger("Seleno");
        public static ISelenoApplication Host { get; private set; }
       
        public static void Run(string webProjectFolder, 
                               int portNumber, 
                               Action<IAppConfigurator> configure = null)
        {
            var webApplication = new WebApplication(ProjectLocation.FromFolder(webProjectFolder), portNumber);
            Run(webApplication, configure);
        }

        public static void Run(WebApplication app, Action<IAppConfigurator> configure)
        {
            try
            {
                Action<IAppConfigurator> action = x =>
                {
                    x.ProjectToTest(app);

                    if (configure != null)
                        configure(x);
                };
                
               Host = New(action);
            }
            catch (Exception ex)
            {
                Log.Error("The Seleno Application exited abnormally with an exception", ex);
                throw;
            }
        }

        public static void Run(Action<IAppConfigurator> configure)
        {
            try
            {
                Action<IAppConfigurator> action = x =>
                {
                    if (configure != null)
                        configure(x);
                };

                Host = New(action);
            }
            catch (Exception ex)
            {
                Log.Error("The Seleno Application exited abnormally with an exception", ex);
                throw;
            }
        }

        private static ISelenoApplication New(Action<IAppConfigurator> configure)
        {
            if (configure == null)
                throw new ArgumentNullException("configure");

            var configurator = new AppConfigurator();
            configure(configurator);
            Host = configurator.CreateApplication();
            Host.Initialize();

            return Host;
        }
    }
}