using RabbitMQ.Client;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Publishes the RabbitMq message.
    /// </summary>
    public interface IMessagePublisher
    {
        /// <summary>
        /// Publishes the message using the specified RabbitMQ model and publication address.
        /// </summary>
        /// <param name="model">RabbitMQ model.</param>
        /// <param name="publicationAddress">RabbitMQ publication address containing the exchange name, exchange type and the routing key.</param>
        /// <param name="message">Publishing message bytes.</param>
        void Publish(IModel model, PublicationAddress publicationAddress, byte[] message);
    }
}
