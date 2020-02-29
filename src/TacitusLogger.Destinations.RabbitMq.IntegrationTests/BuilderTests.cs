using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Builders;
using TacitusLogger.Destinations.RabbitMq;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq.IntegrationTests
{
    [TestFixture]
    public class BuilderTests
    {
        RabbitMqTestManager _rabbitMqTestManager;

        [TearDown]
        public void TearDown()
        {
            _rabbitMqTestManager.CleanUp();
        }
        [SetUp]
        public void SetUp()
        {
            _rabbitMqTestManager = new RabbitMqTestManager();
        }

        [Test]
        public void LoggerBuilder_With_One_LogGroup_Containing_One_RabbitMq_Destination_With_Custom_Components()
        {
            // Arrange
            var connection = new Mock<IConnection>().Object;
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            // Act 
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                                    .RabbitMq()
                                                                                    .WithConnection(connection)
                                                                                    .WithAddress(publicationAddressProvider)
                                                                                    .WithLogSerializer(logSerializer)
                                                                                    .WithPublisher(messagePublisher)
                                                                                    .Add()
                                                                                    .BuildLogger();
            // Assert
            var logGroup = (LogGroup)logger.GetLogGroup("group1");
            Assert.AreEqual(1, logGroup.LogDestinations.Count);
            Assert.IsInstanceOf<RabbitMqDestination>(logGroup.LogDestinations[0]);
            var rabbitMqDestination = (RabbitMqDestination)logGroup.LogDestinations[0];
            // Connection asserts.
            Assert.AreEqual(connection, rabbitMqDestination.RabbitMqConnection);
            // Publication address provider asserts.
            Assert.AreEqual(publicationAddressProvider, rabbitMqDestination.PublicationAddressProvider);
            // Log serializer asserts.
            Assert.AreEqual(logSerializer, rabbitMqDestination.LogSerializer);
            // Message publisher asserts.
            Assert.AreEqual(messagePublisher, rabbitMqDestination.MessagePublisher);
        }
         
        [Test]
        public void LoggerBuilder_With_RabbitMq_Destination_When_Connection_Is_Not_Specified_Throws_InvalidOperationException()
        {
            // Arrange
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act 
                Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                                        .RabbitMq()
                                                                                        .WithAddress(publicationAddressProvider)
                                                                                        .WithLogSerializer(logSerializer)
                                                                                        .WithPublisher(messagePublisher)
                                                                                        .Add()
                                                                                        .BuildLogger();
            }); 
        }
         
        [Test]
        public void LoggerBuilder_With_RabbitMq_Destination_When_Publication_Address_Provider_Is_Not_Specified_Throws_InvalidOperationException()
        {
            // Arrange
            var connection = new Mock<IConnection>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act 
                Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                                        .RabbitMq()
                                                                                        .WithConnection(connection)
                                                                                        .WithLogSerializer(logSerializer)
                                                                                        .WithPublisher(messagePublisher)
                                                                                        .Add()
                                                                                        .BuildLogger();
            });
        }

        [Test]
        public void LoggerBuilder_With_RabbitMq_Destination_When_Log_Serializer_Is_Not_Specified_Sets_Default_Log_Serializer()
        {
            // Arrange
            var connection = new Mock<IConnection>().Object;
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object; 
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            // Act 
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                                    .RabbitMq()
                                                                                    .WithConnection(connection)
                                                                                    .WithAddress(publicationAddressProvider) 
                                                                                    .WithPublisher(messagePublisher)
                                                                                    .Add()
                                                                                    .BuildLogger();
            // Assert
            var logGroup = (LogGroup)logger.GetLogGroup("group1");
            Assert.AreEqual(1, logGroup.LogDestinations.Count);
            Assert.IsInstanceOf<RabbitMqDestination>(logGroup.LogDestinations[0]);
            var rabbitMqDestination = (RabbitMqDestination)logGroup.LogDestinations[0];
            // Log serializer asserts.
            Assert.IsInstanceOf<JsonLogSerializer>(rabbitMqDestination.LogSerializer);
            var logSerializer = (JsonLogSerializer)rabbitMqDestination.LogSerializer;
            Assert.AreEqual(JsonLogSerializer.DefaultConverter, logSerializer.Converter);
            Assert.AreEqual(JsonLogSerializer.DefaultJsonSerializerSettings, logSerializer.JsonSerializerSettings);
        }

        [Test]
        public void LoggerBuilder_With_RabbitMq_Destination_When_Message_Publisher_Not_Specified_Sets_Default_Message_Publisher()
        {
            // Arrange
            var connection = new Mock<IConnection>().Object;
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;

            // Act 
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                                    .RabbitMq()
                                                                                    .WithConnection(connection)
                                                                                    .WithAddress(publicationAddressProvider)
                                                                                    .WithLogSerializer(logSerializer)
                                                                                    .Add()
                                                                                    .BuildLogger();
            // Assert
            var logGroup = (LogGroup)logger.GetLogGroup("group1");
            Assert.AreEqual(1, logGroup.LogDestinations.Count);
            Assert.IsInstanceOf<RabbitMqDestination>(logGroup.LogDestinations[0]);
            var rabbitMqDestination = (RabbitMqDestination)logGroup.LogDestinations[0];
            // Message publisher asserts.
            Assert.IsInstanceOf<BasicMessagePublisher>(rabbitMqDestination.MessagePublisher);
            var messagePublisher = (BasicMessagePublisher)rabbitMqDestination.MessagePublisher;
            Assert.AreEqual(null, messagePublisher.BasicProperties);
        }
        
        [Test]
        public void LoggerBuilder_With_One_LogGroup_Containing_One_RabbitMq_Destination_In_It()
        {
            var connection = _rabbitMqTestManager.Connection;
            string exchangeName = "exchangeName";
            string exchangeType = "exchangeType";
            string routingKey = "routingKey";
             
            // Act 
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                                    .RabbitMq()
                                                                                    .WithConnection(connection)
                                                                                    .WithAddress(exchangeName, exchangeType, routingKey)
                                                                                    .WithSimpleTemplateLogSerializer() 
                                                                                    .Add()
                                                                                    .BuildLogger();
            // Assert
            var logGroup = (LogGroup)logger.GetLogGroup("group1");
            Assert.AreEqual(1, logGroup.LogDestinations.Count);
            Assert.IsInstanceOf<RabbitMqDestination>(logGroup.LogDestinations[0]);
            var rabbitMqDestination = (RabbitMqDestination)logGroup.LogDestinations[0];
            // Connection asserts.
            Assert.AreEqual(connection, rabbitMqDestination.RabbitMqConnection);
            // Publication address provider asserts.
            Assert.IsInstanceOf<DirectPublicationAddressProvider>(rabbitMqDestination.PublicationAddressProvider);
            var publicationAddressProvider = (DirectPublicationAddressProvider)rabbitMqDestination.PublicationAddressProvider;
            Assert.AreEqual(exchangeName, publicationAddressProvider.PublicationAddress.ExchangeName);
            Assert.AreEqual(exchangeType, publicationAddressProvider.PublicationAddress.ExchangeType);
            Assert.AreEqual(routingKey, publicationAddressProvider.PublicationAddress.RoutingKey);
            // Log serializer asserts.
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(rabbitMqDestination.LogSerializer);
            var logSerializer = (SimpleTemplateLogSerializer)rabbitMqDestination.LogSerializer;
            Assert.AreEqual(SimpleTemplateLogSerializer.DefaultTemplate, logSerializer.Template);
            // Message publisher asserts.
            Assert.IsInstanceOf<BasicMessagePublisher>(rabbitMqDestination.MessagePublisher);
            var messagePublisher = (BasicMessagePublisher)rabbitMqDestination.MessagePublisher;
            Assert.AreEqual(null, messagePublisher.BasicProperties);
        }
    }
}
