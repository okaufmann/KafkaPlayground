namespace Producer.Interfaces;

public interface IKafkaProducerService
{
    Task ProduceAsync(string topic, string key, string message);
}