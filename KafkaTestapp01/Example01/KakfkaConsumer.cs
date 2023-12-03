namespace KafkaTestapp01.Example01;

using Confluent.Kafka;

public class KafkaConsumer
{
    public static void Consume(string topic, string bootstrapServers, string groupId)
    {
        var conf = new ConsumerConfig
        {
            GroupId = groupId,
            BootstrapServers = bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
        consumer.Subscribe(topic);

        try
        {
            while (true)
            {
                var cr = consumer.Consume();
                Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}
