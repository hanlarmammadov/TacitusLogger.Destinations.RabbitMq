using Moq;
using RabbitMQ.Client;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq.Examples
{
    class ConfiguringWithBuilders
    {
        private IConnection connection;
        private IBasicProperties basicProperties;

        public void Adding_RabbitMq_With_Minimal_Configuration()
        {
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                          .RabbitMq()
                                          .WithConnection(connection)
                                          .WithAddress("exchangeName", "exchangeType", "routingKey")
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_RabbitMq_With_Custom_Publication_Address_Provider()
        {
            IPublicationAddressProvider customPublicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;

            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                          .RabbitMq()
                                          .WithConnection(connection)
                                          .WithAddress(customPublicationAddressProvider)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_RabbitMq_With_Publication_Address_Function()
        {
            LogModelFunc<PublicationAddress> publicationAddressFunc = (logModel) =>
            {
                if (logModel.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
                    return new PublicationAddress("direct", "ErrorLogs", "");
                else
                    return new PublicationAddress("direct", "InfoLogs", "");
            };
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                          .RabbitMq()
                                          .WithConnection(connection)
                                          .WithAddress(publicationAddressFunc)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_RabbitMq_With_Json_Log_Serializer()
        {
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                          .RabbitMq()
                                          .WithConnection(connection)
                                          .WithAddress("exchangeName", "exchangeType")
                                          .WithJsonLogText()
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_RabbitMq_With_Custom_Log_Serializer()
        {
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                          .RabbitMq()
                                          .WithConnection(connection)
                                          .WithAddress("exchangeName", "exchangeType")
                                          .WithLogSerializer(logSerializer)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_RabbitMq_With_Basic_Message_Publisher()
        {
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                          .RabbitMq()
                                          .WithConnection(connection)
                                          .WithAddress("exchangeName", "exchangeType", "routingKey")
                                          .WithPublisher(basicProperties)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_RabbitMq_With_Custom_Message_Publisher()
        {
            IMessagePublisher customMessagePublisher = new Mock<IMessagePublisher>().Object;

            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                          .RabbitMq()
                                          .WithConnection(connection)
                                          .WithAddress("exchangeName", "exchangeType", "routingKey")
                                          .WithPublisher(customMessagePublisher)
                                          .Add()
                                      .BuildLogger();
        }
    }
}
