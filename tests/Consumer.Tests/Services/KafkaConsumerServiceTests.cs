using Confluent.Kafka;
using Consumer.Services;
using Moq;

namespace Consumer.Tests.Services;

[TestFixture]
public class KafkaConsumerServiceTests
{
    [SetUp]
    public void Setup()
    {
        _mockConsumer = new Mock<IConsumer<Ignore, string>>();
        _consumerService = new KafkaConsumerService(_mockConsumer.Object);
        _cancellationTokenSource = new CancellationTokenSource();

        // Mock the Consume method to return a dummy message
        _mockConsumer.Setup(c => c.Consume(It.IsAny<CancellationToken>()))
            .Returns(new ConsumeResult<Ignore, string>
            {
                Message = new Message<Ignore, string> { Value = "Test Message" }
            });
    }

    private Mock<IConsumer<Ignore, string>> _mockConsumer;
    private KafkaConsumerService _consumerService;
    private CancellationTokenSource _cancellationTokenSource;

    [Test]
    public void StartConsuming_SubscribesToTopic()
    {
        var testTopic = "test-topic";

        // Act
        _consumerService.StartConsuming(testTopic, _cancellationTokenSource.Token);

        // Assert
        _mockConsumer.Verify(c => c.Subscribe(testTopic), Times.Once());
    }

    [Test]
    public void StopConsuming_ClosesConsumer()
    {
        // Act
        _consumerService.StopConsuming();

        // Assert
        _mockConsumer.Verify(c => c.Close(), Times.Once());
    }
}