using Microsoft.Extensions.Configuration;
using Autofac;
using MySql.Data.MySqlClient;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.CloudFoundry.Connector.Services;

namespace CfWorkshopDotNet
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterMySqlConnection(this ContainerBuilder containerBuilder, IConfigurationRoot configuration)
        {
            MySqlProviderConnectorOptions mySqlProviderConnectorOptions = new MySqlProviderConnectorOptions(configuration);
            MySqlServiceInfo mySqlServiceInfo = configuration.GetSingletonServiceInfo<MySqlServiceInfo>();
            MySqlProviderConnectorFactory mySqlProviderConnectorFactory = new MySqlProviderConnectorFactory(
                mySqlServiceInfo, mySqlProviderConnectorOptions, typeof(MySqlConnection));
            containerBuilder.Register<MySqlConnection>(c => (MySqlConnection)mySqlProviderConnectorFactory.Create(null)).InstancePerLifetimeScope();
        }
    }
}