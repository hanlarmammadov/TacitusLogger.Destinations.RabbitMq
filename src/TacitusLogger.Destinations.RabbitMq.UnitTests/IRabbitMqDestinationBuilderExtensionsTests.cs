using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using TacitusLogger.Serializers;

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

        #region Tests for WithLogSerializer method overloads

        public void WithLogSerializer_Taking_Json_Serializer_Settings_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_JsonLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            IRabbitMqDestinationBuilderExtensions.WithJsonLogSerializer(rabbitMqDestinationBuilderMock.Object, jsonSerializerSettings);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<JsonLogSerializer>(s => s.JsonSerializerSettings == jsonSerializerSettings && s.Converter == JsonLogSerializer.DefaultConverter)), Times.Once);
        }

        public void WithJsonLogSerializer_Taking_Json_Serializer_Settings_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithJsonLogSerializer(rabbitMqDestinationBuilderMock.Object, jsonSerializerSettings);

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        public void WithLogSerializer_Without_Params_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_JsonLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            IRabbitMqDestinationBuilderExtensions.WithJsonLogSerializer(rabbitMqDestinationBuilderMock.Object);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<JsonLogSerializer>(s => s.JsonSerializerSettings == JsonLogSerializer.DefaultJsonSerializerSettings && s.Converter == JsonLogSerializer.DefaultConverter)), Times.Once);
        }

        public void WithJsonLogSerializer_Without_Params_Settings_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithJsonLogSerializer(rabbitMqDestinationBuilderMock.Object);

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        public void WithLogSerializer_Taking_Converter_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_JsonLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            LogModelFunc<Object> converter = d => null;

            // Act
            IRabbitMqDestinationBuilderExtensions.WithJsonLogSerializer(rabbitMqDestinationBuilderMock.Object, converter);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<JsonLogSerializer>(s => s.JsonSerializerSettings == JsonLogSerializer.DefaultJsonSerializerSettings && s.Converter == converter)), Times.Once);
        }

        public void WithJsonLogSerializer_Taking_Converter_Settings_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);
            LogModelFunc<Object> converter = d => null;

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithJsonLogSerializer(rabbitMqDestinationBuilderMock.Object, converter);

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        public void WithLogSerializer_Taking_Converter_And_Json_Serializer_Settings_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_JsonLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            LogModelFunc<Object> converter = d => null;
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            IRabbitMqDestinationBuilderExtensions.WithJsonLogSerializer(rabbitMqDestinationBuilderMock.Object, converter, jsonSerializerSettings);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<JsonLogSerializer>(s => s.JsonSerializerSettings == jsonSerializerSettings && s.Converter == converter)), Times.Once);
        }

        public void WithJsonLogSerializer_Taking_Converter_And_Json_Serializer_Settings_Settings_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithJsonLogSerializer(rabbitMqDestinationBuilderMock.Object, d => null, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        #endregion

        #region Tests for WithSimpleTemplateLogSerializer method overloads

        public void WithSimpleTemplateLogSerializer_Taking_Template_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_SimpleTemplateLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            string template = "template";

            // Act
            IRabbitMqDestinationBuilderExtensions.WithSimpleTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object, template);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<SimpleTemplateLogSerializer>(s => s.Template == template)), Times.Once);
        }

        public void WithSimpleTemplateLogSerializer_Taking_Template_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithSimpleTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object, "template");

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        public void WithSimpleTemplateLogSerializer_Taking_No_Params_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_SimpleTemplateLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();

            // Act
            IRabbitMqDestinationBuilderExtensions.WithSimpleTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<SimpleTemplateLogSerializer>(s => s.Template == SimpleTemplateLogSerializer.DefaultTemplate)), Times.Once);
        }

        public void WithSimpleTemplateLogSerializer_Taking_No_Params_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithSimpleTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object);

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        #endregion

        #region Tests for WithExtendedTemplateLogSerializer method overloads


        public void WithExtendedTemplateLogSerializer_Taking_Template_And_Json_Serializer_Settings_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_ExtendedTemplateLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            string template = "template";
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            IRabbitMqDestinationBuilderExtensions.WithExtendedTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object, template, jsonSerializerSettings);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<ExtendedTemplateLogSerializer>(s => s.Template == template && s.JsonSerializerSettings == jsonSerializerSettings)), Times.Once);
        }

        public void WithExtendedTemplateLogSerializer_Taking_Template_And_Json_Serializer_Settings_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithExtendedTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object, "template", new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        public void WithExtendedTemplateLogSerializer_Json_Serializer_Settings_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_ExtendedTemplateLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            IRabbitMqDestinationBuilderExtensions.WithExtendedTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object, jsonSerializerSettings);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<ExtendedTemplateLogSerializer>(s => s.Template == ExtendedTemplateLogSerializer.DefaultTemplate && s.JsonSerializerSettings == jsonSerializerSettings)), Times.Once);
        }

        public void WithExtendedTemplateLogSerializer_Taking_Json_Serializer_Settings_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithExtendedTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        public void WithExtendedTemplateLogSerializer_Taking_Template_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_ExtendedTemplateLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            string template = "template";

            // Act
            IRabbitMqDestinationBuilderExtensions.WithExtendedTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object, template);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<ExtendedTemplateLogSerializer>(s => s.Template == template)), Times.Once);
        }

        public void WithExtendedTemplateLogSerializer_Taking_Template_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithExtendedTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object, "template");

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        public void WithExtendedTemplateLogSerializer_Taking_No_Params_When_Called_Calls_WithLogSerializer_Method_Passing_New_Created_ExtendedTemplateLogSerializer_To_It()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();

            // Act
            IRabbitMqDestinationBuilderExtensions.WithExtendedTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object);

            // Assert
            rabbitMqDestinationBuilderMock.Verify(x => x.WithLogSerializer(It.Is<ExtendedTemplateLogSerializer>(s => s.Template == ExtendedTemplateLogSerializer.DefaultTemplate)), Times.Once);
        }

        public void WithExtendedTemplateLogSerializer_Taking_No_Params_When_Called_Returns_Result_Of_WithLogSerializer_Method()
        {
            // Arrange
            var rabbitMqDestinationBuilderMock = new Mock<IRabbitMqDestinationBuilder>();
            rabbitMqDestinationBuilderMock.Setup(x => x.WithLogSerializer(It.IsAny<ILogSerializer>())).Returns(rabbitMqDestinationBuilderMock.Object);

            // Act
            var rabbitMqDestinationBuilderReturned = IRabbitMqDestinationBuilderExtensions.WithExtendedTemplateLogSerializer(rabbitMqDestinationBuilderMock.Object);

            // Assert
            Assert.AreEqual(rabbitMqDestinationBuilderReturned, rabbitMqDestinationBuilderMock.Object);
        }

        #endregion
    }
}
