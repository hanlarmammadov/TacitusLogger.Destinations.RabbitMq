using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq.UnitTests
{
    [TestFixture]
    public class RabbitMqDestinationBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_Provided_LogGroupDestinationBuilder()
        {
            // Arrange
            var logGroupDestinationBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(logGroupDestinationBuilder);

            // Assert
            // 
            Assert.AreEqual(logGroupDestinationBuilder, rabbitMqDestinationBuilder.LogGroupDestinationsBuilder);
            //
            Assert.IsNull(rabbitMqDestinationBuilder.RabbitMqConnection);
            Assert.IsNull(rabbitMqDestinationBuilder.MessagePublisher);
            Assert.IsNull(rabbitMqDestinationBuilder.PublicationAddressProvider);
            Assert.IsNull(rabbitMqDestinationBuilder.LogSerializer);
        }

        #endregion

        #region Tests for WithConnection method

        [Test]
        public void WithConnection_When_Called_Sets_Provided_RabbitMq_Connection()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            var rabbitMqConnection = new Mock<IConnection>().Object;

            // Act
            rabbitMqDestinationBuilder.WithConnection(rabbitMqConnection);

            // Assert
            Assert.AreEqual(rabbitMqConnection, rabbitMqDestinationBuilder.RabbitMqConnection);
        }

        [Test]
        public void WithConnection_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            // Already has been set here.
            rabbitMqDestinationBuilder.WithConnection(new Mock<IConnection>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.WithConnection(new Mock<IConnection>().Object);
            });
        }

        [Test]
        public void WithConnection_When_Called_With_Null_Connection_Throws_ArgumentNullException()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.WithConnection(null as IConnection);
            });
        }

        #endregion

        #region Tests for WithPublisher method

        [Test]
        public void WithPublisher_When_Called_Sets_Provided_Message_Publisher()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            var messagePublisher = new Mock<IMessagePublisher>().Object;

            // Act
            rabbitMqDestinationBuilder.WithPublisher(messagePublisher);

            // Assert
            Assert.AreEqual(messagePublisher, rabbitMqDestinationBuilder.MessagePublisher);
        }

        [Test]
        public void WithPublisher_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            // Already has been set here.
            rabbitMqDestinationBuilder.WithPublisher(new Mock<IMessagePublisher>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.WithPublisher(new Mock<IMessagePublisher>().Object);
            });
        }

        [Test]
        public void WithPublisher_When_Called_With_Null_Message_Publisher_Throws_ArgumentNullException()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.WithPublisher(null as IMessagePublisher);
            });
        }

        #endregion

        #region Tests for WithAddress method

        [Test]
        public void WithAddress_When_Called_Sets_Provided_Publication_Address_Provider()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;

            // Act
            rabbitMqDestinationBuilder.WithAddress(publicationAddressProvider);

            // Assert
            Assert.AreEqual(publicationAddressProvider, rabbitMqDestinationBuilder.PublicationAddressProvider);
        }

        [Test]
        public void WithAddress_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            // Already has been set here.
            rabbitMqDestinationBuilder.WithAddress(new Mock<IPublicationAddressProvider>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.WithAddress(new Mock<IPublicationAddressProvider>().Object);
            });
        }

        [Test]
        public void WithAddress_When_Called_With_Null_Publication_Address_Provider_Throws_ArgumentNullException()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.WithAddress(null as IPublicationAddressProvider);
            });
        }

        #endregion

        #region Tests for WithLogSerializer method

        [Test]
        public void WithLogSerializer_When_Called_Sets_Provided_Log_Serializer()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            var logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            rabbitMqDestinationBuilder.WithLogSerializer(logSerializer);

            // Assert
            Assert.AreEqual(logSerializer, rabbitMqDestinationBuilder.LogSerializer);
        }

        [Test]
        public void WithLogSerializer_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            // Already has been set here.
            rabbitMqDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);
            });
        }

        [Test]
        public void WithLogSerializer_When_Called_With_Null_Publication_Address_Provider_Throws_ArgumentNullException()
        {
            // Arrange
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.WithLogSerializer(null as ILogSerializer);
            });
        }

        #endregion

        #region Tests for Add method

        [Test]
        public void Add_When_Called_Creates_RabbitMqDestination_Then_Sends_It_To_CustomDestination_Method()
        {
            // Arrange
            // Create the builder
            var logGroupDestinationBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(logGroupDestinationBuilderMock.Object);
            // Add dependencies
            var connection = new Mock<IConnection>().Object;
            rabbitMqDestinationBuilder.WithConnection(connection);
            var messagePublisher = new Mock<IMessagePublisher>().Object;
            rabbitMqDestinationBuilder.WithPublisher(messagePublisher);
            var publicationAddressProvider = new Mock<IPublicationAddressProvider>().Object;
            rabbitMqDestinationBuilder.WithAddress(publicationAddressProvider);
            var logSerializer = new Mock<ILogSerializer>().Object;
            rabbitMqDestinationBuilder.WithLogSerializer(logSerializer);

            // Act
            rabbitMqDestinationBuilder.Add();

            // Assert
            logGroupDestinationBuilderMock.Verify(x => x.CustomDestination(It.Is<RabbitMqDestination>(d => d.RabbitMqConnection == connection &&
                                                                                                           d.MessagePublisher == messagePublisher &&
                                                                                                           d.PublicationAddressProvider == publicationAddressProvider &&
                                                                                                           d.LogSerializer == logSerializer)));
        }

        [Test]
        public void Add_When_Called_Returns_Result_Of_CustomDestination_Method()
        {
            // Arrange
            // Create and setup the builder.
            var logGroupDestinationBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            logGroupDestinationBuilderMock.Setup(x => x.CustomDestination(It.IsAny<RabbitMqDestination>())).Returns(logGroupDestinationBuilderMock.Object);
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(logGroupDestinationBuilderMock.Object);
            // Add dependencies 
            rabbitMqDestinationBuilder.WithConnection(new Mock<IConnection>().Object);
            rabbitMqDestinationBuilder.WithPublisher(new Mock<IMessagePublisher>().Object);
            rabbitMqDestinationBuilder.WithAddress(new Mock<IPublicationAddressProvider>().Object);
            rabbitMqDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);

            // Act
            ILogGroupDestinationsBuilder resultReturned = rabbitMqDestinationBuilder.Add();

            // Assert
            Assert.AreEqual(logGroupDestinationBuilderMock.Object, resultReturned);
        }

        [Test]
        public void Add_When_Called_Given_That_Connection_Was_Not_Specified_Throws_InvalidOperationException()
        {
            // Arrange
            // Create the builder.
            var logGroupDestinationBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(logGroupDestinationBuilderMock.Object);
            // Add all dependencies but connection.
            rabbitMqDestinationBuilder.WithPublisher(new Mock<IMessagePublisher>().Object);
            rabbitMqDestinationBuilder.WithAddress(new Mock<IPublicationAddressProvider>().Object);
            rabbitMqDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.Add();
            });
        }

        [Test]
        public void Add_When_Called_Given_That_Publication_Address_Provider_Was_Not_Specified_Throws_InvalidOperationException()
        {
            // Arrange
            // Create the builder.
            var logGroupDestinationBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(logGroupDestinationBuilderMock.Object);
            // Add all dependencies but publication address provider.
            rabbitMqDestinationBuilder.WithConnection(new Mock<IConnection>().Object);
            rabbitMqDestinationBuilder.WithPublisher(new Mock<IMessagePublisher>().Object);
            rabbitMqDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                rabbitMqDestinationBuilder.Add();
            });
        }

        [Test]
        public void Add_When_Called_Given_That_Message_Publisher_Was_Not_Specified_Sets_Default_Message_Publisher()
        {
            // Arrange
            // Create the builder.
            var logGroupDestinationBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(logGroupDestinationBuilderMock.Object);
            // Add all dependencies but message publisher.
            rabbitMqDestinationBuilder.WithConnection(new Mock<IConnection>().Object);
            rabbitMqDestinationBuilder.WithAddress(new Mock<IPublicationAddressProvider>().Object);
            rabbitMqDestinationBuilder.WithLogSerializer(new Mock<ILogSerializer>().Object);

            // Act
            rabbitMqDestinationBuilder.Add();

            //Assert
            Assert.IsInstanceOf<BasicMessagePublisher>(rabbitMqDestinationBuilder.MessagePublisher);
        }

        [Test]
        public void Add_When_Called_Given_That_Log_Serializer_Was_Not_Specified_Sets_Default_Log_Serializer()
        {
            // Arrange
            // Create the builder.
            var logGroupDestinationBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            RabbitMqDestinationBuilder rabbitMqDestinationBuilder = new RabbitMqDestinationBuilder(logGroupDestinationBuilderMock.Object);
            // Add all dependencies but message publisher.
            rabbitMqDestinationBuilder.WithConnection(new Mock<IConnection>().Object);
            rabbitMqDestinationBuilder.WithPublisher(new Mock<IMessagePublisher>().Object);
            rabbitMqDestinationBuilder.WithAddress(new Mock<IPublicationAddressProvider>().Object);

            // Act
            rabbitMqDestinationBuilder.Add();

            //Assert
            Assert.IsInstanceOf<JsonLogSerializer>(rabbitMqDestinationBuilder.LogSerializer);
        }

        #endregion
    }
}
