using Moq;
using RabbitMQ.Client;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq.Examples
{
    class Configuring
    {
        private IConnection connection;
        private IBasicProperties basicProperties;

        public void Adding_RabbitMq_With_Minimal_Configuration()
        {
            IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, publicationAddressProvider);

            Logger logger = new Logger();
            logger.AddLogDestinations(rabbitMqDestination);
        }
        public void Adding_RabbitMq_With_Custom_Publication_Address_Provider()
        {
            IPublicationAddressProvider customPublicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, customPublicationAddressProvider);

            Logger logger = new Logger();
            logger.AddLogDestinations(rabbitMqDestination);
        }
        public void Adding_RabbitMq_With_Publication_Address_Function()
        {
            FactoryMethodPublicationAddressProvider factoryMethodPublicationAddressProvider = new FactoryMethodPublicationAddressProvider((logModel) =>
            {
                if (logModel.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
                    return new PublicationAddress("direct", "ErrorLogs", "");
                else
                    return new PublicationAddress("direct", "InfoLogs", "");
            });
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, factoryMethodPublicationAddressProvider);

            Logger logger = new Logger();
            logger.AddLogDestinations(rabbitMqDestination);
        }
        public void Adding_RabbitMq_With_Json_Log_Serializer()
        {
            IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
            ILogSerializer logSerializer = new JsonLogSerializer();
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, publicationAddressProvider, logSerializer);

            Logger logger = new Logger();
            logger.AddLogDestinations(rabbitMqDestination);
        }
        public void Adding_RabbitMq_With_Custom_Log_Serializer()
        {
            IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
            ILogSerializer customLogSerializer = new Mock<ILogSerializer>().Object;
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, publicationAddressProvider, customLogSerializer);

            Logger logger = new Logger();
            logger.AddLogDestinations(rabbitMqDestination);
        }
        public void Adding_RabbitMq_With_Basic_Message_Publisher()
        {
            IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
            ILogSerializer logSerializer = new JsonLogSerializer();
            IMessagePublisher basicMessagePublisher = new BasicMessagePublisher(basicProperties);
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, publicationAddressProvider, logSerializer, basicMessagePublisher);

            Logger logger = new Logger();
            logger.AddLogDestinations(rabbitMqDestination);
        }
        public void Adding_RabbitMq_With_Custom_Message_Publisher()
        {

            IPublicationAddressProvider publicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "exchangeType", "routingKey");
            ILogSerializer logSerializer = new JsonLogSerializer();
            IMessagePublisher customMessagePublisher = new Mock<IMessagePublisher>().Object;
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(connection, publicationAddressProvider, logSerializer, customMessagePublisher);

            Logger logger = new Logger();
            logger.AddLogDestinations(rabbitMqDestination);
        }
    }
}
