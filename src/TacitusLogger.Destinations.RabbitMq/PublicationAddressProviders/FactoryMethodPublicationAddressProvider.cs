using System; 
using RabbitMQ.Client;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Publication address provider that uses factory method of type <c>TacitusLogger.LogModelFunc<PublicationAddress></c>
    /// to get RabbitMQ publication addresses.
    /// </summary>
    public class FactoryMethodPublicationAddressProvider : IPublicationAddressProvider
    {
        private readonly LogModelFunc<PublicationAddress> _factoryMethod;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.PublicationAddressProviders.FactoryMethodPublicationAddressProvider</c>
        /// using the factory method of type <c>TacitusLogger.LogModelFunc<PublicationAddress></c>.
        /// </summary>
        /// <param name="factoryMethod">The factory method.</param>
        public FactoryMethodPublicationAddressProvider(LogModelFunc<PublicationAddress> factoryMethod)
        {
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException("factoryMethod");
        }

        /// <summary>
        /// The factory method that was specified during the initialization.
        /// </summary>
        public LogModelFunc<PublicationAddress> FactoryMethod => _factoryMethod;

        /// <summary>
        /// Gets the RabbitMQ publication address containing exchange name, exchange type and the routing key using the log data model.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <returns>The RabbitMQ publication address.</returns>
        public PublicationAddress GetPublicationAddress(LogModel logModel)
        {
            return _factoryMethod(logModel);
        }
    }
}
