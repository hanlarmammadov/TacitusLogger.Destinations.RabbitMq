using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.RabbitMq.UnitTests
{
    [TestFixture]
    public class LogGroupDestinationsBuilderRabbitMqExtensionsTests
    {
        [Test]
        public void RabbitMq_When_Called_Returns_New_RabbitMqDestinationBuilder_Initialized_With_Provided_ILogGroupDestinationsBuilder()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            IRabbitMqDestinationBuilder rabbitMqDestinationBuilderReturned = LogGroupDestinationsBuilderRabbitMqExtensions.RabbitMq(logGroupDestinationsBuilder);

            // Assert
            Assert.IsInstanceOf<RabbitMqDestinationBuilder>(rabbitMqDestinationBuilderReturned);
            Assert.AreEqual(logGroupDestinationsBuilder, ((RabbitMqDestinationBuilder)rabbitMqDestinationBuilderReturned).LogGroupDestinationsBuilder);
        }

    }
}
