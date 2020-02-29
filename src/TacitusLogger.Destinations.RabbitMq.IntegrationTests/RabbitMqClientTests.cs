using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TacitusLogger.Destinations.RabbitMq.IntegrationTests
{
    [TestFixture]
    public class RabbitMqClientTests
    {
        RabbitMqTestManager _rabbitMqTestManager;

        [TearDown]
        public void TearDown()
        {
            _rabbitMqTestManager.CleanUp();
        }

        [SetUp]
        public void SetUp()
        {
            _rabbitMqTestManager = new RabbitMqTestManager();
        }

        [Test]
        public void RabbitMq_Basic_Publish_And_Basic_Get()
        {
            // Arrange
            IModel model1 = _rabbitMqTestManager.Connection.CreateModel();
            string exchangeName = _rabbitMqTestManager.DeclareExchangeQueuePairAndReturnName();
            model1.BasicPublish(exchangeName, "", null, Encoding.UTF8.GetBytes("int tests1"));
            model1.BasicPublish(exchangeName, "", null, Encoding.UTF8.GetBytes("int tests2"));
            model1.BasicPublish(exchangeName, "", null, Encoding.UTF8.GetBytes("int tests3"));

            // Act
            var messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(3, messages.Count);
            Assert.AreEqual("int tests1", messages[0]);
            Assert.AreEqual("int tests2", messages[1]);
            Assert.AreEqual("int tests3", messages[2]);
        }

        [Test]
        public void RabbitMq_Basic_Publish_And_Basic_Get_Called_Given_That_Messages_Already_Consumed()
        {
            // Arrange
            IModel model1 = _rabbitMqTestManager.Connection.CreateModel();
            string exchangeName = _rabbitMqTestManager.DeclareExchangeQueuePairAndReturnName();
            model1.BasicPublish(exchangeName, "", null, Encoding.UTF8.GetBytes("int tests1"));
            model1.BasicPublish(exchangeName, "", null, Encoding.UTF8.GetBytes("int tests2"));
            model1.BasicPublish(exchangeName, "", null, Encoding.UTF8.GetBytes("int tests3"));
            _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Act 
            var messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(0, messages.Count);
        }

        [Test]
        public void When_One_Model_From_Connection_Get_Corrupted_It_Gets_Closed_And_Cannot_Be_Used()
        {
            // Create and corrupt the first model.
            IModel modelThatWillError = _rabbitMqTestManager.Connection.CreateModel();
            try
            {
                modelThatWillError.BasicGet("not_existing_queue", false);
            }
            catch (Exception) { }
            Assert.Catch<AlreadyClosedException>(() =>
            {
                // Try to use same model again 
                modelThatWillError.BasicPublish("some_exchange", "", null, Encoding.UTF8.GetBytes("int tests1"));
            });
        }

        [Test]
        public void When_One_Model_From_Connection_Get_Corrupted_New_Created_Model_Works_Correctly()
        {
            // Create and corrupt the first model.
            IModel modelThatWillError = _rabbitMqTestManager.Connection.CreateModel();
            try
            {
                modelThatWillError.BasicGet("not_existing_queue", false);
            }
            catch (Exception) { }
            // Create new model from the same connection that should work correctly.
            IModel newModel = _rabbitMqTestManager.Connection.CreateModel();
            string exchangeName = _rabbitMqTestManager.DeclareExchangeQueuePairAndReturnName();
            newModel.BasicPublish(exchangeName, "", null, Encoding.UTF8.GetBytes("int tests1"));

            // Act
            var messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual("int tests1", messages[0]);
        }
    }
}

