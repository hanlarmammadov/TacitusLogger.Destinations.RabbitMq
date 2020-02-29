using Moq;
using NUnit.Framework;
using RabbitMQ.Client;

namespace TacitusLogger.Destinations.RabbitMq.UnitTests
{
    [TestFixture]
    public class IRabbitMqDestinationBuilderExtensionsTests
    {
        #region Tests for WithPublisher method overloads

        [Test]
        public void WithPublisher_Taking_Basic_Properties_When_Called_Calls_WithPublisher_Method_Passing_New_Created_BasicMessagePublisher_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            var basicProperties = new Mock<IBasicProperties>().Object;

            // Act
            IRabbitMqDestinationBuilderExtensions.WithPublisher(rabbitMqDestinationBuilderMock.Object, basicProperties);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithPublisher(It.Is<BasicMessagePublisher>(p => p.BasicProperties == basicProperties)), Times.Once);
        }

        [Test]
        public void WithPublisher_Taking_Basic_Properties_When_Called_Without_Basic_Properties_Uses_Null_As_Default()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();

            // Act
            IRabbitMqDestinationBuilderExtensions.WithPublisher(rabbitMqDestinationBuilderMock.Object);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithPublisher(It.Is<BasicMessagePublisher>(p => p.BasicProperties == null)), Times.Once);
        }

        [Test]
        public void WithPublisher_Taking_Basic_Properties_When_Called_Returns_Result_Of_WithPublisher_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithPublisher(It.IsAny<IMessagePublisher>())).Returns(rabbitMqDestinationBuilderMock.Object);
            var basicProperties = new Mock<IBasicProperties>().Object;

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithPublisher(rabbitMqDestinationBuilderMock.Object, basicProperties);

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        #endregion

        #region Tests for WithAddress method overloads

        [Test]
        public void WithAddress_Taking_ExchangeNameTypeAndRoutingKey_When_Called_Calls_WithAddress_Method_Passing_New_Created_DirectPublicationAddressProvider_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            var exchangeName = "exchangeName";
            var exchangeType = "exchangeType";
            var routingKey = "routingKey";

            // Act
            IRabbitMqDestinationBuilderExtensions.WithAddress(rabbitMqDestinationBuilderMock.Object, exchangeName, exchangeType, routingKey);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithAddress(It.Is<DirectPublicationAddressProvider>(p => p.PublicationAddress.ExchangeName == exchangeName &&
                                                                                                                  p.PublicationAddress.ExchangeType == exchangeType &&
                                                                                                                  p.PublicationAddress.RoutingKey == routingKey)), Times.Once);
        }

        [Test]
        public void WithAddress_Taking_Exchange_Name_Type_And_Routing_Key_When_Called_Without_Routing_Key_Uses_Empty_String_As_Default_Routing_Key()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            var exchangeName = "exchangeName";
            var exchangeType = "exchangeType";

            // Act
            IRabbitMqDestinationBuilderExtensions.WithAddress(rabbitMqDestinationBuilderMock.Object, exchangeName, exchangeType);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithAddress(It.Is<DirectPublicationAddressProvider>(p => p.PublicationAddress.ExchangeName == exchangeName &&
                                                                                                                    p.PublicationAddress.ExchangeType == exchangeType &&
                                                                                                                    p.PublicationAddress.RoutingKey == string.Empty)), Times.Once);
        }

        [Test]
        public void WithAddress_Taking_Exchange_Name_Type_And_Routing_Key_When_Called_Returns_Result_Of_WithAddress_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithAddress(It.IsAny<IPublicationAddressProvider>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithAddress(rabbitMqDestinationBuilderMock.Object, "exchangeName", "exchangeType", "routingKey");

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        [Test]
        public void WithAddress_Taking_Factory_Method_When_Called_Calls_WithAddress_Method_Passing_New_Created_FactoryMethodPublicationAddressProvider_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            LogModelFunc<PublicationAddress> factoryMethod = d => null;

            // Act
            IRabbitMqDestinationBuilderExtensions.WithAddress(rabbitMqDestinationBuilderMock.Object, factoryMethod);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithAddress(It.Is<FactoryMethodPublicationAddressProvider>(p => p.FactoryMethod == factoryMethod)), Times.Once);
        }

        [Test]
        public void WithAddress_Taking_Factory_Method_When_Called_Returns_Result_Of_WithAddress_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithAddress(It.IsAny<IPublicationAddressProvider>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithAddress(rabbitMqDestinationBuilderMock.Object, d => null);

            // Assert        
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        #endregion 
    }
}
