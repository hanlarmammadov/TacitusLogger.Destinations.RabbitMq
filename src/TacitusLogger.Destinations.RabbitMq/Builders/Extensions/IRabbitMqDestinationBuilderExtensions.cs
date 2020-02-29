using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Adds extension methods to <c>TacitusLogger.Builders.IRabbitMqDestinationBuilder</c> interface and its implementations.
    /// </summary>
    public static class IRabbitMqDestinationBuilderExtensions
    {
        #region Extension methods related to WithPublisher method

        /// <summary>
        /// Add the message publisher of type <c>TacitusLogger.Destinations.RabbitMq.BasicMessagePublisher</c> with specified properties of type
        /// <c>RabbitMQ.Client.IBasicProperties</c>. 
        /// </summary>
        /// <param name="basicProperties">Properties for RabbitMQ basic publish.</param>
        /// <returns></returns>
        public static IRabbitMqDestinationBuilder WithPublisher(this IRabbitMqDestinationBuilder self, IBasicProperties basicProperties = null)
        {
            return self.WithPublisher(new BasicMessagePublisher(basicProperties));
        }

        #endregion

        #region Extension methods related to WithAddress method

        /// <summary>
        /// Adds the publication address provider of type <c>TacitusLogger.Destinations.RabbitMq.DirectPublicationAddressProvider</c> using 
        /// exchange name, type and routing key.
        /// </summary>
        /// <param name="exchangeName">The exchange name.</param>
        /// <param name="exchangeType">The exchange type.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithAddress(this IRabbitMqDestinationBuilder self, string exchangeName, string exchangeType, string routingKey = "")
        {
            return self.WithAddress(new DirectPublicationAddressProvider(exchangeName, exchangeType, routingKey));
        }
        /// <summary>
        /// Adds the publication address provider of type <c>TacitusLogger.Destinations.RabbitMq.FactoryMethodPublicationAddressProvider</c> with the 
        /// specified factory method of type <c>TacitusLogger.LogModelFunc<PublicationAddress></c>.
        /// </summary>
        /// <param name="factoryMethod">The factory method.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithAddress(this IRabbitMqDestinationBuilder self, LogModelFunc<PublicationAddress> factoryMethod)
        {
            return self.WithAddress(new FactoryMethodPublicationAddressProvider(factoryMethod));
        }

        #endregion

        #region Extension methods related to WithLogSerializer method

        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.JsonLogSerializer</c> using json serializer settings of type
        /// <c>Newtonsoft.Json.JsonSerializerSettings</c>.
        /// </summary>
        /// <param name="jsonSerializerSettings">Json serializer settings.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithJsonLogSerializer(this IRabbitMqDestinationBuilder self, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithLogSerializer(new JsonLogSerializer(jsonSerializerSettings));
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.JsonLogSerializer</c> using default json serializer settings.
        /// </summary>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithJsonLogSerializer(this IRabbitMqDestinationBuilder self)
        {
            return self.WithLogSerializer(new JsonLogSerializer());
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.LogModelToCustomTypeJsonSerializer</c> using converter delegate of type 
        /// <c>TacitusLoggerLogModelFunc<Object></c> and default json serializer settings. 
        /// </summary>
        /// <param name="converter">The converter delegate. It is used to create custom object from log data model that will then JSON serialized.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithJsonLogSerializer(this IRabbitMqDestinationBuilder self, LogModelFunc<Object> converter)
        {
            return self.WithLogSerializer(new JsonLogSerializer(converter));
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.LogModelToCustomTypeJsonSerializer</c> using converter delegate of type 
        /// <c>TacitusLogger.LogModelFunc<Object></c> and custom json serializer settings <c>Newtonsoft.Json.JsonSerializerSettings</c>.
        /// </summary>
        /// <param name="converter">The converter delegate. It is used to create custom object from log data model that will then JSON serialized.</param>
        /// <param name="jsonSerializerSettings">Json serializer settings.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithJsonLogSerializer(this IRabbitMqDestinationBuilder self, LogModelFunc<Object> converter, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithLogSerializer(new JsonLogSerializer(converter, jsonSerializerSettings));
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.SimpleTemplateLogSerializer</c> using specified template string.
        /// </summary>
        /// <param name="template">Log serializer template string.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithSimpleTemplateLogSerializer(this IRabbitMqDestinationBuilder self, string template)
        {
            return self.WithLogSerializer(new SimpleTemplateLogSerializer(template));
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.SimpleTemplateLogSerializer</c> using default template string.
        /// </summary>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithSimpleTemplateLogSerializer(this IRabbitMqDestinationBuilder self)
        {
            return self.WithLogSerializer(new SimpleTemplateLogSerializer());
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> using custom template string and custom
        /// json serializer settings of type <c>Newtonsoft.Json.JsonSerializerSettings</c>.
        /// </summary>
        /// <param name="template">Log serializer template string.</param>
        /// <param name="jsonSerializerSettings">Json serializer settings.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithExtendedTemplateLogSerializer(this IRabbitMqDestinationBuilder self, string template, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithLogSerializer(new ExtendedTemplateLogSerializer(template, jsonSerializerSettings));
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> using custom template string and default
        /// json serializer settings.
        /// </summary>
        /// <param name="template">Log serializer template string.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithExtendedTemplateLogSerializer(this IRabbitMqDestinationBuilder self, string template)
        {
            return self.WithLogSerializer(new ExtendedTemplateLogSerializer(template));
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> using default template string and custom
        /// json serializer settings of type <c>Newtonsoft.Json.JsonSerializerSettings</c>.
        /// </summary>
        /// <param name="jsonSerializerSettings">Json serializer settings.</param>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithExtendedTemplateLogSerializer(this IRabbitMqDestinationBuilder self, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithLogSerializer(new ExtendedTemplateLogSerializer(jsonSerializerSettings));
        }
        /// <summary>
        /// Adds the log serializer of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> using default template string and 
        /// json serializer settings.
        /// </summary>
        /// <returns>Self.</returns>
        public static IRabbitMqDestinationBuilder WithExtendedTemplateLogSerializer(this IRabbitMqDestinationBuilder self)
        {
            return self.WithLogSerializer(new ExtendedTemplateLogSerializer());
        }
        
        #endregion
    }
}
