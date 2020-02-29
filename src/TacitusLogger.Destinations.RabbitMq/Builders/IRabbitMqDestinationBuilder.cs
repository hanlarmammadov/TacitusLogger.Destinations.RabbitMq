using RabbitMQ.Client;
using TacitusLogger.Builders;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Builds and adds an instance of <c>TacitusLogger.Destinations.RabbitMq.RabbitMqDestination</c> class to the <c>LogGroupDestinationsBuilder</c>.
    /// </summary>
    public interface IRabbitMqDestinationBuilder : IDestinationBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rabbitMqConnection"></param>
        /// <returns></returns>
        IRabbitMqDestinationBuilder WithConnection(IConnection rabbitMqConnection);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagePublisher"></param>
        /// <returns></returns>
        IRabbitMqDestinationBuilder WithPublisher(IMessagePublisher messagePublisher);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicationAddressProvider"></param>
        /// <returns></returns>
        IRabbitMqDestinationBuilder WithAddress(IPublicationAddressProvider publicationAddressProvider);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logSerializer"></param>
        /// <returns></returns>
        IRabbitMqDestinationBuilder WithLogSerializer(ILogSerializer logSerializer);
    }
}
