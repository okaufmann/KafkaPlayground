using Confluent.Kafka;
using Consumer.Interfaces;

namespace Consumer.Services;

public class KafkaConsumerService(string bootstrapServers, string groupId) : IKafkaConsumerService
{
    private readonly ConsumerConfig _config = new()
    {
        GroupId = groupId,
        BootstrapServers = bootstrapServers,
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    private IConsumer<Ignore, string> _consumer;
    private bool _consuming;

    public void StartConsuming(string topic, CancellationToken cancellationToken)
    {
        _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        _consumer.Subscribe(topic);
        _consuming = true;

        Task.Run(() =>
        {
            try
            {
                while (_consuming)
                {
                    var cr = _consumer.Consume(cancellationToken);
                    Console.WriteLine($"Received message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }, cancellationToken);
    }

    public void StopConsuming()
    {
        _consuming = false;
        _consumer?.Close();
    }
}