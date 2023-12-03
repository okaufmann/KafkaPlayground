using Producer.Interfaces;

namespace Producer.Services;

using Confluent.Kafka;


public class KafkaProducerService(string bootstrapServers) : IKafkaProducerService
{
    private readonly ProducerConfig _config = new() { BootstrapServers = bootstrapServers };

    public async Task ProduceAsync(string topic, string message)
    {
        using var producer = new ProducerBuilder<Null, string>(_config).Build();
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        // Note: In a production environment, you should handle potential exceptions and failures.
    }
}