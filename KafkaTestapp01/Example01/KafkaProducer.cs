namespace KafkaTestapp01.Example01;

using Confluent.Kafka;

public class KafkaProducer
{
    public static async Task Produce(string topic, string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        try
        {
            var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = "Hello Kafka!" });
            Console.WriteLine($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}