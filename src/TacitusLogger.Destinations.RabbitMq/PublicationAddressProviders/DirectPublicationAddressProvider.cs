using System; 
using RabbitMQ.Client;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Publication address provider that uses the address specified during the initialization.
    /// </summary>
    public class DirectPublicationAddressProvider : IPublicationAddressProvider
    {
        private PublicationAddress _publicationAddress;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.PublicationAddressProviders.DirectPublicationAddressProvider</c>
        /// using the exchange name, type and the routing key.
        /// </summary>
        /// <param name="exchangeName">RabbitMQ exchange name.</param>
        /// <param name="exchangeType">RabbitMQ exchange type.</param>
        /// <param name="routingKey">Routing key.</param>
        public DirectPublicationAddressProvider(string exchangeName, string exchangeType, string routingKey = "")
        {
            if (exchangeName == null)
                throw new ArgumentNullException("exchangeName");
            if (exchangeType == null)
                throw new ArgumentNullException("exchangeType");

            _publicationAddress = new PublicationAddress(exchangeType, exchangeName, routingKey);
        }

        /// <summary>
        /// Publication address from the components specified during the initialization. For testing purposes.
        /// </summary>
        internal PublicationAddress PublicationAddress => _publicationAddress;

        /// <summary>
        /// Gets the RabbitMQ publication address containing exchange name, exchange type and the routing key using data 
        /// specified during the initialization.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <returns>The RabbitMQ publication address.</returns>
        public PublicationAddress GetPublicationAddress(LogModel logModel)
        {
            return _publicationAddress;
        }
    }
}
