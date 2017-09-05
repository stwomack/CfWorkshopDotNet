using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.PlatformAbstractions;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace CfWorkshopDotNet
{
    public class ProviderConfig
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static CloudFoundryApplicationOptions ApplicationOptions
        {
            get
            {
                var options = new CloudFoundryApplicationOptions();
                ConfigurationBinder.Bind(Configuration, options);
                return options;
            }
        }

        public static CloudFoundryServicesOptions ServicesOptions
        {
            get
            {
                var options = new CloudFoundryServicesOptions();
                ConfigurationBinder.Bind(Configuration, options);
                return options;
            }
        }

        public static void RegisterConfigProviders(String environmentName)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole(LogLevel.Debug);

            var environment = new HostingEnvironment(environmentName);
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCloudFoundry();

            Configuration = builder.Build();
        }
    }

    public class HostingEnvironment : IHostingEnvironment
    {
        public HostingEnvironment(String environmentName)
        {
            EnvironmentName = environmentName;
            ApplicationName = PlatformServices.Default.Application.ApplicationName;
            ContentRootPath = PlatformServices.Default.Application.ApplicationBasePath;
        }

        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}