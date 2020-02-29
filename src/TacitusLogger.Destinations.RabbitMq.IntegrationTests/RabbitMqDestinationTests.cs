using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using TacitusLogger.Serializers;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.Destinations.RabbitMq.IntegrationTests
{
    [TestFixture]
    public class RabbitMqDestinationTests
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
        public void RabbitMqDestination_With_Stub_Log_Serializer_Sends_Serialized_Text_To_RabbitMq()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("serialized text");
            //
            string exchangeName = _rabbitMqTestManager.DeclareExchangeQueuePairAndReturnName();
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(_rabbitMqTestManager.Connection, logSerializerMock.Object, exchangeName, "direct");

            // Act
            rabbitMqDestination.Send(new LogModel[] { new LogModel() });
            List<string> messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual("serialized text", messages[0]);
        }

        [Test]
        public void RabbitMqDestination_With_Stub_Log_Serializer_Takes_Two_Logs_And_Sends_Serialized_Texts_To_RabbitMq()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.SetupSequence(x => x.Serialize(It.IsAny<LogModel>())).Returns("serialized text1")
                                                                                  .Returns("serialized text2");
            //
            string exchangeName = _rabbitMqTestManager.DeclareExchangeQueuePairAndReturnName();
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(_rabbitMqTestManager.Connection, logSerializerMock.Object, exchangeName, "direct");

            // Act
            rabbitMqDestination.Send(new LogModel[] { new LogModel(), new LogModel() });
            List<string> messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual("serialized text1", messages[0]);
            Assert.AreEqual("serialized text2", messages[1]);
        }

        [Test]
        public void RabbitMqDestination_With_Stub_Log_Serializer_Send_Called_Two_Times_Sends_Serialized_Texts_To_RabbitMq()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.SetupSequence(x => x.Serialize(It.IsAny<LogModel>())).Returns("serialized text1")
                                                                                  .Returns("serialized text2");
            //
            string exchangeName = _rabbitMqTestManager.DeclareExchangeQueuePairAndReturnName();
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(_rabbitMqTestManager.Connection, logSerializerMock.Object, exchangeName, "direct");

            // Act
            rabbitMqDestination.Send(new LogModel[] { new LogModel() });
            rabbitMqDestination.Send(new LogModel[] { new LogModel() });
            List<string> messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual("serialized text1", messages[0]);
            Assert.AreEqual("serialized text2", messages[1]);
        }

        [TestCase(2)]
        [TestCase(5)]
        [TestCase(20)]
        public void RabbitMqDestination_With_Stub_Log_Serializer_Takes_Two_Logs_And_Send_Serialized_Texts_To_RabbitMq(int numberOfLogs)
        {
            // Arrange
            LogModel[] logs = new LogModel[numberOfLogs];
            for (int i = 0; i < numberOfLogs; i++)
                logs[i] = new LogModel() { LogId = i.ToString() };

            var logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel ld) => $"log{ld.LogId}");
            //
            string exchangeName = _rabbitMqTestManager.DeclareExchangeQueuePairAndReturnName();
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(_rabbitMqTestManager.Connection, logSerializerMock.Object, exchangeName, "direct");

            // Act
            rabbitMqDestination.Send(logs);
            List<string> messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(numberOfLogs, messages.Count);
            for (int i = 0; i < numberOfLogs; i++)
                Assert.AreEqual($"log{logs[i].LogId}", messages[i]);
        }

        [Test]
        public void RabbitMqDestination_Given_That_Exchange_Not_Exists_Does_Not_Creates_It()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("serialized text");
            //
            string exchangeName = "not_existing_exchange";
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(_rabbitMqTestManager.Connection, logSerializerMock.Object, exchangeName, "direct");

            // Act
            rabbitMqDestination.Send(new LogModel[] { new LogModel() });
            List<string> messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(0, messages.Count);
        }

        [Test]
        public void RabbitMqDestination_With_Json_Log_Serializer_Sends_Json_To_RabbitMq()
        {
            // Arrange 
            string exchangeName = _rabbitMqTestManager.DeclareExchangeQueuePairAndReturnName();
            RabbitMqDestination rabbitMqDestination = new RabbitMqDestination(_rabbitMqTestManager.Connection, new JsonLogSerializer(), exchangeName, "direct");
            var logModel = Samples.LogModels.Standard();

            // Act
            rabbitMqDestination.Send(new LogModel[] { logModel });
            List<string> messages = _rabbitMqTestManager.GetMessagesFromQueue(exchangeName);

            // Assert
            Assert.AreEqual(1, messages.Count);
            var expectedMessageStr = JsonConvert.SerializeObject(new SerializableLogModel(logModel));
            Assert.AreEqual(expectedMessageStr, messages[0]);
        }
    }
}
