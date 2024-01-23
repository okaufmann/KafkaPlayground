using Confluent.Kafka;
using Producer.Interfaces;

namespace Producer.Services;

public class KafkaProducerService(IProducer<string, string> producer) : IKafkaProducerService
{
    public async Task ProduceAsync(string topic, string key, string message)
    {
        await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = message });
        // Note: In a production environment, you should handle potential exceptions and failures.
    }
}