using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Destination that sends log information to RabbitMQ exchange.
    /// </summary>
    public class RabbitMqDestination : ILogDestination
    {
        private readonly IConnection _rabbitMqConnection;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IPublicationAddressProvider _publicationAddressProvider;
        private readonly ILogSerializer _logSerializer;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.RabbitMqDestination</c> using all its dependencies.
        /// </summary>
        /// <param name="rabbitMqConnection">RabbitMQ connection that will be used to publish messages.</param>
        /// <param name="messagePublisher">An implementation of <c>TacitusLogger.Destinations.RabbitMq.Publishers.IMessagePublisher</c> that encapsulates the publishing logic.</param>
        /// <param name="publicationAddressProvider">An implementation of <c>TacitusLogger.Destinations.RabbitMq.PublicationAddressProviders.IPublicationAddressProvider</c> that provides the publication address.</param>
        /// <param name="logSerializer">Log model serializer.</param>
        public RabbitMqDestination(IConnection rabbitMqConnection, IPublicationAddressProvider publicationAddressProvider, ILogSerializer logSerializer, IMessagePublisher messagePublisher)
        {
            _rabbitMqConnection = rabbitMqConnection ?? throw new ArgumentNullException("rabbitMqConnection");
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException("messagePublisher");
            _publicationAddressProvider = publicationAddressProvider ?? throw new ArgumentNullException("publicationAddressProvider");
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
        }
        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.RabbitMqDestination</c> using 
        /// <c>TacitusLogger.Destinations.RabbitMq.Publishers.BasicMessagePublisher</c> as a default message publisher.
        /// </summary>
        /// <param name="rabbitMqConnection"></param>
        /// <param name="publicationAddressProvider"></param>
        /// <param name="logSerializer"></param>
        public RabbitMqDestination(IConnection rabbitMqConnection, IPublicationAddressProvider publicationAddressProvider, ILogSerializer logSerializer)
            : this(rabbitMqConnection, publicationAddressProvider, logSerializer, new BasicMessagePublisher())
        {

        }
        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.RabbitMqDestination</c> using 
        /// <c>TacitusLogger.Destinations.RabbitMq.Publishers.BasicMessagePublisher</c> as a default message publisher 
        /// and <c>JsonLogSerializer</c> as a default log data serializer.
        /// </summary>
        /// <param name="rabbitMqConnection"></param>
        /// <param name="publicationAddressProvider"></param>
        public RabbitMqDestination(IConnection rabbitMqConnection, IPublicationAddressProvider publicationAddressProvider)
            : this(rabbitMqConnection, publicationAddressProvider, new JsonLogSerializer(), new BasicMessagePublisher())
        {

        }
        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.RabbitMqDestination</c> using 
        /// <c>TacitusLogger.Destinations.RabbitMq.Publishers.BasicMessagePublisher</c> as a default message publisher 
        /// and <c>TacitusLogger.Destinations.RabbitMq.PublicationAddressProviders.DirectPublicationAddressProvider</c> as a
        /// default publication address provider.
        /// </summary>
        /// <param name="rabbitMqConnection"></param>
        /// <param name="logSerializer"></param>
        /// <param name="exchangeName"></param>
        /// <param name="exchangeType"></param>
        /// <param name="routingKey"></param>
        public RabbitMqDestination(IConnection rabbitMqConnection, ILogSerializer logSerializer, string exchangeName, string exchangeType, string routingKey = "")
            : this(rabbitMqConnection, new DirectPublicationAddressProvider(exchangeName, exchangeType, routingKey), logSerializer, new BasicMessagePublisher())
        {

        }
        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.RabbitMqDestination</c> using 
        /// <c>TacitusLogger.Destinations.RabbitMq.Publishers.BasicMessagePublisher</c> as a default message publisher 
        /// and <c>JsonLogSerializer</c> as a default log data serializer and
        /// <c>TacitusLogger.Destinations.RabbitMq.PublicationAddressProviders.DirectPublicationAddressProvider</c> as a
        /// default publication address provider.
        /// </summary>
        /// <param name="rabbitMqConnection"></param>
        /// <param name="exchangeName"></param>
        /// <param name="exchangeType"></param>
        /// <param name="routingKey"></param>
        public RabbitMqDestination(IConnection rabbitMqConnection, string exchangeName, string exchangeType, string routingKey = "")
            : this(rabbitMqConnection, new DirectPublicationAddressProvider(exchangeName, exchangeType, routingKey), new JsonLogSerializer(), new BasicMessagePublisher())
        {

        }

        /// <summary>
        /// Gets the RabbitMq connection that was specified during initialization.
        /// </summary>
        public IConnection RabbitMqConnection => _rabbitMqConnection;
        /// <summary>
        /// Gets the message provider that was specified during initialization.
        /// </summary>
        public IMessagePublisher MessagePublisher => _messagePublisher;
        /// <summary>
        /// Gets the publication address provider that was specified during initialization.
        /// </summary>
        public IPublicationAddressProvider PublicationAddressProvider => _publicationAddressProvider;
        /// <summary>
        /// Gets the log data serializer that was specified during initialization.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;

        /// <summary>
        /// Writes log data models to the destination.
        /// </summary>
        /// <param name="logs">Log models array.</param>
        public void Send(LogModel[] logs)
        {
            IModel channel = null;
            for (int i = 0; i < logs.Length; i++)
            {
                // Prepare the message.
                byte[] messageBytes = Encoding.UTF8.GetBytes(_logSerializer.Serialize(logs[i]) ?? string.Empty);

                // Determine the publication address that will be used to publish the message.
                PublicationAddress publicationAddress = _publicationAddressProvider.GetPublicationAddress(logs[i]);

                // Create a new channel if previous has been closed.
                if (channel == null || channel.IsClosed)
                    channel = _rabbitMqConnection.CreateModel();

                // Publish the message.
                _messagePublisher.Publish(channel, publicationAddress, messageBytes);
            }
        }
        /// <summary>
        /// Asynchronously writes log data models to the destination.
        /// </summary>
        /// <param name="logs">Log models array.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
            Send(logs);
            return Task.CompletedTask;
        }
        public void Dispose()
        {

        }
    }
}
