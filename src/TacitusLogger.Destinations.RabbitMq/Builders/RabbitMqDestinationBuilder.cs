using RabbitMQ.Client;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Builds and adds an instance of <c>TacitusLogger.Destinations.RabbitMq.RabbitMqDestination</c> class to the <c>LogGroupDestinationsBuilder</c>.
    /// </summary>
    public class RabbitMqDestinationBuilder : IRabbitMqDestinationBuilder
    {
        private readonly ILogGroupDestinationsBuilder _logGroupDestinationsBuilder;
        private IConnection _rabbitMqConnection;
        private IMessagePublisher _messagePublisher;
        private IPublicationAddressProvider _publicationAddressProvider;
        private ILogSerializer _logSerializer;


        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.RabbitMq.RabbitMqDestinationBuilder</c> using parent <c>ILogGroupDestinationsBuilder</c> instance.
        /// </summary>
        /// <param name="logGroupDestinationsBuilder"></param>
        public RabbitMqDestinationBuilder(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
        {
            _logGroupDestinationsBuilder = logGroupDestinationsBuilder;
        }


        /// <summary>
        /// Gets <c>ILogGroupDestinationsBuilder</c> specified during the initialization. 
        /// </summary>
        public ILogGroupDestinationsBuilder LogGroupDestinationsBuilder => _logGroupDestinationsBuilder;
        /// <summary>
        /// Gets the RabbitMQ connection specified during the build process. 
        /// </summary>
        public IConnection RabbitMqConnection => _rabbitMqConnection;
        /// <summary>
        /// Gets the message publisher specified during the build process. 
        /// </summary>
        public IMessagePublisher MessagePublisher => _messagePublisher;
        /// <summary>
        /// Gets the publication address provider specified during the build process. 
        /// </summary>
        public IPublicationAddressProvider PublicationAddressProvider => _publicationAddressProvider;
        /// <summary>
        /// Gets the log data serializer specified during the build process. 
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;


        /// <summary>
        /// Adds the RabbitMQ connection of type <c>RabbitMQ.Client.IConnection</c> that will
        /// be used to open and utilize AMQP channels.
        /// </summary>
        /// <param name="rabbitMqConnection">Opened RabbitMQ connection.</param>
        /// <returns>Self.</returns>
        public IRabbitMqDestinationBuilder WithConnection(IConnection rabbitMqConnection)
        {
            if (_rabbitMqConnection != null)
                throw new InvalidOperationException("RabbitMQ connection has already been set during this build");
            _rabbitMqConnection = rabbitMqConnection ?? throw new ArgumentNullException("rabbitMqConnection");
            return this;
        }
        /// <summary>
        /// Adds the message publisher of type <c>TacitusLogger.Destinations.RabbitMq.IMessagePublisher</c> that will be used send messages to
        /// the RabbitMQ.
        /// </summary>
        /// <param name="messagePublisher">The message publisher.</param>
        /// <returns>Self.</returns>
        public IRabbitMqDestinationBuilder WithPublisher(IMessagePublisher messagePublisher)
        {
            if (_messagePublisher != null)
                throw new InvalidOperationException("Message publisher has already been set during this build");
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException("messagePublisher");
            return this;
        }
        /// <summary>
        /// Adds the publication address provider of type <c>TacitusLogger.Destinations.RabbitMq.IPublicationAddressProvider</c> that is used
        /// by RabbitMQ destination to get publication addresses of type <c>RabbitMQ.Client.PublicationAddress</c>.
        /// </summary>
        /// <param name="publicationAddressProvider">The publication address provider.</param>
        /// <returns>Self.</returns>
        public IRabbitMqDestinationBuilder WithAddress(IPublicationAddressProvider publicationAddressProvider)
        {
            if (_publicationAddressProvider != null)
                throw new InvalidOperationException("Publication address provider has already been set during this build");
            _publicationAddressProvider = publicationAddressProvider ?? throw new ArgumentNullException("publicationAddressProvider");
            return this;
        }
        /// <summary>
        /// Adds the log data serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c> that is used to generate
        /// RabbitMQ message strings of specific format and content.
        /// </summary>
        /// <param name="logSerializer">The log data serializer.</param>
        /// <returns>Self.</returns>
        public IRabbitMqDestinationBuilder WithLogSerializer(ILogSerializer logSerializer)
        {
            if (_logSerializer != null)
                throw new InvalidOperationException("Log data serializer has already been set during this build");
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            return this;
        }


        /// <summary>
        /// Completes log destination build process by adding it to the parent log group destination builder.
        /// </summary>
        /// <returns></returns>
        public ILogGroupDestinationsBuilder Add()
        {
            // Mandatory dependencies.
            if (_rabbitMqConnection == null)
                throw new InvalidOperationException("RabbitMQ connection was not specified during the build");
            if (_publicationAddressProvider == null)
                throw new InvalidOperationException("Publication address provider was not specified during the build");

            // Dependencies with defaults.
            if (_messagePublisher == null)
                _messagePublisher = new BasicMessagePublisher();
            if (_logSerializer == null)
                _logSerializer = new JsonLogSerializer();
             
            // Create the destination.
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(_rabbitMqConnection, _publicationAddressProvider, _logSerializer, _messagePublisher);

            // Add to log group and return it.
            return _logGroupDestinationsBuilder.CustomDestination(rabbitMqDestination);
        }
    }
}
