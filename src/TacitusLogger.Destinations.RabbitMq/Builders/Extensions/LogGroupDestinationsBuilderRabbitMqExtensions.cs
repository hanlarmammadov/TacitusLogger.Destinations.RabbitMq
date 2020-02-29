using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.RabbitMq
{
    /// <summary>
    /// Adds Tacitus logger RabbitMQ destination builder extension method to <c>TacitusLogger.Builders.ILogGroupDestinationsBuilder</c> interface and its implementations.
    /// </summary>
    public static class LogGroupDestinationsBuilderRabbitMqExtensions
    {
        /// <summary>
        /// Initiate the adding a RabbitMQ destination to the log group builder.
        /// </summary> 
        /// <returns>Tacitus logger RabbitMQ destination builder.</returns>
        public static IRabbitMqDestinationBuilder RabbitMq(this ILogGroupDestinationsBuilder obj)
        {
            return new RabbitMqDestinationBuilder(obj);
        }
    }
}
