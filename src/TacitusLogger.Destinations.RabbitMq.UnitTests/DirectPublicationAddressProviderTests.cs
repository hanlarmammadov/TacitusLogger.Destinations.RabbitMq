using NUnit.Framework;
using System;

namespace TacitusLogger.Destinations.RabbitMq.UnitTests
{
    [TestFixture]
    public class DirectPublicationAddressProviderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_Publication_Address_Using_Provided_Data()
        {
            //Arrange
            string exchangeName = "exchangeName";
            string exchangeType = "fanout";
            string routingKey = "routingKey";

            //Act
            DirectPublicationAddressProvider directPublicationAddressProvider = new DirectPublicationAddressProvider(exchangeName, exchangeType, routingKey);

            //Assert
            Assert.AreEqual(exchangeName, directPublicationAddressProvider.PublicationAddress.ExchangeName);
            Assert.AreEqual(exchangeType, directPublicationAddressProvider.PublicationAddress.ExchangeType);
            Assert.AreEqual(routingKey, directPublicationAddressProvider.PublicationAddress.RoutingKey);
        }

        [Test]
        public void Ctor_When_Called_With_Null_Exchange_Name_Throws_ArgumentNullException()
        {
            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                DirectPublicationAddressProvider directPublicationAddressProvider = new DirectPublicationAddressProvider(null, "fanout", "routingKey");
            });
        }

        [Test]
        public void Ctor_When_Called_With_Null_Exchange_Type_Throws_ArgumentNullException()
        {
            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                DirectPublicationAddressProvider directPublicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", null, "routingKey");
            });
        }

        [Test]
        public void Ctor_When_Called_With_Null_Routing_Key_Sets_Routing_Key_To_Null()
        {
            //Arrange
            string exchangeName = "exchangeName";
            string exchangeType = "fanout";

            //Act
            DirectPublicationAddressProvider directPublicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "fanout", null);

            //Assert
            Assert.AreEqual(exchangeName, directPublicationAddressProvider.PublicationAddress.ExchangeName);
            Assert.AreEqual(exchangeType, directPublicationAddressProvider.PublicationAddress.ExchangeType);
            Assert.IsNull(directPublicationAddressProvider.PublicationAddress.RoutingKey);

        }

        [Test]
        public void Ctor_When_Called_Without_Routing_Key_Sets_Routing_Key_To_Empty_String()
        {
            //Arrange
            string exchangeName = "exchangeName";
            string exchangeType = "fanout";

            //Act
            DirectPublicationAddressProvider directPublicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "fanout");

            //Assert
            Assert.AreEqual(exchangeName, directPublicationAddressProvider.PublicationAddress.ExchangeName);
            Assert.AreEqual(exchangeType, directPublicationAddressProvider.PublicationAddress.ExchangeType);
            Assert.AreEqual("", directPublicationAddressProvider.PublicationAddress.RoutingKey);
        }

        #endregion

        #region Tests for GetPublicationAddress

        [Test]
        public void GetPublicationAddress_When_Called_Returns_Address_That_Was_Created_During_Initialization()
        {
            //Arrange 
            DirectPublicationAddressProvider directPublicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "fanout", "routingKey");

            //Act
            var publicationAddress = directPublicationAddressProvider.GetPublicationAddress(new LogModel());

            //Assert
            Assert.AreEqual(directPublicationAddressProvider.PublicationAddress, publicationAddress);
        }

        [Test]
        public void GetPublicationAddress_When_Called_With_Null_LogModel_Returns_Address_That_Was_Created_During_Initialization()
        {
            //Arrange 
            DirectPublicationAddressProvider directPublicationAddressProvider = new DirectPublicationAddressProvider("exchangeName", "fanout", "routingKey");

            //Act
            var publicationAddress = directPublicationAddressProvider.GetPublicationAddress(null);

            //Assert
            Assert.AreEqual(directPublicationAddressProvider.PublicationAddress, publicationAddress);
        }
        
        #endregion 
    }
}
