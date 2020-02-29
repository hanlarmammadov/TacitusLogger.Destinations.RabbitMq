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
    }
}
