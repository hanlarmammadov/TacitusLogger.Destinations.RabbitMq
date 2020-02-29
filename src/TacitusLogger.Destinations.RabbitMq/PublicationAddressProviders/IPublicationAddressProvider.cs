using RabbitMQ.Client; 

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Used to provider with RabbitMQ publication address using log data model object.
    /// </summary>
    public interface IPublicationAddressProvider
    {
        /// <summary>
        /// Gets the RabbitMQ publication address containing exchange name, exchange type and the routing key using 
        /// log data model object.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <returns>The RabbitMQ publication address.</returns>
        PublicationAddress GetPublicationAddress(LogModel logModel); 
    }
}
