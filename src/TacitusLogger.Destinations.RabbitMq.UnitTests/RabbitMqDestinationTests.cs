using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using System.Text;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq.UnitTests
{
    [TestFixture]
    public class RabbitMqDestinationTests
    {
        private Mock<IConnection> RabbitMqConnectionMockThatReturns(IModel model)
        {
            var rabbitMqConnectionMock = new Mock<IConnection>();
            rabbitMqConnectionMock.Setup(x => x.CreateModel()).Returns(model);
            return rabbitMqConnectionMock;
        }
        private Mock<IPublicationAddressProvider> PublicationAddressProviderMockThatTakesAndReturns(LogModel logModel, PublicationAddress publicationAddress)
        {
            var publicationAddressProviderMock = new Mock<IPublicationAddressProvider>();
            publicationAddressProviderMock.Setup(x => x.GetPublicationAddress(logModel)).Returns(publicationAddress);
            return publicationAddressProviderMock;
        }
        private Mock<IPublicationAddressProvider> PublicationAddressProviderMockThatReturns(Func<LogModel, PublicationAddress> func)
        {
            var publicationAddressProviderMock = new Mock<IPublicationAddressProvider>();
            publicationAddressProviderMock.Setup(x => x.GetPublicationAddress(It.IsNotNull<LogModel>())).Returns(func);
            return publicationAddressProviderMock;
        }
        private Mock<ILogSerializer> LogSerializerMockThatTakesAndReturns(LogModel logModel, string resultingString)
        {
            var logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.Setup(x => x.Serialize(logModel)).Returns(resultingString);
            return logSerializerMock;
        }
        private Mock<ILogSerializer> LogSerializerMockThatReturns(Func<LogModel, string> func)
        {
            var logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.Setup(x => x.Serialize(It.IsNotNull<LogModel>())).Returns(func);
            return logSerializerMock;
        }

        #region Ctor tests

        [Test]
        public void Ctor_Taking_RabbitMqConnection_PublicationAddressProvider_LogSerializer_MessagePublisher_When_Called_Sets_All_Dependencies_Correctly()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            //Act
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, publicationAddressProvider, logSerializer, messagePublisher);

            //Assert
            Assert.AreEqual(rabbitMqConnection, rabbitMqDestination.RabbitMqConnection);
            Assert.AreEqual(publicationAddressProvider, rabbitMqDestination.PublicationAddressProvider);
            Assert.AreEqual(logSerializer, rabbitMqDestination.LogSerializer);
            Assert.AreEqual(messagePublisher, rabbitMqDestination.MessagePublisher);
        }

        [Test]
        public void Ctor_Taking_RabbitMqConnection_PublicationAddressProvider_LogSerializer_MessagePublisher_When_Called_With_Null_RabbitMqConnection_Throws_ArgumentNullException()
        {
            //Arrange 
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(null as IConnection, publicationAddressProvider, logSerializer, messagePublisher);
            });
        }

        [Test]
        public void Ctor_Taking_RabbitMqConnection_PublicationAddressProvider_LogSerializer_MessagePublisher_When_Called_With_Null_Publication_Address_Provider_Throws_ArgumentNullException()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, null as IPublicationAddressProvider, logSerializer, messagePublisher);
            });
        }

        [Test]
        public void Ctor_Taking_RabbitMqConnection_PublicationAddressProvider_LogSerializer_MessagePublisher_When_Called_With_Null_Log_Data_Serializer_Throws_ArgumentNullException()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, publicationAddressProvider, null as ILogSerializer, messagePublisher);
            });
        }

        [Test]
        public void Ctor_Taking_RabbitMqConnection_PublicationAddressProvider_LogSerializer_MessagePublisher_When_Called_With_Null_Message_Publisher_Throws_ArgumentNullException()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, publicationAddressProvider, logSerializer, null as IMessagePublisher);
            });
        }

        [Test]
        public void Ctor_Taking_RabbitMqConnection_PublicationAddressProvider_LogSerializer_When_Called_Sets_BasicMessagePublisher_As_Default()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;

            //Act
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, publicationAddressProvider, logSerializer);

            //Assert
            Assert.AreEqual(rabbitMqConnection, rabbitMqDestination.RabbitMqConnection);
            Assert.AreEqual(publicationAddressProvider, rabbitMqDestination.PublicationAddressProvider);
            Assert.AreEqual(logSerializer, rabbitMqDestination.LogSerializer);
            Assert.IsInstanceOf<BasicMessagePublisher>(rabbitMqDestination.MessagePublisher);
        }

        [Test]
        public void Ctor_Taking_RabbitMqConnection_PublicationAddressProvider_When_Called_Sets_JsonLogSerializer_And_BasicMessagePublisher_As_Defaults()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;


            //Act
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, publicationAddressProvider);

            //Assert
            Assert.AreEqual(rabbitMqConnection, rabbitMqDestination.RabbitMqConnection);
            Assert.AreEqual(publicationAddressProvider, rabbitMqDestination.PublicationAddressProvider);
            Assert.IsInstanceOf<JsonLogSerializer>(rabbitMqDestination.LogSerializer);
            Assert.IsInstanceOf<BasicMessagePublisher>(rabbitMqDestination.MessagePublisher);
        }

        [Test]
        public void Ctor_RabbitMqConnection_LogSerializer_Exchange_Info_When_Called_Sets_DirectPublicationAddressProvider_And_BasicMessagePublisher_As_Defaults()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;

            //Act
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, logSerializer, "exchangeName", "exchangeType", "routingKey");

            //Assert
            Assert.AreEqual(rabbitMqConnection, rabbitMqDestination.RabbitMqConnection);
            Assert.AreEqual(logSerializer, rabbitMqDestination.LogSerializer);
            Assert.IsInstanceOf<BasicMessagePublisher>(rabbitMqDestination.MessagePublisher);
            Assert.IsInstanceOf<DirectPublicationAddressProvider>(rabbitMqDestination.PublicationAddressProvider);
            Assert.AreEqual("exchangeName", (rabbitMqDestination.PublicationAddressProvider as DirectPublicationAddressProvider).PublicationAddress.ExchangeName);
            Assert.AreEqual("exchangeType", (rabbitMqDestination.PublicationAddressProvider as DirectPublicationAddressProvider).PublicationAddress.ExchangeType);
            Assert.AreEqual("routingKey", (rabbitMqDestination.PublicationAddressProvider as DirectPublicationAddressProvider).PublicationAddress.RoutingKey);
        }

        [Test]
        public void Ctor_RabbitMqConnection_LogSerializer_Exchange_Info_When_Called_Without_Routing_Key_Sets_Empty_String_As_Default_Routing_Key()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;
            var logSerializer = new Mock<ILogSerializer>().Object;

            //Act
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, logSerializer, "exchangeName", "exchangeType");

            //Assert  
            Assert.AreEqual("", (rabbitMqDestination.PublicationAddressProvider as DirectPublicationAddressProvider).PublicationAddress.RoutingKey);
        }

        [Test]
        public void Ctor_Taking_RabbitMqConnection_Exchange_Info_When_Called_Sets_DirectPublicationAddressProvider_And_BasicMessagePublisher_As_Defaults()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;

            //Act
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, "exchangeName", "exchangeType", "routingKey");

            //Assert
            Assert.AreEqual(rabbitMqConnection, rabbitMqDestination.RabbitMqConnection);
            Assert.IsInstanceOf<JsonLogSerializer>(rabbitMqDestination.LogSerializer);
            Assert.IsInstanceOf<BasicMessagePublisher>(rabbitMqDestination.MessagePublisher);
            Assert.IsInstanceOf<DirectPublicationAddressProvider>(rabbitMqDestination.PublicationAddressProvider);
            Assert.AreEqual("exchangeName", (rabbitMqDestination.PublicationAddressProvider as DirectPublicationAddressProvider).PublicationAddress.ExchangeName);
            Assert.AreEqual("exchangeType", (rabbitMqDestination.PublicationAddressProvider as DirectPublicationAddressProvider).PublicationAddress.ExchangeType);
            Assert.AreEqual("routingKey", (rabbitMqDestination.PublicationAddressProvider as DirectPublicationAddressProvider).PublicationAddress.RoutingKey);
        }

        [Test]
        public void Ctor_Taking_RabbitMqConnection_Exchange_Info_When_Called_Without_Routing_Key_Sets_Empty_String_As_Default_Routing_Key()
        {
            //Arrange
            var rabbitMqConnection = new Mock<IConnection>().Object;

            //Act
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnection, "exchangeName", "exchangeType");

            //Assert 
            Assert.AreEqual("", (rabbitMqDestination.PublicationAddressProvider as DirectPublicationAddressProvider).PublicationAddress.RoutingKey);
        }

        #endregion

        #region Tests for Send methods

        [Test]
        public void Send_When_Called_With_One_LogModel_Sends_All_Nessessary_Data_To_Publish_Method_Of_Message_Publisher()
        {
            //Arrange           
            LogModel logModel = new LogModel();
            // RabbitMQ connection mock. 
            var modelMock = new Mock<IModel>();
            var rabbitMqConnectionMock = RabbitMqConnectionMockThatReturns(modelMock.Object);
            // Publication address provider mock.
            PublicationAddress publicationAddress = new PublicationAddress("fanout", "exchangeName", "routingKey");
            var publicationAddressProviderMock = PublicationAddressProviderMockThatTakesAndReturns(logModel, publicationAddress);
            // Log data serializer mock.
            var logSerializerMock = LogSerializerMockThatTakesAndReturns(logModel, "serialized string");
            byte[] expectedMessageBytes = Encoding.UTF8.GetBytes("serialized string");
            // Message publisher mock.
            var messagePublisherMock = new Mock<IMessagePublisher>();
            // Creating the destination using all above.
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnectionMock.Object, publicationAddressProviderMock.Object, logSerializerMock.Object, messagePublisherMock.Object);

            //Act
            rabbitMqDestination.Send(new LogModel[] { logModel });

            //Assert
            messagePublisherMock.Verify(x => x.Publish(modelMock.Object, publicationAddress, expectedMessageBytes), Times.Once);
        }

        [Test]
        public void Send_When_Called_With_Several_LogModel_In_Array_Sends_All_Nessessary_Data_To_Publish_Method_Of_Message_Publisher()
        {
            // Arrange          
            LogModel[] logs = new LogModel[5];
            for (int i = 0; i < logs.Length; i++)
                logs[i] = new LogModel() { Description = $"Log {i}" };

            // RabbitMQ connection mock. 
            var modelMock = new Mock<IModel>();
            var rabbitMqConnectionMock = RabbitMqConnectionMockThatReturns(modelMock.Object);
            // Publication address provider mock. 
            var publicationAddressProviderMock = PublicationAddressProviderMockThatReturns((LogModel l) => new PublicationAddress("fanout", "exchangeName", l.Description));
            // Log data serializer mock.
            var logSerializerMock = LogSerializerMockThatReturns((LogModel ld) => ld.Description);
            // Message publisher mock.
            var messagePublisherMock = new Mock<IMessagePublisher>();
            // Creating the destination using all above.
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnectionMock.Object, publicationAddressProviderMock.Object, logSerializerMock.Object, messagePublisherMock.Object);

            //Act
            rabbitMqDestination.Send(logs);

            //Assert
            messagePublisherMock.Verify(x => x.Publish(modelMock.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Exactly(5));
            for (int i = 0; i < logs.Length; i++)
                messagePublisherMock.Verify(x => x.Publish(modelMock.Object, It.Is<PublicationAddress>(p => p.RoutingKey == logs[i].Description), It.Is<byte[]>(b => Encoding.UTF8.GetString(b) == logs[i].Description)), Times.Once);
        }

        [Test]
        public void Send_While_Model_Is_Not_Closed_The_Same_Model_Is_Used_And_New_One_Is_Not_Requested_From_Connection()
        {
            // Arrange          
            LogModel[] logs = new LogModel[5];
            for (int i = 0; i < logs.Length; i++)
                logs[i] = new LogModel() { };

            // RabbitMQ connection mock. 
            var modelMock1 = new Mock<IModel>();
            var modelMock2 = new Mock<IModel>();
            var modelMock3 = new Mock<IModel>();
            var modelMock4 = new Mock<IModel>();
            var modelMock5 = new Mock<IModel>();
            var rabbitMqConnectionMock = new Mock<IConnection>();
            // The connection mock will return new model on every call (up to 5 times).
            rabbitMqConnectionMock.SetupSequence(x => x.CreateModel()).Returns(modelMock1.Object)
                                                                      .Returns(modelMock2.Object)
                                                                      .Returns(modelMock3.Object)
                                                                      .Returns(modelMock4.Object)
                                                                      .Returns(modelMock5.Object);
            // Publication address provider mock.
            var publicationAddressProviderMock = PublicationAddressProviderMockThatReturns(lg => new PublicationAddress("fanout", "exchangeName", "routingKey"));
            // Log data serializer mock.
            var logSerializerMock = LogSerializerMockThatReturns(lg => "Log text");
            // Message publisher mock.
            var messagePublisherMock = new Mock<IMessagePublisher>();
            // Creating the destination using all above.
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnectionMock.Object, publicationAddressProviderMock.Object, logSerializerMock.Object, messagePublisherMock.Object);

            // Act
            rabbitMqDestination.Send(logs);

            // Assert
            // First model was used for all logs in the array (5 times).
            messagePublisherMock.Verify(x => x.Publish(modelMock1.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Exactly(5));
            // Remaining models were never requested from the connection.
            messagePublisherMock.Verify(x => x.Publish(modelMock2.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Never);
            messagePublisherMock.Verify(x => x.Publish(modelMock3.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Never);
            messagePublisherMock.Verify(x => x.Publish(modelMock4.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Never);
            messagePublisherMock.Verify(x => x.Publish(modelMock5.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        public void Send_When_Model_From_The_Previous_Iteration_Is_In_Closed_State_New_Model_Is_Requested_From_Connection_And_Used()
        {
            // Arrange          
            LogModel[] logs = new LogModel[5];
            for (int i = 0; i < logs.Length; i++)
                logs[i] = new LogModel() { };

            // RabbitMQ connection mock. 
            var modelMock1 = new Mock<IModel>();
            modelMock1.SetupSequence(x => x.IsClosed).Returns(false)
                                                     .Returns(true);
            var modelMock2 = new Mock<IModel>();
            modelMock1.SetupSequence(x => x.IsClosed).Returns(false)
                                                     .Returns(true);
            var modelMock3 = new Mock<IModel>();
            var modelMock4 = new Mock<IModel>();
            var modelMock5 = new Mock<IModel>();
            var rabbitMqConnectionMock = new Mock<IConnection>();
            // The connection mock will return new model on every call (up to 5 times).
            rabbitMqConnectionMock.SetupSequence(x => x.CreateModel()).Returns(modelMock1.Object)
                                                                      .Returns(modelMock2.Object)
                                                                      .Returns(modelMock3.Object)
                                                                      .Returns(modelMock4.Object)
                                                                      .Returns(modelMock5.Object);
            // Publication address provider mock.
            var publicationAddressProviderMock = PublicationAddressProviderMockThatReturns(lg => new PublicationAddress("fanout", "exchangeName", "routingKey"));
            // Log data serializer mock.
            var logSerializerMock = LogSerializerMockThatReturns(lg => "Log text");
            // Message publisher mock.
            var messagePublisherMock = new Mock<IMessagePublisher>();
            // Creating the destination using all above.
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnectionMock.Object, publicationAddressProviderMock.Object, logSerializerMock.Object, messagePublisherMock.Object);

            // Act
            rabbitMqDestination.Send(logs);

            // Assert
            // First and second models were used each once. 
            messagePublisherMock.Verify(x => x.Publish(modelMock1.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Exactly(2));
            // Remaining logs were published by the third model.
            messagePublisherMock.Verify(x => x.Publish(modelMock2.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Exactly(3));
            // Remaining models were never requested from the connection.
            messagePublisherMock.Verify(x => x.Publish(modelMock3.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Never);
            messagePublisherMock.Verify(x => x.Publish(modelMock4.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Never);
            messagePublisherMock.Verify(x => x.Publish(modelMock5.Object, It.IsAny<PublicationAddress>(), It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        public void Send_When_LogSerializer_Returns_Null_String_Send_Empty_String_Bytes_To_MessagePublisher()
        {
            // Arrange       
            // RabbitMQ connection mock.
            var rabbitMqConnectionMock = new Mock<IConnection>();
            // Publication address provider mock.
            var publicationAddressProviderMock = new Mock<IPublicationAddressProvider>();
            // Log data serializer mock.
            var logSerializerMock = LogSerializerMockThatReturns((LogModel ld) => null);
            // Message publisher mock.
            var messagePublisherMock = new Mock<IMessagePublisher>();
            // Creating the destination using all above.
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(rabbitMqConnectionMock.Object, publicationAddressProviderMock.Object, logSerializerMock.Object, messagePublisherMock.Object);

            //Act
            rabbitMqDestination.Send(new LogModel[] { new LogModel() });

            //Assert
            messagePublisherMock.Verify(x => x.Publish(It.IsAny<IModel>(), It.IsAny<PublicationAddress>(), It.Is<byte[]>(b => b.Length == 0)), Times.Once);
        }

        #endregion
    }
}
