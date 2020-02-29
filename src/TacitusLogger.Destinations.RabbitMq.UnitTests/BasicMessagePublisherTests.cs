using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using System; 

namespace TacitusLogger.Destinations.RabbitMq.UnitTests
{
    [TestFixture]
    public class BasicMessagePublisherTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_BasicProperties()
        {
            //Arrange
            IBasicProperties basicProperties = new Mock<IBasicProperties>().Object;

            //Act
            BasicMessagePublisher basicMessagePublisher = new BasicMessagePublisher(basicProperties);

            //Assert
            Assert.AreEqual(basicProperties, basicMessagePublisher.BasicProperties);
        }

        [Test]
        public void Ctor_When_Called_Without_BasicProperties_Sets_BasicProperties_To_Null()
        {
            //Act
            BasicMessagePublisher basicMessagePublisher = new BasicMessagePublisher();

            //Assert
            Assert.IsNull(basicMessagePublisher.BasicProperties);
        }

        #endregion

        #region Tests for Publish method

        [Test]
        public void Publish_When_Called_Calls_Models_BasicPublish_Method()
        {
            //Arrange
            IBasicProperties basicProperties = new Mock<IBasicProperties>().Object;
            var publicationAddress = new PublicationAddress("exchangeType", "exchangeName", "routingKey");
            byte[] message = new byte[10];
            var modelMock = new Mock<IModel>();
            BasicMessagePublisher basicMessagePublisher = new BasicMessagePublisher(basicProperties);

            //Act
            basicMessagePublisher.Publish(modelMock.Object, publicationAddress, message);

            //Assert
            modelMock.Verify(x => x.BasicPublish("exchangeName", "routingKey", It.IsAny<bool>(), basicProperties, message), Times.Once);
        }

        [Test]
        public void Publish_When_Called_Given_That_BasicProperties_Are_Null_Calls_Models_BasicPublish_Method_With_Null_Basic_Properties()
        {
            //Arrange  
            var modelMock = new Mock<IModel>();
            BasicMessagePublisher basicMessagePublisher = new BasicMessagePublisher(null as IBasicProperties);

            //Act
            basicMessagePublisher.Publish(modelMock.Object, new PublicationAddress("exchangeType", "exchangeName", "routingKey"), new byte[10]);

            //Assert
            modelMock.Verify(x => x.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), null as IBasicProperties, It.IsAny<byte[]>()), Times.Once);
        }

        [Test]
        public void Publish_When_Called_With_Null_Model_Throws_ArgumentNullException()
        {
            //Arrange
            BasicMessagePublisher basicMessagePublisher = new BasicMessagePublisher();

            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                basicMessagePublisher.Publish(null as IModel, new PublicationAddress("exchangeType", "exchangeName", "routingKey"), new byte[10]);
            });
        }

        [Test]
        public void Publish_When_Called_With_Null_PublicationAddress_Throws_ArgumentNullException()
        {
            //Arrange
            BasicMessagePublisher basicMessagePublisher = new BasicMessagePublisher();

            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                basicMessagePublisher.Publish(new Mock<IModel>().Object, null as PublicationAddress, new byte[10]);
            });
        }

        [Test]
        public void Publish_When_Called_With_Null_Message_Throws_ArgumentNullException()
        {
            //Arrange
            BasicMessagePublisher basicMessagePublisher = new BasicMessagePublisher();

            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                basicMessagePublisher.Publish(new Mock<IModel>().Object, new PublicationAddress("exchangeType", "exchangeName", "routingKey"), null as byte[]);
            });
        }

        #endregion
    }
}
