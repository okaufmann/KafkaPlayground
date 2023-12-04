using Confluent.Kafka;
using Moq;
using Producer.Services;

namespace Producer.Tests.Services;

[TestFixture]
public class KafkaProducerServiceTests
{
    [SetUp]
    public void Setup()
    {
        _mockProducer = new Mock<IProducer<Null, string>>();
        _producerService = new KafkaProducerService(_mockProducer.Object);

        // Mock the ProduceAsync method with all required arguments
        _mockProducer.Setup(producer => producer.ProduceAsync(
                It.IsAny<string>(),
                It.IsAny<Message<Null, string>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeliveryResult<Null, string>());
    }

    private Mock<IProducer<Null, string>> _mockProducer;
    private KafkaProducerService _producerService;

    [Test]
    public async Task ProduceAsync_SendsMessage()
    {
        var testTopic = "test-topic";
        var testMessage = "Hello Kafka";

        // Act
        await _producerService.ProduceAsync(testTopic, testMessage);

        // Assert
        _mockProducer.Verify(producer => producer.ProduceAsync(
                testTopic,
                It.Is<Message<Null, string>>(m => m.Value == testMessage),
                It.IsAny<CancellationToken>()),
            Times.Once());
    }
}