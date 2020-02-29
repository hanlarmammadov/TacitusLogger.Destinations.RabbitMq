//using Microsoft.Extensions.Configuration;
//using RabbitMQ.Client;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace TacitusLogger.Destinations.RabbitMq.IntegrationTests
//{
//    public abstract class IntegrationTestBase
//    {
//        protected static IConfigurationRoot _configs;
//        private static IConnection _rabbitMqConnection;
//        private static readonly object _rabbitMqConnectionLock = new object();
//        private static readonly object _configsLock = new object();

//        protected IConfigurationRoot Configs
//        {
//            get
//            {
//                if (_configs == null)
//                    lock (_configsLock)
//                        if (_configs == null)
//                            _configs = new ConfigurationBuilder()
//                                      .AddJsonFile(".//rabbitmq-configs.json")
//                                      .Build();
//                return _configs;
//            }
//        }

//        protected IConnection TestRabbitMqConnection
//        {
//            get
//            {
//                if (_rabbitMqConnection == null)
//                    lock (_rabbitMqConnectionLock)
//                        if (_rabbitMqConnection == null)
//                        {
//                            IConfigurationSection rabbitMqSection = Configs.GetSection("rabbitmq");
//                            var factory = new RabbitMQ.Client.ConnectionFactory()
//                            {
//                                HostName = rabbitMqSection["host"],
//                                Port = rabbitMqSection.GetValue<int>("port"),
//                                UserName = rabbitMqSection["username"],
//                                Password = rabbitMqSection["password"]
//                            };
//                            _rabbitMqConnection = factory.CreateConnection();
//                        }
//                return _rabbitMqConnection;
//            }
//        }
//        protected LogModel GetSampleLogModel(LogType logType = LogType.Error, string context = "Context1")
//        {
//            return new LogModel()
//            {
//                LogId = GetRandomGuidBasedName(),
//                Source = "Source1",
//                Context = context,
//                LogType = logType,
//                Description = "Description1",
//                LoggingObject = new { Name = "Value" },
//                LogDate = DateTime.Now,
//                Attachments = new List<LogAttachment>()
//            };
//        }
//        protected string GetRandomGuidBasedName(int length = 10)
//        {
//            return Guid.NewGuid().ToString("n").Substring(0, 10);
//        }
//        protected string DeclareExchangeQueuePairAndReturnName(string type = "direct")
//        {
//            string name = $"TacitusLogger.Destinations.RabbitMq.IntTests.{GetRandomGuidBasedName()}";
//            var model = TestRabbitMqConnection.CreateModel();
//            model.ExchangeDeclare(name, type, true);
//            model.QueueDeclare(name, true, false, false);
//            model.QueueBind(name, name, "");
//            model.Close();
//            return name;
//            model.ExchangeDelete()
//        }

//    }
//}
