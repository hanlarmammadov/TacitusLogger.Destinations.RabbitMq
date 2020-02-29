using NUnit.Framework;
using RabbitMQ.Client;
using System;

namespace TacitusLogger.Destinations.RabbitMq.UnitTests
{
    [TestFixture]
    public class FactoryMethodPublicationAddressProviderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_The_Factory_Method()
        {
            //Arrange
            LogModelFunc<PublicationAddress> factoryMethod = d => null;

            //Act
            FactoryMethodPublicationAddressProvider factoryMethodPublicationAddressProvider = new FactoryMethodPublicationAddressProvider(factoryMethod);

            //Assert
            Assert.AreEqual(factoryMethod, factoryMethodPublicationAddressProvider.FactoryMethod);
        }

        [Test]
        public void Ctor_When_Called_With_Null_Factory_Method_Throws_ArgumentNullException()
        {
            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                //Act
                FactoryMethodPublicationAddressProvider factoryMethodPublicationAddressProvider = new FactoryMethodPublicationAddressProvider(null);
            });
        }

        #endregion

        #region Tests for GetPublicationAddress

        [Test]
        public void GetPublicationAddress_When_Called_Returns_Address_Using_Factory_Method()
        {
            //Arrange
            PublicationAddress publicationAddress = new PublicationAddress("fanout", "name", "rkey");
            LogModelFunc<PublicationAddress> factoryMethod = d => publicationAddress;
            FactoryMethodPublicationAddressProvider factoryMethodPublicationAddressProvider = new FactoryMethodPublicationAddressProvider(factoryMethod);

            //Act
            var publicationAddressReturned = factoryMethodPublicationAddressProvider.GetPublicationAddress(new LogModel());

            //Assert
            Assert.AreEqual(publicationAddress, publicationAddressReturned);
        }

        #endregion
    }
}
