using System;
using Autofac;
using MySql.Data.MySqlClient;
using log4net;

namespace CfWorkshopDotNet
{
    public class DbConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DbConfig));

        public static void InitializeDb(IContainer container)
        {
            if (container == null)
            {
                log.Error("Container is null.");
                throw new ArgumentNullException("container");
            }

            using (var connection = container.Resolve<MySqlConnection>())
            {
                connection.Open();
                CreateDbSchema(connection);
                InsertSampleData(connection);
                connection.Close();
            }
        }

        private static void CreateDbSchema(MySqlConnection connection)
        {
            log.Info("Dropping and creating Notes database tables.");
            MySqlCommand command = new MySqlCommand("DROP TABLE IF EXISTS NOTES", connection);
            command.ExecuteNonQuery();
            command = new MySqlCommand("CREATE TABLE NOTES (ID INT PRIMARY KEY, CREATED DATETIME DEFAULT CURRENT_TIMESTAMP(), TEXT VARCHAR(255))", connection);
            command.ExecuteNonQuery();
        }

        private static void InsertSampleData(MySqlConnection connection)
        {
            log.Info("Inserting sample notes data.");
            MySqlCommand command = new MySqlCommand("INSERT INTO NOTES (ID, TEXT) VALUES (1, 'Note 1')", connection);
            command.ExecuteNonQuery();
            command = new MySqlCommand("INSERT INTO NOTES (ID, TEXT) VALUES (2, 'Note 2')", connection);
            command.ExecuteNonQuery();
        }
    }
}