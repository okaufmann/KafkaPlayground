using Confluent.Kafka;
using Consumer.Interfaces;

namespace Consumer.Services;

public class KafkaConsumerService(IConsumer<string, string> consumer) : IKafkaConsumerService
{
    private bool _consuming;

    public void StartConsuming(string topic, CancellationToken cancellationToken)
    {
        consumer.Subscribe(topic);
        _consuming = true;

        Task.Run(() =>
        {
            try
            {
                while (_consuming)
                {
                    var cr = consumer.Consume(cancellationToken);
                    Console.WriteLine(
                        $"Received key '{cr.Message.Key}' message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }, cancellationToken);
    }

    public void StopConsuming()
    {
        _consuming = false;
        consumer?.Close();
    }
}