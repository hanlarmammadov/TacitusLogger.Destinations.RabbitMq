using RabbitMQ.Client;
using System;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Publishes the RabbitMq message by using the basic publish of RabbitMq channel.
    /// </summary>
    public class BasicMessagePublisher : IMessagePublisher
    {
        private readonly IBasicProperties _basicProperties;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.Publishers.BasicMessagePublisher</c>
        /// using optional basic properties of type <c>RabbitMQ.Client.IBasicProperties</c>.
        /// </summary>
        /// <param name="basicProperties">Basic properties used to publish.</param>
        public BasicMessagePublisher(IBasicProperties basicProperties = null)
        {
            _basicProperties = basicProperties;
        }

        /// <summary>
        /// Gets the basic properties specified during the initialization.
        /// </summary>
        public IBasicProperties BasicProperties => _basicProperties;

        /// <summary>
        /// Publishes the message using the specified RabbitMQ model and publication address.
        /// </summary>
        /// <param name="model">RabbitMQ model.</param>
        /// <param name="publicationAddress">RabbitMQ publication address containing the exchange name, exchange type and the routing key.</param>
        /// <param name="message">Publishing message bytes.</param>
        public void Publish(IModel model, PublicationAddress publicationAddress, byte[] message)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            model.BasicPublish(publicationAddress ?? throw new ArgumentNullException("publicationAddress"), _basicProperties, message ?? throw new ArgumentNullException("message"));
        }
    }
}
