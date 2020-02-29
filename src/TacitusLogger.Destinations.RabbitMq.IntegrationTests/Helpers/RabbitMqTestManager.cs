using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.Destinations.RabbitMq.IntegrationTests
{
    internal class RabbitMqTestManager
    {
        protected static IConfigurationRoot _configs;
        private static IConnection _rabbitMqConnection;
        private List<string> _createdExchangeNames = new List<string>();
        private static string _objectPrefixes;

        static RabbitMqTestManager()
        {
            _configs = new ConfigurationBuilder()
                                     .AddJsonFile(".//rabbitmq-configs.json")
                                     .Build();

            IConfigurationSection rabbitMqSection = _configs.GetSection("rabbitmq");
            var factory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = rabbitMqSection["host"],
                Port = rabbitMqSection.GetValue<int>("port"),
                UserName = rabbitMqSection["username"],
                Password = rabbitMqSection["password"]
            };
            _rabbitMqConnection = factory.CreateConnection();
            _objectPrefixes = rabbitMqSection["objectPrefixes"];
        }

        public IConfigurationRoot Configs => _configs;
        public IConnection Connection => _rabbitMqConnection;
        public string ObjectPrefixes => _objectPrefixes;

        public string DeclareExchangeQueuePairAndReturnName(string type = "direct")
        {
            string name = $"{_objectPrefixes}{ Samples.Strings.RandomGuidBased() }";
            var model = Connection.CreateModel();
            model.ExchangeDeclare(name, type, true);
            model.QueueDeclare(name, true, false, false);
            model.QueueBind(name, name, "");
            model.Close();
            _createdExchangeNames.Add(name);
            return name;
        }
        public void CleanUp()
        {
            var model = Connection.CreateModel();
            foreach (string name in _createdExchangeNames)
            {
                model.QueueDelete(name);
                model.ExchangeDelete(name);
            }
        }
        public List<string> GetMessagesFromQueue(string name)
        {
            List<string> messages = new List<string>();
            IModel model = Connection.CreateModel();
            while (true)
            {
                BasicGetResult result = null;
                try
                {
                    result = model.BasicGet(name, true);
                }
                catch (Exception) { }
                if (result == null)
                    break;
                messages.Add(Encoding.UTF8.GetString(result.Body));
            }
            return messages;
        }
    }
}
