using Confluent.Kafka;
using Producer.Interfaces;

namespace Producer.Services;

public class KafkaProducerService(IProducer<Null, string> producer) : IKafkaProducerService
{
    public async Task ProduceAsync(string topic, string message)
    {
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        // Note: In a production environment, you should handle potential exceptions and failures.
    }
}